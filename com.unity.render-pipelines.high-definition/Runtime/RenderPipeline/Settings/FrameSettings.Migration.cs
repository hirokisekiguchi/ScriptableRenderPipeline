using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    [Obsolete("For data migration")]
    public enum ObsoleteLitShaderMode
    {
        Forward,
        Deferred
    }

    [Flags, Obsolete("For data migration")]
    enum ObsoleteLightLoopSettingsOverrides
    {
        FptlForForwardOpaque = 1 << 0,
        BigTilePrepass = 1 << 1,
        ComputeLightEvaluation = 1 << 2,
        ComputeLightVariants = 1 << 3,
        ComputeMaterialVariants = 1 << 4,
        TileAndCluster = 1 << 5,
        //Fptl = 1 << 6, //isFptlEnabled set up by system
    }

    [Flags, Obsolete("For data migration")]
    enum ObsoleteFrameSettingsOverrides
    {
        //lighting settings
        Shadow = 1 << 0,
        ContactShadow = 1 << 1,
        ShadowMask = 1 << 2,
        SSR = 1 << 3,
        SSAO = 1 << 4,
        SubsurfaceScattering = 1 << 5,
        Transmission = 1 << 6,
        AtmosphericScaterring = 1 << 7,
        Volumetrics = 1 << 8,
        ReprojectionForVolumetrics = 1 << 9,
        LightLayers = 1 << 10,
        MSAA = 1 << 11,

        //rendering pass
        TransparentPrepass = 1 << 13,
        TransparentPostpass = 1 << 14,
        MotionVectors = 1 << 15,
        ObjectMotionVectors = 1 << 16,
        Decals = 1 << 17,
        RoughRefraction = 1 << 18,
        Distortion = 1 << 19,
        Postprocess = 1 << 20,

        //rendering settings
        ShaderLitMode = 1 << 21,
        DepthPrepassWithDeferredRendering = 1 << 22,
        OpaqueObjects = 1 << 24,
        TransparentObjects = 1 << 25,
        RealtimePlanarReflection = 1 << 26,

        // Async settings
        AsyncCompute = 1 << 23,
        LightListAsync = 1 << 27,
        SSRAsync = 1 << 28,
        SSAOAsync = 1 << 29,
        ContactShadowsAsync = 1 << 30,
        VolumeVoxelizationsAsync = 1 << 31,
    }

    [Serializable, Obsolete("For data migration")]
    class ObsoleteLightLoopSettings
    {
        public ObsoleteLightLoopSettingsOverrides overrides;
        public bool enableTileAndCluster;
        public bool enableComputeLightEvaluation;
        public bool enableComputeLightVariants;
        public bool enableComputeMaterialVariants;
        public bool enableFptlForForwardOpaque;
        public bool enableBigTilePrepass;
        public bool isFptlEnabled;
    }

    // The settings here are per frame settings.
    // Each camera must have its own per frame settings
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("FrameSettings overriding {overrides.ToString(\"X\")}")]
    [Obsolete("For data migration")]
    class ObsoleteFrameSettings
    {
        public ObsoleteFrameSettingsOverrides overrides;

        public bool enableShadow;
        public bool enableContactShadows;
        public bool enableShadowMask;
        public bool enableSSR;
        public bool enableSSAO;
        public bool enableSubsurfaceScattering;
        public bool enableTransmission;  
        public bool enableAtmosphericScattering;
        public bool enableVolumetrics;
        public bool enableReprojectionForVolumetrics;
        public bool enableLightLayers;
        
        public float diffuseGlobalDimmer;
        public float specularGlobalDimmer;
        
        public ObsoleteLitShaderMode shaderLitMode;
        public bool enableDepthPrepassWithDeferredRendering;

        public bool enableTransparentPrepass;
        public bool enableMotionVectors; // Enable/disable whole motion vectors pass (Camera + Object).
        public bool enableObjectMotionVectors;
        [FormerlySerializedAs("enableDBuffer")]
        public bool enableDecals;
        public bool enableRoughRefraction; // Depends on DepthPyramid - If not enable, just do a copy of the scene color (?) - how to disable rough refraction ?
        public bool enableTransparentPostpass;
        public bool enableDistortion;
        public bool enablePostprocess;

        public bool enableOpaqueObjects;
        public bool enableTransparentObjects;
        public bool enableRealtimePlanarReflection;

        public bool enableMSAA;
        
        public bool enableAsyncCompute;
        public bool runLightListAsync;
        public bool runSSRAsync;
        public bool runSSAOAsync;
        public bool runContactShadowsAsync;
        public bool runVolumeVoxelizationAsync;
        
        public ObsoleteLightLoopSettings lightLoopSettings;

        int m_LitShaderModeEnumIndex; 
    }

    public partial struct FrameSettings
    {
        //handle migration here
    }
}
