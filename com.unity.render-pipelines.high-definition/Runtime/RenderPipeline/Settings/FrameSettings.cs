using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public enum LitShaderMode
    {
        Forward,
        Deferred
    }
    
    public enum FrameSettingsField
    {
        //lighting settings from 0 to 19
        Shadow = 0,
        ContactShadow = 1,
        ShadowMask = 2,
        SSR = 3,
        SSAO = 4,
        SubsurfaceScattering = 5,
        Transmission = 6,
        AtmosphericScaterring = 7,
        Volumetrics = 8,
        ReprojectionForVolumetrics = 9,
        LightLayers = 10,
        MSAA = 11,

        //rendering pass from 20 to 39
        TransparentPrepass = 20,
        TransparentPostpass = 21,
        MotionVectors = 22,
        ObjectMotionVectors = 23,
        Decals = 24,
        RoughRefraction = 25,
        Distortion = 26,
        Postprocess = 27,

        //rendering settings from 40 to 59
        ShaderLitMode = 40,
        DepthPrepassWithDeferredRendering = 41,
        OpaqueObjects = 42,
        TransparentObjects = 43,
        RealtimePlanarReflection = 44,

        //async settings from 60 to 79
        AsyncCompute = 60,
        LightListAsync = 61,
        SSRAsync = 62,
        SSAOAsync = 63,
        ContactShadowsAsync = 64,
        VolumeVoxelizationsAsync = 65,

        //from 80 to 119 : space for new scopes

        //lightLoop settings from 120 to 128
        FptlForForwardOpaque = 120,
        BigTilePrepass = 121,
        ComputeLightEvaluation = 122,
        ComputeLightVariants = 123,
        ComputeMaterialVariants = 124,
        TileAndCluster = 125,
        //126 is IsFptlEnabled. Do not use it
    }

    // The settings here are per frame settings.
    // Each camera must have its own per frame settings
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("FrameSettings overriding {overrides.ToString(\"X\")}")]
    public struct FrameSettings
    {
        static Dictionary<FrameSettingsField, Action<FrameSettings, FrameSettings>> s_Overrides = new Dictionary<FrameSettingsField, Action<FrameSettings, FrameSettings>>
        {
            {FrameSettingsField.Shadow, (a, b) => { a.enableShadow = b.enableShadow; } },
            {FrameSettingsField.ContactShadow, (a, b) => { a.enableContactShadows = b.enableContactShadows; } },
            {FrameSettingsField.ShadowMask, (a, b) => { a.enableShadowMask = b.enableShadowMask; } },
            {FrameSettingsField.SSR, (a, b) => { a.enableSSR = b.enableSSR; } },
            {FrameSettingsField.SSAO, (a, b) => { a.enableSSAO = b.enableSSAO; } },
            {FrameSettingsField.SubsurfaceScattering, (a, b) => { a.enableSubsurfaceScattering = b.enableSubsurfaceScattering; } },
            {FrameSettingsField.Transmission, (a, b) => { a.enableTransmission = b.enableTransmission; } },
            {FrameSettingsField.AtmosphericScaterring, (a, b) => { a.enableAtmosphericScattering = b.enableAtmosphericScattering; } },
            {FrameSettingsField.Volumetrics, (a, b) => { a.enableVolumetrics = b.enableVolumetrics; } },
            {FrameSettingsField.ReprojectionForVolumetrics, (a, b) => { a.enableReprojectionForVolumetrics = b.enableReprojectionForVolumetrics; } },
            {FrameSettingsField.LightLayers, (a, b) => { a.enableLightLayers = b.enableLightLayers; } },
            {FrameSettingsField.MSAA, (a, b) => { a.enableMSAA = b.enableMSAA; } },
            {FrameSettingsField.TransparentPrepass, (a, b) => { a.enableTransparentPrepass = b.enableTransparentPrepass; } },
            {FrameSettingsField.TransparentPostpass, (a, b) => { a.enableTransparentPostpass = b.enableTransparentPostpass; } },
            {FrameSettingsField.MotionVectors, (a, b) => { a.enableMotionVectors = b.enableMotionVectors; } },
            {FrameSettingsField.ObjectMotionVectors, (a, b) => { a.enableObjectMotionVectors = b.enableObjectMotionVectors; } },
            {FrameSettingsField.Decals, (a, b) => { a.enableDecals = b.enableDecals; } },
            {FrameSettingsField.RoughRefraction, (a, b) => { a.enableRoughRefraction = b.enableRoughRefraction; } },
            {FrameSettingsField.Distortion, (a, b) => { a.enableDistortion = b.enableDistortion; } },
            {FrameSettingsField.Postprocess, (a, b) => { a.enablePostprocess = b.enablePostprocess; } },
            {FrameSettingsField.ShaderLitMode, (a, b) => { a.shaderLitMode = b.shaderLitMode; } },
            {FrameSettingsField.DepthPrepassWithDeferredRendering, (a, b) => { a.enableDepthPrepassWithDeferredRendering = b.enableDepthPrepassWithDeferredRendering; } },
            {FrameSettingsField.AsyncCompute, (a, b) => { a.enableAsyncCompute = b.enableAsyncCompute; } },
            {FrameSettingsField.OpaqueObjects, (a, b) => { a.enableOpaqueObjects = b.enableOpaqueObjects; } },
            {FrameSettingsField.TransparentObjects, (a, b) => { a.enableTransparentObjects = b.enableTransparentObjects; } },
            {FrameSettingsField.RealtimePlanarReflection, (a, b) => { a.enableRealtimePlanarReflection = b.enableRealtimePlanarReflection; } },
            {FrameSettingsField.LightListAsync, (a, b) => { a.runLightListAsync = b.runLightListAsync; } },
            {FrameSettingsField.SSRAsync, (a, b) => { a.runSSRAsync= b.runSSRAsync; } },
            {FrameSettingsField.SSAOAsync, (a, b) => { a.runSSAOAsync = b.runSSAOAsync; } },
            {FrameSettingsField.ContactShadowsAsync, (a, b) => { a.runContactShadowsAsync = b.runContactShadowsAsync; } },
            {FrameSettingsField.VolumeVoxelizationsAsync, (a, b) => { a.runVolumeVoxelizationAsync = b.runVolumeVoxelizationAsync; } }
            {FrameSettingsField.FptlForForwardOpaque, (a, b) => { a.enableFptlForForwardOpaque = b.enableFptlForForwardOpaque; } },
            {FrameSettingsField.BigTilePrepass, (a, b) => { a.enableBigTilePrepass = b.enableBigTilePrepass; } },
            {FrameSettingsField.ComputeLightEvaluation, (a, b) => { a.enableComputeLightEvaluation = b.enableComputeLightEvaluation; } },
            {FrameSettingsField.ComputeLightVariants, (a, b) => { a.enableComputeLightVariants = b.enableComputeLightVariants; } },
            {FrameSettingsField.ComputeMaterialVariants, (a, b) => { a.enableComputeMaterialVariants = b.enableComputeMaterialVariants; } },
            {FrameSettingsField.TileAndCluster, (a, b) => { a.enableTileAndCluster = b.enableTileAndCluster; } },
        };

        [SerializeField]
        CheapBoolArray128 overrides;

        [SerializeField]
        CheapBoolArray128 boolData;

        public FrameSettings(bool unused)
        {
            overrides = new CheapBoolArray128();
            boolData = new CheapBoolArray128(new uint[] {
                (uint)FrameSettingsField.Shadow,
                (uint)FrameSettingsField.ContactShadow,
                (uint)FrameSettingsField.ShadowMask,
                (uint)FrameSettingsField.SSAO,
                (uint)FrameSettingsField.SubsurfaceScattering,
                (uint)FrameSettingsField.Transmission,   // Caution: this is only for debug, it doesn't save the cost of Transmission execution
                (uint)FrameSettingsField.AtmosphericScaterring,
                (uint)FrameSettingsField.Volumetrics,
                (uint)FrameSettingsField.ReprojectionForVolumetrics,
                (uint)FrameSettingsField.LightLayers,
                (uint)FrameSettingsField.ShaderLitMode, //deffered ; enum with only two value saved as a bool
                (uint)FrameSettingsField.TransparentPrepass,
                (uint)FrameSettingsField.TransparentPostpass,
                (uint)FrameSettingsField.MotionVectors, // Enable/disable whole motion vectors pass (Camera + Object).
                (uint)FrameSettingsField.ObjectMotionVectors,
                (uint)FrameSettingsField.Decals,
                (uint)FrameSettingsField.RoughRefraction, // Depends on DepthPyramid - If not enable, just do a copy of the scene color (?) - how to disable rough refraction ?
                (uint)FrameSettingsField.Distortion,
                (uint)FrameSettingsField.Postprocess,
                (uint)FrameSettingsField.OpaqueObjects,
                (uint)FrameSettingsField.TransparentObjects,
                (uint)FrameSettingsField.RealtimePlanarReflection,
                (uint)FrameSettingsField.AsyncCompute,
                (uint)FrameSettingsField.LightListAsync,
                (uint)FrameSettingsField.SSRAsync,
                (uint)FrameSettingsField.SSRAsync,
                (uint)FrameSettingsField.SSAOAsync,
                (uint)FrameSettingsField.ContactShadowsAsync,
                (uint)FrameSettingsField.VolumeVoxelizationsAsync,
                (uint)FrameSettingsField.TileAndCluster,
                (uint)FrameSettingsField.ComputeLightEvaluation,
                (uint)FrameSettingsField.ComputeLightVariants,
                (uint)FrameSettingsField.ComputeMaterialVariants,
                (uint)FrameSettingsField.FptlForForwardOpaque,
                (uint)FrameSettingsField.BigTilePrepass,
                126u //isFptlEnabled
            });

            diffuseGlobalDimmer = 1f;
            specularGlobalDimmer = 1f;
            m_LitShaderModeEnumIndex = 1; //match Deferred index
        }

        // Setup by system
        public float diffuseGlobalDimmer;
        public float specularGlobalDimmer;

        // View
        public LitShaderMode shaderLitMode
        {
            get => boolData[(uint)FrameSettingsField.ShaderLitMode] ? LitShaderMode.Deferred : LitShaderMode.Forward;
            set => boolData[(uint)FrameSettingsField.ShaderLitMode] = value == LitShaderMode.Deferred;
        }
        
        //saved enum fields for when repainting Debug Menu
        int m_LitShaderModeEnumIndex;   
        
        public void ApplyOverrideOn(FrameSettings overridedFrameSettings)
        {
            if(overrides.allFalse)
                return;

            Array values = Enum.GetValues(typeof(FrameSettingsField));
            foreach(FrameSettingsField val in values)
            {
                if(overrides[(uint)val])
                {
                    overridedFrameSettings.boolData[(uint)val] = boolData[(uint)val];
                    //s_Overrides[val](overridedFrameSettings, this);
                }
            }

            //propagate override to be chained
            overridedFrameSettings.overrides |= overrides;

            //refresh enums for DebugMenu
            overridedFrameSettings.Refresh();
        }

        // Init a FrameSettings from renderpipeline settings, frame settings and debug settings (if any)
        // This will aggregate the various option
        public static void InitializeFrameSettings(Camera camera, RenderPipelineSettings renderPipelineSettings, FrameSettings srcFrameSettings, ref FrameSettings aggregate)
        {
            if (aggregate == null)
                aggregate = new FrameSettings();

            // When rendering reflection probe we disable specular as it is view dependent
            if (camera.cameraType == CameraType.Reflection)
            {
                aggregate.diffuseGlobalDimmer = 1.0f;
                aggregate.specularGlobalDimmer = 0.0f;
            }
            else
            {
                aggregate.diffuseGlobalDimmer = 1.0f;
                aggregate.specularGlobalDimmer = 1.0f;
            }

            aggregate.enableShadow = srcFrameSettings.enableShadow;
            aggregate.enableContactShadows = srcFrameSettings.enableContactShadows;
            aggregate.enableShadowMask = srcFrameSettings.enableShadowMask && renderPipelineSettings.supportShadowMask;
            aggregate.enableSSR = camera.cameraType != CameraType.Reflection && srcFrameSettings.enableSSR && renderPipelineSettings.supportSSR; // No recursive reflections
            aggregate.enableSSAO = srcFrameSettings.enableSSAO && renderPipelineSettings.supportSSAO;
            aggregate.enableSubsurfaceScattering = camera.cameraType != CameraType.Reflection && srcFrameSettings.enableSubsurfaceScattering && renderPipelineSettings.supportSubsurfaceScattering;
            aggregate.enableTransmission = srcFrameSettings.enableTransmission;
            aggregate.enableAtmosphericScattering = srcFrameSettings.enableAtmosphericScattering;
            // We must take care of the scene view fog flags in the editor
            if (!CoreUtils.IsSceneViewFogEnabled(camera))
                aggregate.enableAtmosphericScattering = false;
            // Volumetric are disabled if there is no atmospheric scattering
            aggregate.enableVolumetrics = srcFrameSettings.enableVolumetrics && renderPipelineSettings.supportVolumetrics && aggregate.enableAtmosphericScattering;
            aggregate.enableReprojectionForVolumetrics = srcFrameSettings.enableReprojectionForVolumetrics;

            aggregate.enableLightLayers = srcFrameSettings.enableLightLayers && renderPipelineSettings.supportLightLayers;

            // We have to fall back to forward-only rendering when scene view is using wireframe rendering mode
            // as rendering everything in wireframe + deferred do not play well together
            if (GL.wireframe) //force forward mode for wireframe
            {
                aggregate.shaderLitMode = LitShaderMode.Forward;
            }
            else
            {
                switch (renderPipelineSettings.supportedLitShaderMode)
                {
                    case RenderPipelineSettings.SupportedLitShaderMode.ForwardOnly:
                        aggregate.shaderLitMode = LitShaderMode.Forward;
                        break;
                    case RenderPipelineSettings.SupportedLitShaderMode.DeferredOnly:
                        aggregate.shaderLitMode = LitShaderMode.Deferred;
                        break;
                    case RenderPipelineSettings.SupportedLitShaderMode.Both:
                        aggregate.shaderLitMode = srcFrameSettings.shaderLitMode;
                        break;
                }
            }

            aggregate.enableDepthPrepassWithDeferredRendering = srcFrameSettings.enableDepthPrepassWithDeferredRendering;

            aggregate.enableTransparentPrepass = srcFrameSettings.enableTransparentPrepass && renderPipelineSettings.supportTransparentDepthPrepass;
            aggregate.enableMotionVectors = camera.cameraType != CameraType.Reflection && srcFrameSettings.enableMotionVectors && renderPipelineSettings.supportMotionVectors;
            // Object motion vector are disabled if motion vector are disabled
            aggregate.enableObjectMotionVectors = srcFrameSettings.enableObjectMotionVectors && aggregate.enableMotionVectors;
            aggregate.enableDecals = srcFrameSettings.enableDecals && renderPipelineSettings.supportDecals;
            aggregate.enableRoughRefraction = srcFrameSettings.enableRoughRefraction;
            aggregate.enableTransparentPostpass = srcFrameSettings.enableTransparentPostpass && renderPipelineSettings.supportTransparentDepthPostpass;
            aggregate.enableDistortion = camera.cameraType != CameraType.Reflection && srcFrameSettings.enableDistortion && renderPipelineSettings.supportDistortion;

            // Planar and real time cubemap doesn't need post process and render in FP16
            aggregate.enablePostprocess = camera.cameraType != CameraType.Reflection && srcFrameSettings.enablePostprocess;
                        
            aggregate.enableAsyncCompute = srcFrameSettings.enableAsyncCompute && SystemInfo.supportsAsyncCompute;
            aggregate.runLightListAsync = aggregate.enableAsyncCompute && srcFrameSettings.runLightListAsync;
            aggregate.runSSRAsync = aggregate.enableAsyncCompute && srcFrameSettings.runSSRAsync;
            aggregate.runSSAOAsync = aggregate.enableAsyncCompute && srcFrameSettings.runSSAOAsync;
            aggregate.runContactShadowsAsync = aggregate.enableAsyncCompute && srcFrameSettings.runContactShadowsAsync;
            aggregate.runVolumeVoxelizationAsync = aggregate.enableAsyncCompute && srcFrameSettings.runVolumeVoxelizationAsync;

            aggregate.enableOpaqueObjects = srcFrameSettings.enableOpaqueObjects;
            aggregate.enableTransparentObjects = srcFrameSettings.enableTransparentObjects;
            aggregate.enableRealtimePlanarReflection = srcFrameSettings.enableRealtimePlanarReflection;       

            //MSAA only supported in forward
            aggregate.enableMSAA = srcFrameSettings.enableMSAA && renderPipelineSettings.supportMSAA && aggregate.shaderLitMode == LitShaderMode.Forward;

            aggregate.ConfigureMSAADependentSettings();
            aggregate.ConfigureStereoDependentSettings(camera);

            // Disable various option for the preview except if we are a Camera Editor preview
            if (HDUtils.IsRegularPreviewCamera(camera))
            {
                aggregate.enableShadow = false;
                aggregate.enableContactShadows = false;
                aggregate.enableShadowMask = false;
                aggregate.enableSSR = false;
                aggregate.enableSSAO = false;
                aggregate.enableAtmosphericScattering = false;
                aggregate.enableVolumetrics = false;
                aggregate.enableReprojectionForVolumetrics = false;
                aggregate.enableLightLayers = false;
                aggregate.enableTransparentPrepass = false;
                aggregate.enableMotionVectors = false;
                aggregate.enableObjectMotionVectors = false;
                aggregate.enableDecals = false;
                aggregate.enableTransparentPostpass = false;
                aggregate.enableDistortion = false;
                aggregate.enablePostprocess = false;
            }

            ObsoleteLightLoopSettings.InitializeLightLoopSettings(camera, aggregate, renderPipelineSettings, srcFrameSettings, ref aggregate.lightLoopSettings);

            aggregate.Refresh();
        }

        void Refresh()
        {
            // actually, we need to sync up changes done in the debug menu too
            switch(shaderLitMode)
            {
                case LitShaderMode.Forward:
                    m_LitShaderModeEnumIndex = 0;
                    break;
                case LitShaderMode.Deferred:
                    m_LitShaderModeEnumIndex = 1;
                    break;
                default:
                    throw new ArgumentException("Unknown LitShaderMode");
            }
        }

        public bool BuildLightListRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && enableAsyncCompute && runLightListAsync;
        }

        public bool SSRRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && enableAsyncCompute && runSSRAsync;
        }

        public bool SSAORunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && enableAsyncCompute && runSSAOAsync;
        }

        public bool ContactShadowsRunAsync()
        {
            return SystemInfo.supportsAsyncCompute && enableAsyncCompute && runContactShadowsAsync;
        }

        public bool VolumeVoxelizationRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && enableAsyncCompute && runVolumeVoxelizationAsync;
        }


        public void ConfigureMSAADependentSettings()
        {
            if (enableMSAA)
            {
                // Initially, MSAA will only support forward
                shaderLitMode = LitShaderMode.Forward;

                // TODO: The work will be implemented piecemeal to support all passes
                enableDistortion = false; // no gaussian final color
                enableSSR = false;
            }
        }

        public void ConfigureStereoDependentSettings(Camera cam)
        {
            if (cam.stereoEnabled)
            {
                // Stereo deferred rendering still has the following problems:
                // VR TODO: Dispatch tile light-list compute per-eye
                // VR TODO: Update compute lighting shaders for stereo
                shaderLitMode = LitShaderMode.Forward;

                // TODO: The work will be implemented piecemeal to support all passes
                enableMotionVectors = enablePostprocess && !enableMSAA;
                enableSSR = false;
            }
        }


        public static void RegisterDebug(string menuName, FrameSettings frameSettings)
        {
            List<DebugUI.Widget> widgets = new List<DebugUI.Widget>();
            widgets.AddRange(
            new DebugUI.Widget[]
            {
                new DebugUI.Foldout
                {
                    displayName = "Rendering Passes",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable Transparent Prepass", getter = () => frameSettings.enableTransparentPrepass, setter = value => frameSettings.enableTransparentPrepass = value },
                        new DebugUI.BoolField { displayName = "Enable Transparent Postpass", getter = () => frameSettings.enableTransparentPostpass, setter = value => frameSettings.enableTransparentPostpass = value },
                        new DebugUI.BoolField { displayName = "Enable Motion Vectors", getter = () => frameSettings.enableMotionVectors, setter = value => frameSettings.enableMotionVectors = value },
                        new DebugUI.BoolField { displayName = "  Enable Object Motion Vectors", getter = () => frameSettings.enableObjectMotionVectors, setter = value => frameSettings.enableObjectMotionVectors = value },
                        new DebugUI.BoolField { displayName = "Enable DBuffer", getter = () => frameSettings.enableDecals, setter = value => frameSettings.enableDecals = value },
                        new DebugUI.BoolField { displayName = "Enable Rough Refraction", getter = () => frameSettings.enableRoughRefraction, setter = value => frameSettings.enableRoughRefraction = value },
                        new DebugUI.BoolField { displayName = "Enable Distortion", getter = () => frameSettings.enableDistortion, setter = value => frameSettings.enableDistortion = value },
                        new DebugUI.BoolField { displayName = "Enable Postprocess", getter = () => frameSettings.enablePostprocess, setter = value => frameSettings.enablePostprocess = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Rendering Settings",
                    children =
                    {
                        new DebugUI.EnumField { displayName = "Lit Shader Mode", getter = () => (int)frameSettings.shaderLitMode, setter = value => frameSettings.shaderLitMode = (LitShaderMode)value, autoEnum = typeof(LitShaderMode), getIndex = () => frameSettings.m_LitShaderModeEnumIndex, setIndex = value => frameSettings.m_LitShaderModeEnumIndex = value },
                        new DebugUI.BoolField { displayName = "Deferred Depth Prepass", getter = () => frameSettings.enableDepthPrepassWithDeferredRendering, setter = value => frameSettings.enableDepthPrepassWithDeferredRendering = value },
                        new DebugUI.BoolField { displayName = "Enable Opaque Objects", getter = () => frameSettings.enableOpaqueObjects, setter = value => frameSettings.enableOpaqueObjects = value },
                        new DebugUI.BoolField { displayName = "Enable Transparent Objects", getter = () => frameSettings.enableTransparentObjects, setter = value => frameSettings.enableTransparentObjects = value },
                        new DebugUI.BoolField { displayName = "Enable Realtime Planar Reflection", getter = () => frameSettings.enableRealtimePlanarReflection, setter = value => frameSettings.enableRealtimePlanarReflection = value },                        
                        new DebugUI.BoolField { displayName = "Enable MSAA", getter = () => frameSettings.enableMSAA, setter = value => frameSettings.enableMSAA = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Lighting Settings",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable SSR", getter = () => frameSettings.enableSSR, setter = value => frameSettings.enableSSR = value },
                        new DebugUI.BoolField { displayName = "Enable SSAO", getter = () => frameSettings.enableSSAO, setter = value => frameSettings.enableSSAO = value },
                        new DebugUI.BoolField { displayName = "Enable SubsurfaceScattering", getter = () => frameSettings.enableSubsurfaceScattering, setter = value => frameSettings.enableSubsurfaceScattering = value },
                        new DebugUI.BoolField { displayName = "Enable Transmission", getter = () => frameSettings.enableTransmission, setter = value => frameSettings.enableTransmission = value },
                        new DebugUI.BoolField { displayName = "Enable Shadows", getter = () => frameSettings.enableShadow, setter = value => frameSettings.enableShadow = value },
                        new DebugUI.BoolField { displayName = "Enable Contact Shadows", getter = () => frameSettings.enableContactShadows, setter = value => frameSettings.enableContactShadows = value },
                        new DebugUI.BoolField { displayName = "Enable ShadowMask", getter = () => frameSettings.enableShadowMask, setter = value => frameSettings.enableShadowMask = value },
                        new DebugUI.BoolField { displayName = "Enable Atmospheric Scattering", getter = () => frameSettings.enableAtmosphericScattering, setter = value => frameSettings.enableAtmosphericScattering = value },
                        new DebugUI.BoolField { displayName = "Enable Volumetrics", getter = () => frameSettings.enableVolumetrics, setter = value => frameSettings.enableVolumetrics = value },
                        new DebugUI.BoolField { displayName = "Enable Reprojection For Volumetrics", getter = () => frameSettings.enableReprojectionForVolumetrics, setter = value => frameSettings.enableReprojectionForVolumetrics = value },
                        new DebugUI.BoolField { displayName = "Enable LightLayers", getter = () => frameSettings.enableLightLayers, setter = value => frameSettings.enableLightLayers = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Async Compute Settings",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable Async Compute", getter = () => frameSettings.enableAsyncCompute, setter = value => frameSettings.enableAsyncCompute = value },
                        new DebugUI.BoolField { displayName = "Run Build Light List Async", getter = () => frameSettings.runLightListAsync, setter = value => frameSettings.runLightListAsync = value },
                        new DebugUI.BoolField { displayName = "Run SSR Async", getter = () => frameSettings.runSSRAsync, setter = value => frameSettings.runSSRAsync = value },
                        new DebugUI.BoolField { displayName = "Run SSAO Async", getter = () => frameSettings.runSSAOAsync, setter = value => frameSettings.runSSAOAsync = value },
                        new DebugUI.BoolField { displayName = "Run Contact Shadows Async", getter = () => frameSettings.runContactShadowsAsync, setter = value => frameSettings.runContactShadowsAsync = value },
                        new DebugUI.BoolField { displayName = "Run Volume Voxelization Async", getter = () => frameSettings.runVolumeVoxelizationAsync, setter = value => frameSettings.runVolumeVoxelizationAsync = value },
                    }
                }
            });

            ObsoleteLightLoopSettings.RegisterDebug(frameSettings.lightLoopSettings, widgets);

            var panel = DebugManager.instance.GetPanel(menuName, true);
            panel.children.Add(widgets.ToArray());
        }

        public static void UnRegisterDebug(string menuName)
        {
            DebugManager.instance.RemovePanel(menuName);
        }
    }
}
