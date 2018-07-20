using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace UnityEditor.Experimental.Rendering.HDPipeline
{
    /// <summary>
    /// Contains the logic of the reflection probe baking ticking.
    /// </summary>
    unsafe struct HDReflectionSystemTick
    {
        // Algorithm settings
        public ReflectionSettings settings;

        // Injected resources by callee
        public BakedProbeHashes bakedProbeHashes;
        public HDProbeTickedRenderer tickedRenderer;
        public HDReflectionEntityManager2 entityManager;

        internal void Tick(SceneStateHash sceneStateHash, IScriptableBakedReflectionSystemStageNotifier handle)
        {
            // Explanation of the algorithm:
            // 1. First we create the hash of the world that can impact the reflection probes.
            // 2. Then for each probe, we calculate a hash that represent what this specific probe should have baked.
            // 3. We compare those hashes against the baked one and decide:
            //   a. If we have to remove a baked data
            //   b. If we have to bake a probe
            // 4. Then we check with the renderer what to bake:
            //   a. If the baking is in flight, we continue and another tick will acknowledge its completion
            //   b. Otherwise, we cancel the baking a restart a new one
            // 5. When baking is complete:
            //   a. Remove the unused baked data
            //   b. Write asset files to disk

            // = Step 1 =
            // Allocate stack variables
            var bakedProbeCount = entityManager.BakedProbeCount;
            var bakedProbeOnlyHashes = stackalloc Hash128[bakedProbeCount];
            var bakedProbeIDs = stackalloc HDReflectionEntityID[bakedProbeCount];
            ComputeProbeStateHashesAndGetEntityIDs(
                entityManager.GetActiveBakedProbeEnumerator(),
                bakedProbeOnlyHashes, bakedProbeIDs
            );

            var allBakedProbeHash = new Hash128();
            // Baked probes depends on other baked probes when baking several bounces
            ComputeAllBakedProbeHashWithBounces(
                &allBakedProbeHash, bakedProbeCount, bakedProbeOnlyHashes, settings.bounces
            );

            // Baked probes depends on probes with custom textures
            var allCustomProbeHash = new Hash128();
            ComputeAllCustomProbeHash(entityManager.GetActiveCustomProbeEnumerator(), ref allCustomProbeHash);
            HashUtilities.AppendHash(ref allCustomProbeHash, ref allBakedProbeHash);

            // TODO: calculate a custom hash for light that hashes additional data as well.
            // Baked probes depends on scene gameobjects (static geometry, lights)
            var sceneObjectHash = sceneStateHash.sceneObjectsHash;
            HashUtilities.AppendHash(ref sceneObjectHash, ref allBakedProbeHash);
            // Baked probes depends on sky settings
            var skySettingsHash = sceneStateHash.skySettingsHash;
            HashUtilities.AppendHash(ref skySettingsHash, ref allBakedProbeHash);

            // = Step 2 =
            // Compute the hash of the data that should have been baked for each probe.
            var bakedProbeOutputHashes = stackalloc Hash128[bakedProbeCount];
            ComputeBakedProbeOutputHashes(
                &allBakedProbeHash,
                bakedProbeCount,
                bakedProbeOnlyHashes, bakedProbeOutputHashes
            );

            // = Step 3 =
            var maxProbeCount = Mathf.Max(bakedProbeCount, bakedProbeHashes.count);
            var addIndices = stackalloc int[maxProbeCount];
            var remIndices = stackalloc int[maxProbeCount];
            fixed (Hash128* oldHashes = bakedProbeHashes.probeOutputHashes)
            {
                int addCount = 0, remCount = 0;
                // The actual comparison happens here
                // addIndicies will hold indices of probes to bake
                // remIndicies will hold indices of baked data to delete
                if (Utilities.CompareHashes(
                    bakedProbeHashes.count, oldHashes,          // old hashes
                    bakedProbeCount, bakedProbeOutputHashes,    // new hashes
                    addIndices, remIndices,
                    out addCount, out remCount
                    ) > 0)
                {
                    // Notify Unity we are baking probes
                    handle.EnterStage(
                        (int)HDReflectionSystem.BakeStages.ReflectionProbes,
                        string.Format("Reflection Probes | {0} jobs", addCount),
                        Utilities.CalculateProgress(addCount, bakedProbeCount)
                    );

                    // = Step 4 =
                    // No probe to bake == we already baked all probes properly
                    var bakingComplete = addCount == 0;
                    // Check if the renderer is currently baking the probes
                    if (!bakingComplete)
                    {
                        var allProbeOutputHash = new Hash128();
                        Utilities.CombineHashes(bakedProbeCount, bakedProbeOutputHashes, &allProbeOutputHash);
                        if (tickedRenderer.isComplete && tickedRenderer.inputHash == allProbeOutputHash)
                            bakingComplete = true;
                        else
                        {
                            // We must restart the renderer with the new data
                            tickedRenderer.Cancel();
                            var toBakeIDs = stackalloc HDReflectionEntityID[addCount];
                            Utilities.CopyToIndirect(
                                addCount, addIndices,
                                (byte*)bakedProbeIDs, (byte*)toBakeIDs,
                                UnsafeUtility.SizeOf<HDReflectionEntityID>()
                            );
                            tickedRenderer.Start(
                                allProbeOutputHash,
                                settings,
                                addCount, toBakeIDs
                            );
                        }
                    }

                    if (!bakingComplete && !tickedRenderer.isComplete)
                        // Do one job this tick
                        bakingComplete = tickedRenderer.Tick();

                    // = Step 5 =
                    if (bakingComplete)
                    {
                        // TODO:
                        //   a. Remove the unused baked data
                        //   b. Write asset files to disk
                    }

                    return;
                }
            }

            // Notify Unity we completed the baking
            handle.ExitStage((int)HDReflectionSystem.BakeStages.ReflectionProbes);
            handle.SetIsDone(true);
        }



        void ComputeBakedProbeOutputHashes(
            Hash128* allBakedProbeHash,
            int bakedProbeCount,
            Hash128* bakedProbeOnlyHashes, Hash128* bakedProbeOutputHashes
        )
        {
            for (int i = 0; i < bakedProbeCount; ++i)
            {
                bakedProbeOutputHashes[i] = bakedProbeOnlyHashes[i]; // copy probe only data
                HashUtilities.AppendHash(ref *allBakedProbeHash, ref bakedProbeOutputHashes[i]);
            }
        }

        void ComputeProbeStateHashesAndGetEntityIDs(
            IEnumerator<HDProbe> enumerator,
            Hash128* bakedProbeOnlyHashes,
            HDReflectionEntityID* bakedProbeIDs
        )
        {
            var i = 0;
            while (enumerator.MoveNext())
            {
                var bakedProbe = enumerator.Current;
                bakedProbeIDs[i] = bakedProbe.entityId;
                bakedProbeOnlyHashes[i] = bakedProbe.ComputeBakePropertyHashes();
                ++i;
            }
        }

        void ComputeAllCustomProbeHash(IEnumerator<HDProbe> enumerator, ref Hash128 hash)
        {
            while (enumerator.MoveNext())
            {
                var customProbe = enumerator.Current;
                var customHash = customProbe.customProperties.customTextureHash;
                HashUtilities.AppendHash(ref customHash, ref hash);
            }
        }

        void ComputeAllBakedProbeHashWithBounces(
            Hash128* allProbeHash,
            int bakedProbeCount, Hash128* bakedProbeOnlyHashes,
            uint bounces
        )
        {
            if (bounces > 1)
            {
                // Adding a dependency to other baked probes
                for (int i = 0; i < bakedProbeCount; ++i)
                    HashUtilities.AppendHash(ref bakedProbeOnlyHashes[i], ref *allProbeHash);
            }
        }
    }
}
