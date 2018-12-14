using UnityEditor.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace UnityEditor.Experimental.Rendering.HDPipeline
{
    public class SerializedLightLoopSettings
    {
        public SerializedProperty root;

        public SerializedProperty enableTileAndCluster;
        public SerializedProperty enableComputeLightEvaluation;
        public SerializedProperty enableComputeLightVariants;
        public SerializedProperty enableComputeMaterialVariants;
        public SerializedProperty enableFptlForForwardOpaque;
        public SerializedProperty enableBigTilePrepass;
        public SerializedProperty isFptlEnabled;

        private SerializedProperty overrides;
        public bool overridesFptlForForwardOpaque
        {
            get { return (overrides.intValue & (int)ObsoleteLightLoopSettingsOverrides.FptlForForwardOpaque) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)ObsoleteLightLoopSettingsOverrides.FptlForForwardOpaque;
                else
                    overrides.intValue &= ~(int)ObsoleteLightLoopSettingsOverrides.FptlForForwardOpaque;
            }
        }
        public bool overridesBigTilePrepass
        {
            get { return (overrides.intValue & (int)ObsoleteLightLoopSettingsOverrides.BigTilePrepass) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)ObsoleteLightLoopSettingsOverrides.BigTilePrepass;
                else
                    overrides.intValue &= ~(int)ObsoleteLightLoopSettingsOverrides.BigTilePrepass;
            }
        }
        public bool overridesComputeLightEvaluation
        {
            get { return (overrides.intValue & (int)ObsoleteLightLoopSettingsOverrides.ComputeLightEvaluation) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)ObsoleteLightLoopSettingsOverrides.ComputeLightEvaluation;
                else
                    overrides.intValue &= ~(int)ObsoleteLightLoopSettingsOverrides.ComputeLightEvaluation;
            }
        }
        public bool overridesComputeLightVariants
        {
            get { return (overrides.intValue & (int)ObsoleteLightLoopSettingsOverrides.ComputeLightVariants) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)ObsoleteLightLoopSettingsOverrides.ComputeLightVariants;
                else
                    overrides.intValue &= ~(int)ObsoleteLightLoopSettingsOverrides.ComputeLightVariants;
            }
        }
        public bool overridesComputeMaterialVariants
        {
            get { return (overrides.intValue & (int)ObsoleteLightLoopSettingsOverrides.ComputeMaterialVariants) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)ObsoleteLightLoopSettingsOverrides.ComputeMaterialVariants;
                else
                    overrides.intValue &= ~(int)ObsoleteLightLoopSettingsOverrides.ComputeMaterialVariants;
            }
        }
        public bool overridesTileAndCluster
        {
            get { return (overrides.intValue & (int)ObsoleteLightLoopSettingsOverrides.TileAndCluster) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)ObsoleteLightLoopSettingsOverrides.TileAndCluster;
                else
                    overrides.intValue &= ~(int)ObsoleteLightLoopSettingsOverrides.TileAndCluster;
            }
        }

        public SerializedLightLoopSettings(SerializedProperty root)
        {
            this.root = root;

            enableTileAndCluster = root.Find((ObsoleteLightLoopSettings l) => l.enableTileAndCluster);
            enableComputeLightEvaluation = root.Find((ObsoleteLightLoopSettings l) => l.enableComputeLightEvaluation);
            enableComputeLightVariants = root.Find((ObsoleteLightLoopSettings l) => l.enableComputeLightVariants);
            enableComputeMaterialVariants = root.Find((ObsoleteLightLoopSettings l) => l.enableComputeMaterialVariants);
            enableFptlForForwardOpaque = root.Find((ObsoleteLightLoopSettings l) => l.enableFptlForForwardOpaque);
            enableBigTilePrepass = root.Find((ObsoleteLightLoopSettings l) => l.enableBigTilePrepass);
            isFptlEnabled = root.Find((ObsoleteLightLoopSettings l) => l.isFptlEnabled);

            overrides = root.Find((ObsoleteLightLoopSettings l) => l.overrides);
        }
    }
}
