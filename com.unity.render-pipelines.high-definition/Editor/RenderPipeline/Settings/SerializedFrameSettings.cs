using UnityEditor.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace UnityEditor.Experimental.Rendering.HDPipeline
{
    class SerializedFrameSettings
    {
        public SerializedProperty root;

        public SerializedProperty enableShadow;
        public SerializedProperty enableContactShadow;
        public SerializedProperty enableSSR;
        public SerializedProperty enableSSAO;
        public SerializedProperty enableSubsurfaceScattering;
        public SerializedProperty enableTransmission;
        public SerializedProperty enableAtmosphericScattering;
        public SerializedProperty enableVolumetrics;
        public SerializedProperty enableReprojectionForVolumetrics;
        public SerializedProperty enableLightLayers;

        public SerializedProperty diffuseGlobalDimmer;
        public SerializedProperty specularGlobalDimmer;

        public SerializedProperty litShaderMode;
        public SerializedProperty enableDepthPrepassWithDeferredRendering;

        public SerializedProperty enableTransparentPrepass;
        public SerializedProperty enableMotionVectors;
        public SerializedProperty enableObjectMotionVectors;
        public SerializedProperty enableDecals;
        public SerializedProperty enableRoughRefraction;
        public SerializedProperty enableTransparentPostpass;
        public SerializedProperty enableDistortion;
        public SerializedProperty enablePostprocess;

        public SerializedProperty enableAsyncCompute;
        public SerializedProperty runBuildLightListAsync;
        public SerializedProperty runSSRAsync;
        public SerializedProperty runSSAOAsync;
        public SerializedProperty runContactShadowsAsync;
        public SerializedProperty runVolumeVoxelizationAsync;

        public SerializedProperty enableOpaqueObjects;
        public SerializedProperty enableTransparentObjects;
        public SerializedProperty enableRealtimePlanarReflection;        

        public SerializedProperty enableMSAA;

        public SerializedProperty enableShadowMask;

        public SerializedLightLoopSettings lightLoopSettings;

        private  SerializedProperty overrides;
        public bool overridesShadow
        {
            get { return (overrides.intValue & (int)FrameSettingsField.Shadow) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.Shadow;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.Shadow;
            }
        }
        public bool overridesContactShadow
        {
            get { return (overrides.intValue & (int)FrameSettingsField.ContactShadow) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.ContactShadow;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.ContactShadow;
            }
        }
        public bool overridesShadowMask
        {
            get { return (overrides.intValue & (int)FrameSettingsField.ShadowMask) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.ShadowMask;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.ShadowMask;
            }
        }
        public bool overridesSSR
        {
            get { return (overrides.intValue & (int)FrameSettingsField.SSR) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.SSR;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.SSR;
            }
        }
        public bool overridesSSAO
        {
            get { return (overrides.intValue & (int)FrameSettingsField.SSAO) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.SSAO;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.SSAO;
            }
        }
        public bool overridesSubsurfaceScattering
        {
            get { return (overrides.intValue & (int)FrameSettingsField.SubsurfaceScattering) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.SubsurfaceScattering;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.SubsurfaceScattering;
            }
        }
        public bool overridesTransmission
        {
            get { return (overrides.intValue & (int)FrameSettingsField.Transmission) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.Transmission;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.Transmission;
            }
        }
        public bool overridesAtmosphericScaterring
        {
            get { return (overrides.intValue & (int)FrameSettingsField.AtmosphericScaterring) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.AtmosphericScaterring;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.AtmosphericScaterring;
            }
        }
        public bool overridesVolumetrics
        {
            get { return (overrides.intValue & (int)FrameSettingsField.Volumetrics) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.Volumetrics;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.Volumetrics;
            }
        }
        public bool overridesProjectionForVolumetrics
        {
            get { return (overrides.intValue & (int)FrameSettingsField.ReprojectionForVolumetrics) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.ReprojectionForVolumetrics;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.ReprojectionForVolumetrics;
            }
        }
        public bool overridesLightLayers
        {
            get { return (overrides.intValue & (int)FrameSettingsField.LightLayers) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.LightLayers;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.LightLayers;
            }
        }
        public bool overridesTransparentPrepass
        {
            get { return (overrides.intValue & (int)FrameSettingsField.TransparentPrepass) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.TransparentPrepass;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.TransparentPrepass;
            }
        }
        public bool overridesTransparentPostpass
        {
            get { return (overrides.intValue & (int)FrameSettingsField.TransparentPostpass) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.TransparentPostpass;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.TransparentPostpass;
            }
        }
        public bool overridesMotionVectors
        {
            get { return (overrides.intValue & (int)FrameSettingsField.MotionVectors) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.MotionVectors;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.MotionVectors;
            }
        }
        public bool overridesObjectMotionVectors
        {
            get { return (overrides.intValue & (int)FrameSettingsField.ObjectMotionVectors) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.ObjectMotionVectors;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.ObjectMotionVectors;
            }
        }
        public bool overridesDecals
        {
            get { return (overrides.intValue & (int)FrameSettingsField.Decals) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.Decals;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.Decals;
            }
        }
        public bool overridesRoughRefraction
        {
            get { return (overrides.intValue & (int)FrameSettingsField.RoughRefraction) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.RoughRefraction;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.RoughRefraction;
            }
        }
        public bool overridesDistortion
        {
            get { return (overrides.intValue & (int)FrameSettingsField.Distortion) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.Distortion;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.Distortion;
            }
        }
        public bool overridesPostprocess
        {
            get { return (overrides.intValue & (int)FrameSettingsField.Postprocess) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.Postprocess;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.Postprocess;
            }
        }
        public bool overridesShaderLitMode
        {
            get { return (overrides.intValue & (int)FrameSettingsField.ShaderLitMode) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.ShaderLitMode;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.ShaderLitMode;
            }
        }
        public bool overridesDepthPrepassWithDeferredRendering
        {
            get { return (overrides.intValue & (int)FrameSettingsField.DepthPrepassWithDeferredRendering) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.DepthPrepassWithDeferredRendering;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.DepthPrepassWithDeferredRendering;
            }
        }
        public bool overridesAsyncCompute
        {
            get { return (overrides.intValue & (int)FrameSettingsField.AsyncCompute) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.AsyncCompute;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.AsyncCompute;
            }
        }

        public bool overrideLightListInAsync
        {
            get { return (overrides.intValue & (int)FrameSettingsField.LightListAsync) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.LightListAsync;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.LightListAsync;
            }
        }

        public bool overrideSSRInAsync
        {
            get { return (overrides.intValue & (int)FrameSettingsField.SSRAsync) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.SSRAsync;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.SSRAsync;
            }
        }

        public bool overrideSSAOInAsync
        {
            get { return (overrides.intValue & (int)FrameSettingsField.SSAOAsync) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.SSAOAsync;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.SSAOAsync;
            }
        }

        public bool overrideContactShadowsInAsync
        {
            get { return (overrides.intValue & (int)FrameSettingsField.ContactShadowsAsync) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.ContactShadowsAsync;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.ContactShadowsAsync;
            }
        }

        public bool overrideVolumeVoxelizationInAsync
        {
            get { return (overrides.intValue & (int)FrameSettingsField.VolumeVoxelizationsAsync) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.VolumeVoxelizationsAsync;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.VolumeVoxelizationsAsync;
            }
        }

        public bool overridesOpaqueObjects
        {
            get { return (overrides.intValue & (int)FrameSettingsField.OpaqueObjects) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.OpaqueObjects;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.OpaqueObjects;
            }
        }
        public bool overridesTransparentObjects
        {
            get { return (overrides.intValue & (int)FrameSettingsField.TransparentObjects) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.TransparentObjects;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.TransparentObjects;
            }
        }

        public bool overridesRealtimePlanarReflection
        {
            get { return (overrides.intValue & (int)FrameSettingsField.RealtimePlanarReflection) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.RealtimePlanarReflection;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.RealtimePlanarReflection;
            }
        }        

        public bool overridesMSAA
        {
            get { return (overrides.intValue & (int)FrameSettingsField.MSAA) > 0; }
            set
            {
                if (value)
                    overrides.intValue |= (int)FrameSettingsField.MSAA;
                else
                    overrides.intValue &= ~(int)FrameSettingsField.MSAA;
            }
        }

        public SerializedFrameSettings(SerializedProperty root)
        {
            this.root = root;

            enableShadow = root.Find((FrameSettings d) => d.enableShadow);
            enableContactShadow = root.Find((FrameSettings d) => d.enableContactShadows);
            enableSSR = root.Find((FrameSettings d) => d.enableSSR);
            enableSSAO = root.Find((FrameSettings d) => d.enableSSAO);
            enableSubsurfaceScattering = root.Find((FrameSettings d) => d.enableSubsurfaceScattering);
            enableTransmission = root.Find((FrameSettings d) => d.enableTransmission);
            enableAtmosphericScattering = root.Find((FrameSettings d) => d.enableAtmosphericScattering);
            enableVolumetrics = root.Find((FrameSettings d) => d.enableVolumetrics);
            enableReprojectionForVolumetrics = root.Find((FrameSettings d) => d.enableReprojectionForVolumetrics);
            enableLightLayers = root.Find((FrameSettings d) => d.enableLightLayers);
            diffuseGlobalDimmer = root.Find((FrameSettings d) => d.diffuseGlobalDimmer);
            specularGlobalDimmer = root.Find((FrameSettings d) => d.specularGlobalDimmer);
            litShaderMode = root.Find((FrameSettings d) => d.shaderLitMode);
            enableDepthPrepassWithDeferredRendering = root.Find((FrameSettings d) => d.enableDepthPrepassWithDeferredRendering);
            enableTransparentPrepass = root.Find((FrameSettings d) => d.enableTransparentPrepass);
            enableMotionVectors = root.Find((FrameSettings d) => d.enableMotionVectors);
            enableObjectMotionVectors = root.Find((FrameSettings d) => d.enableObjectMotionVectors);
            enableDecals = root.Find((FrameSettings d) => d.enableDecals);
            enableRoughRefraction = root.Find((FrameSettings d) => d.enableRoughRefraction);
            enableTransparentPostpass = root.Find((FrameSettings d) => d.enableTransparentPostpass);
            enableDistortion = root.Find((FrameSettings d) => d.enableDistortion);
            enablePostprocess = root.Find((FrameSettings d) => d.enablePostprocess);
            enableAsyncCompute = root.Find((FrameSettings d) => d.enableAsyncCompute);
            runBuildLightListAsync = root.Find((FrameSettings d) => d.runLightListAsync);
            runSSRAsync = root.Find((FrameSettings d) => d.runSSRAsync);
            runSSAOAsync = root.Find((FrameSettings d) => d.runSSAOAsync);
            runContactShadowsAsync = root.Find((FrameSettings d) => d.runContactShadowsAsync);
            runVolumeVoxelizationAsync = root.Find((FrameSettings d) => d.runVolumeVoxelizationAsync);
            enableOpaqueObjects = root.Find((FrameSettings d) => d.enableOpaqueObjects);
            enableTransparentObjects = root.Find((FrameSettings d) => d.enableTransparentObjects);
            enableRealtimePlanarReflection = root.Find((FrameSettings d) => d.enableRealtimePlanarReflection);
            enableMSAA = root.Find((FrameSettings d) => d.enableMSAA);
            enableShadowMask = root.Find((FrameSettings d) => d.enableShadowMask);
            overrides = root.Find((FrameSettings d) => d.overrides);

            lightLoopSettings = new SerializedLightLoopSettings(root.Find((FrameSettings d) => d.lightLoopSettings));
        }
    }
}
