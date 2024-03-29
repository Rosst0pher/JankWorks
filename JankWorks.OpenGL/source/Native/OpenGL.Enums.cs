﻿// Sourced from https://github.com/tstavrianos/OpenGL

using System;

namespace JankWorks.Drivers.OpenGL.Native
{
    public static class Enums
    {
        public enum AttribMask : uint
        {
            CurrentBit = 0x00000001,
            PointBit = 0x00000002,
            LineBit = 0x00000004,
            PolygonBit = 0x00000008,
            PolygonStippleBit = 0x00000010,
            PixelModeBit = 0x00000020,
            LightingBit = 0x00000040,
            FogBit = 0x00000080,
            DepthBufferBit = 0x00000100,
            AccumBufferBit = 0x00000200,
            StencilBufferBit = 0x00000400,
            ViewportBit = 0x00000800,
            TransformBit = 0x00001000,
            EnableBit = 0x00002000,
            ColorBufferBit = 0x00004000,
            HintBit = 0x00008000,
            EvalBit = 0x00010000,
            ListBit = 0x00020000,
            TextureBit = 0x00040000,
            ScissorBit = 0x00080000,
            MultisampleBit = 0x20000000,
            MultisampleBitArb = 0x20000000,
            MultisampleBitExt = 0x20000000,
            MultisampleBit3dfx = 0x20000000,
            // Guaranteed to mark all attribute groups at once
            AllAttribBits = 0xFFFFFFFF,
        }

        // GL_MAP_{COHERENT,PERSISTENT,READ,WRITE}_{BIT,BIT_EXT} also lie in this namespace
        public enum BufferStorageMask : uint
        {
            DynamicStorageBit = 0x0100,
            DynamicStorageBitExt = 0x0100,
            ClientStorageBit = 0x0200,
            ClientStorageBitExt = 0x0200,
            SparseStorageBitArb = 0x0400,
            LgpuSeparateStorageBitNvx = 0x0800,
            PerGpuStorageBitNv = 0x0800,
            ExternalStorageBitNvx = 0x2000,
        }

        // GL_{DEPTH,ACCUM,STENCIL,COLOR}_BUFFER_BIT also lie in this namespace
        public enum ClearBufferMask : uint
        {
            // Collides with AttribMask bit GL_HINT_BIT. OK since this token is for OpenGL ES 2, which doesn't have attribute groups.
            CoverageBufferBitNv = 0x00008000,
        }

        public enum ClientAttribMask : uint
        {
            ClientPixelStoreBit = 0x00000001,
            ClientVertexArrayBit = 0x00000002,
            ClientAllAttribBits = 0xFFFFFFFF,
        }

        // Should be shared with WGL/GLX, but aren't since the FORWARD_COMPATIBLE and DEBUG values are swapped vs. WGL/GLX.
        public enum ContextFlagMask : uint
        {
            ContextFlagForwardCompatibleBit = 0x00000001,
            ContextFlagDebugBit = 0x00000002,
            ContextFlagDebugBitKhr = 0x00000002,
            ContextFlagRobustAccessBit = 0x00000004,
            ContextFlagRobustAccessBitArb = 0x00000004,
            ContextFlagNoErrorBit = 0x00000008,
            ContextFlagNoErrorBitKhr = 0x00000008,
            ContextFlagProtectedContentBitExt = 0x00000010,
        }

        public enum ContextProfileMask : uint
        {
            ContextCoreProfileBit = 0x00000001,
            ContextCompatibilityProfileBit = 0x00000002,
        }

        public enum MapBufferAccessMask : uint
        {
            MapReadBit = 0x0001,
            MapReadBitExt = 0x0001,
            MapWriteBit = 0x0002,
            MapWriteBitExt = 0x0002,
            MapInvalidateRangeBit = 0x0004,
            MapInvalidateRangeBitExt = 0x0004,
            MapInvalidateBufferBit = 0x0008,
            MapInvalidateBufferBitExt = 0x0008,
            MapFlushExplicitBit = 0x0010,
            MapFlushExplicitBitExt = 0x0010,
            MapUnsynchronizedBit = 0x0020,
            MapUnsynchronizedBitExt = 0x0020,
            MapPersistentBit = 0x0040,
            MapPersistentBitExt = 0x0040,
            MapCoherentBit = 0x0080,
            MapCoherentBitExt = 0x0080,
        }

        public enum MemoryBarrierMask : uint
        {
            VertexAttribArrayBarrierBit = 0x00000001,
            VertexAttribArrayBarrierBitExt = 0x00000001,
            ElementArrayBarrierBit = 0x00000002,
            ElementArrayBarrierBitExt = 0x00000002,
            UniformBarrierBit = 0x00000004,
            UniformBarrierBitExt = 0x00000004,
            TextureFetchBarrierBit = 0x00000008,
            TextureFetchBarrierBitExt = 0x00000008,
            ShaderGlobalAccessBarrierBitNv = 0x00000010,
            ShaderImageAccessBarrierBit = 0x00000020,
            ShaderImageAccessBarrierBitExt = 0x00000020,
            CommandBarrierBit = 0x00000040,
            CommandBarrierBitExt = 0x00000040,
            PixelBufferBarrierBit = 0x00000080,
            PixelBufferBarrierBitExt = 0x00000080,
            TextureUpdateBarrierBit = 0x00000100,
            TextureUpdateBarrierBitExt = 0x00000100,
            BufferUpdateBarrierBit = 0x00000200,
            BufferUpdateBarrierBitExt = 0x00000200,
            FramebufferBarrierBit = 0x00000400,
            FramebufferBarrierBitExt = 0x00000400,
            TransformFeedbackBarrierBit = 0x00000800,
            TransformFeedbackBarrierBitExt = 0x00000800,
            AtomicCounterBarrierBit = 0x00001000,
            AtomicCounterBarrierBitExt = 0x00001000,
            ShaderStorageBarrierBit = 0x00002000,
            ClientMappedBufferBarrierBit = 0x00004000,
            ClientMappedBufferBarrierBitExt = 0x00004000,
            QueryBufferBarrierBit = 0x00008000,
            AllBarrierBits = 0xFFFFFFFF,
            AllBarrierBitsExt = 0xFFFFFFFF,
        }

        public enum OcclusionQueryEventMaskAMD : uint
        {
            QueryDepthPassEventBitAmd = 0x00000001,
            QueryDepthFailEventBitAmd = 0x00000002,
            QueryStencilFailEventBitAmd = 0x00000004,
            QueryDepthBoundsFailEventBitAmd = 0x00000008,
            QueryAllEventBitsAmd = 0xFFFFFFFF,
        }

        public enum SyncObjectMask : uint
        {
            SyncFlushCommandsBit = 0x00000001,
            SyncFlushCommandsBitApple = 0x00000001,
        }

        public enum UseProgramStageMask : uint
        {
            VertexShaderBit = 0x00000001,
            VertexShaderBitExt = 0x00000001,
            FragmentShaderBit = 0x00000002,
            FragmentShaderBitExt = 0x00000002,
            GeometryShaderBit = 0x00000004,
            GeometryShaderBitExt = 0x00000004,
            GeometryShaderBitOes = 0x00000004,
            TessControlShaderBit = 0x00000008,
            TessControlShaderBitExt = 0x00000008,
            TessControlShaderBitOes = 0x00000008,
            TessEvaluationShaderBit = 0x00000010,
            TessEvaluationShaderBitExt = 0x00000010,
            TessEvaluationShaderBitOes = 0x00000010,
            ComputeShaderBit = 0x00000020,
            MeshShaderBitNv = 0x00000040,
            TaskShaderBitNv = 0x00000080,
            AllShaderBits = 0xFFFFFFFF,
            AllShaderBitsExt = 0xFFFFFFFF,
        }

        public enum TextureStorageMaskAMD : uint
        {
            TextureStorageSparseBitAmd = 0x00000001,
        }

        public enum FragmentShaderDestMaskATI : uint
        {
            RedBitAti = 0x00000001,
            GreenBitAti = 0x00000002,
            BlueBitAti = 0x00000004,
        }

        public enum FragmentShaderDestModMaskATI : uint
        {
            _2xBitAti = 0x00000001,
            _4xBitAti = 0x00000002,
            _8xBitAti = 0x00000004,
            HalfBitAti = 0x00000008,
            QuarterBitAti = 0x00000010,
            EighthBitAti = 0x00000020,
            SaturateBitAti = 0x00000040,
        }

        public enum FragmentShaderColorModMaskATI : uint
        {
            CompBitAti = 0x00000002,
            NegateBitAti = 0x00000004,
            BiasBitAti = 0x00000008,
        }

        public enum TraceMaskMESA : uint
        {
            TraceOperationsBitMesa = 0x0001,
            TracePrimitivesBitMesa = 0x0002,
            TraceArraysBitMesa = 0x0004,
            TraceTexturesBitMesa = 0x0008,
            TracePixelsBitMesa = 0x0010,
            TraceErrorsBitMesa = 0x0020,
            TraceAllBitsMesa = 0xFFFF,
        }

        public enum PathRenderingMaskNV : uint
        {
            BoldBitNv = 0x01,
            ItalicBitNv = 0x02,
            GlyphWidthBitNv = 0x01,
            GlyphHeightBitNv = 0x02,
            GlyphHorizontalBearingXBitNv = 0x04,
            GlyphHorizontalBearingYBitNv = 0x08,
            GlyphHorizontalBearingAdvanceBitNv = 0x10,
            GlyphVerticalBearingXBitNv = 0x20,
            GlyphVerticalBearingYBitNv = 0x40,
            GlyphVerticalBearingAdvanceBitNv = 0x80,
            GlyphHasKerningBitNv = 0x100,
            FontXMinBoundsBitNv = 0x00010000,
            FontYMinBoundsBitNv = 0x00020000,
            FontXMaxBoundsBitNv = 0x00040000,
            FontYMaxBoundsBitNv = 0x00080000,
            FontUnitsPerEmBitNv = 0x00100000,
            FontAscenderBitNv = 0x00200000,
            FontDescenderBitNv = 0x00400000,
            FontHeightBitNv = 0x00800000,
            FontMaxAdvanceWidthBitNv = 0x01000000,
            FontMaxAdvanceHeightBitNv = 0x02000000,
            FontUnderlinePositionBitNv = 0x04000000,
            FontUnderlineThicknessBitNv = 0x08000000,
            FontHasKerningBitNv = 0x10000000,
            FontNumGlyphIndicesBitNv = 0x20000000,
        }

        public enum PerformanceQueryCapsMaskINTEL : uint
        {
            PerfquerySingleContextIntel = 0x00000000,
            PerfqueryGlobalContextIntel = 0x00000001,
        }

        public enum VertexHintsMaskPGI : uint
        {
            Vertex23BitPgi = 0x00000004,
            Vertex4BitPgi = 0x00000008,
            Color3BitPgi = 0x00010000,
            Color4BitPgi = 0x00020000,
            EdgeflagBitPgi = 0x00040000,
            IndexBitPgi = 0x00080000,
            MatAmbientBitPgi = 0x00100000,
            MatAmbientAndDiffuseBitPgi = 0x00200000,
            MatDiffuseBitPgi = 0x00400000,
            MatEmissionBitPgi = 0x00800000,
            MatColorIndexesBitPgi = 0x01000000,
            MatShininessBitPgi = 0x02000000,
            MatSpecularBitPgi = 0x04000000,
            NormalBitPgi = 0x08000000,
            Texcoord1BitPgi = 0x10000000,
            Texcoord2BitPgi = 0x20000000,
            Texcoord3BitPgi = 0x40000000,
            Texcoord4BitPgi = 0x80000000,
        }

        public enum BufferBitQCOM : uint
        {
            ColorBufferBit0Qcom = 0x00000001,
            ColorBufferBit1Qcom = 0x00000002,
            ColorBufferBit2Qcom = 0x00000004,
            ColorBufferBit3Qcom = 0x00000008,
            ColorBufferBit4Qcom = 0x00000010,
            ColorBufferBit5Qcom = 0x00000020,
            ColorBufferBit6Qcom = 0x00000040,
            ColorBufferBit7Qcom = 0x00000080,
            DepthBufferBit0Qcom = 0x00000100,
            DepthBufferBit1Qcom = 0x00000200,
            DepthBufferBit2Qcom = 0x00000400,
            DepthBufferBit3Qcom = 0x00000800,
            DepthBufferBit4Qcom = 0x00001000,
            DepthBufferBit5Qcom = 0x00002000,
            DepthBufferBit6Qcom = 0x00004000,
            DepthBufferBit7Qcom = 0x00008000,
            StencilBufferBit0Qcom = 0x00010000,
            StencilBufferBit1Qcom = 0x00020000,
            StencilBufferBit2Qcom = 0x00040000,
            StencilBufferBit3Qcom = 0x00080000,
            StencilBufferBit4Qcom = 0x00100000,
            StencilBufferBit5Qcom = 0x00200000,
            StencilBufferBit6Qcom = 0x00400000,
            StencilBufferBit7Qcom = 0x00800000,
            MultisampleBufferBit0Qcom = 0x01000000,
            MultisampleBufferBit1Qcom = 0x02000000,
            MultisampleBufferBit2Qcom = 0x04000000,
            MultisampleBufferBit3Qcom = 0x08000000,
            MultisampleBufferBit4Qcom = 0x10000000,
            MultisampleBufferBit5Qcom = 0x20000000,
            MultisampleBufferBit6Qcom = 0x40000000,
            MultisampleBufferBit7Qcom = 0x80000000,
        }

        public enum FoveationConfigBitQCOM : uint
        {
            FoveationEnableBitQcom = 0x00000001,
            FoveationScaledBinMethodBitQcom = 0x00000002,
            FoveationSubsampledLayoutMethodBitQcom = 0x00000004,
        }

        public enum FfdMaskSGIX : uint
        {
            TextureDeformationBitSgix = 0x00000001,
            GeometryDeformationBitSgix = 0x00000002,
        }

        // For NV_command_list.
        public enum CommandOpcodesNV
        {
            TerminateSequenceCommandNv = 0x0000,
            NopCommandNv = 0x0001,
            DrawElementsCommandNv = 0x0002,
            DrawArraysCommandNv = 0x0003,
            DrawElementsStripCommandNv = 0x0004,
            DrawArraysStripCommandNv = 0x0005,
            DrawElementsInstancedCommandNv = 0x0006,
            DrawArraysInstancedCommandNv = 0x0007,
            ElementAddressCommandNv = 0x0008,
            AttributeAddressCommandNv = 0x0009,
            UniformAddressCommandNv = 0x000A,
            BlendColorCommandNv = 0x000B,
            StencilRefCommandNv = 0x000C,
            LineWidthCommandNv = 0x000D,
            PolygonOffsetCommandNv = 0x000E,
            AlphaRefCommandNv = 0x000F,
            ViewportCommandNv = 0x0010,
            ScissorCommandNv = 0x0011,
            FrontFaceCommandNv = 0x0012,
        }

        // Texture memory layouts for INTEL_map_texture
        public enum MapTextureFormatINTEL
        {
            LayoutDefaultIntel = 0,
            LayoutLinearIntel = 1,
            LayoutLinearCpuCachedIntel = 2,
        }

        public enum PathRenderingTokenNV
        {
            ClosePathNv = 0x00,
            MoveToNv = 0x02,
            RelativeMoveToNv = 0x03,
            LineToNv = 0x04,
            RelativeLineToNv = 0x05,
            HorizontalLineToNv = 0x06,
            RelativeHorizontalLineToNv = 0x07,
            VerticalLineToNv = 0x08,
            RelativeVerticalLineToNv = 0x09,
            QuadraticCurveToNv = 0x0A,
            RelativeQuadraticCurveToNv = 0x0B,
            CubicCurveToNv = 0x0C,
            RelativeCubicCurveToNv = 0x0D,
            SmoothQuadraticCurveToNv = 0x0E,
            RelativeSmoothQuadraticCurveToNv = 0x0F,
            SmoothCubicCurveToNv = 0x10,
            RelativeSmoothCubicCurveToNv = 0x11,
            SmallCcwArcToNv = 0x12,
            RelativeSmallCcwArcToNv = 0x13,
            SmallCwArcToNv = 0x14,
            RelativeSmallCwArcToNv = 0x15,
            LargeCcwArcToNv = 0x16,
            RelativeLargeCcwArcToNv = 0x17,
            LargeCwArcToNv = 0x18,
            RelativeLargeCwArcToNv = 0x19,
            ConicCurveToNv = 0x1A,
            RelativeConicCurveToNv = 0x1B,
            SharedEdgeNv = 0xC0,
            RoundedRectNv = 0xE8,
            RelativeRoundedRectNv = 0xE9,
            RoundedRect2Nv = 0xEA,
            RelativeRoundedRect2Nv = 0xEB,
            RoundedRect4Nv = 0xEC,
            RelativeRoundedRect4Nv = 0xED,
            RoundedRect8Nv = 0xEE,
            RelativeRoundedRect8Nv = 0xEF,
            RestartPathNv = 0xF0,
            DupFirstCubicCurveToNv = 0xF2,
            DupLastCubicCurveToNv = 0xF4,
            RectNv = 0xF6,
            RelativeRectNv = 0xF7,
            CircularCcwArcToNv = 0xF8,
            CircularCwArcToNv = 0xFA,
            CircularTangentArcToNv = 0xFC,
            ArcToNv = 0xFE,
            RelativeArcToNv = 0xFF,
        }

        // For NV_transform_feedback. No clue why small negative values are used
        public enum TransformFeedbackTokenNV
        {
            NextBufferNv = -2,
            SkipComponents4Nv = -3,
            SkipComponents3Nv = -4,
            SkipComponents2Nv = -5,
            SkipComponents1Nv = -6,
        }

        public enum TriangleListSUN
        {
            RestartSun = 0x0001,
            ReplaceMiddleSun = 0x0002,
            ReplaceOldestSun = 0x0003,
        }

        public enum RegisterCombinerPname
        {
            Combine = 0x8570,
            CombineArb = 0x8570,
            CombineExt = 0x8570,
            CombineRgb = 0x8571,
            CombineRgbArb = 0x8571,
            CombineRgbExt = 0x8571,
            CombineAlpha = 0x8572,
            CombineAlphaArb = 0x8572,
            CombineAlphaExt = 0x8572,
            RgbScale = 0x8573,
            RgbScaleArb = 0x8573,
            RgbScaleExt = 0x8573,
            AddSigned = 0x8574,
            AddSignedArb = 0x8574,
            AddSignedExt = 0x8574,
            Interpolate = 0x8575,
            InterpolateArb = 0x8575,
            InterpolateExt = 0x8575,
            Constant = 0x8576,
            ConstantArb = 0x8576,
            ConstantExt = 0x8576,
            ConstantNv = 0x8576,
            PrimaryColor = 0x8577,
            PrimaryColorArb = 0x8577,
            PrimaryColorExt = 0x8577,
            Previous = 0x8578,
            PreviousArb = 0x8578,
            PreviousExt = 0x8578,
            Source0Rgb = 0x8580,
            Source0RgbArb = 0x8580,
            Source0RgbExt = 0x8580,
            Src0Rgb = 0x8580,
            Source1Rgb = 0x8581,
            Source1RgbArb = 0x8581,
            Source1RgbExt = 0x8581,
            Src1Rgb = 0x8581,
            Source2Rgb = 0x8582,
            Source2RgbArb = 0x8582,
            Source2RgbExt = 0x8582,
            Src2Rgb = 0x8582,
            Source3RgbNv = 0x8583,
            Source0Alpha = 0x8588,
            Source0AlphaArb = 0x8588,
            Source0AlphaExt = 0x8588,
            Src0Alpha = 0x8588,
            Source1Alpha = 0x8589,
            Source1AlphaArb = 0x8589,
            Source1AlphaExt = 0x8589,
            Src1Alpha = 0x8589,
            Src1AlphaExt = 0x8589,
            Source2Alpha = 0x858A,
            Source2AlphaArb = 0x858A,
            Source2AlphaExt = 0x858A,
            Src2Alpha = 0x858A,
            Source3AlphaNv = 0x858B,
            Operand0Rgb = 0x8590,
            Operand0RgbArb = 0x8590,
            Operand0RgbExt = 0x8590,
            Operand1Rgb = 0x8591,
            Operand1RgbArb = 0x8591,
            Operand1RgbExt = 0x8591,
            Operand2Rgb = 0x8592,
            Operand2RgbArb = 0x8592,
            Operand2RgbExt = 0x8592,
            Operand3RgbNv = 0x8593,
            Operand0Alpha = 0x8598,
            Operand0AlphaArb = 0x8598,
            Operand0AlphaExt = 0x8598,
            Operand1Alpha = 0x8599,
            Operand1AlphaArb = 0x8599,
            Operand1AlphaExt = 0x8599,
            Operand2Alpha = 0x859A,
            Operand2AlphaArb = 0x859A,
            Operand2AlphaExt = 0x859A,
            Operand3AlphaNv = 0x859B,
        }

        public enum ShaderType
        {
            FragmentShader = 0x8B30,
            FragmentShaderArb = 0x8B30,
            VertexShader = 0x8B31,
            VertexShaderArb = 0x8B31,
        }

        public enum ContainerType
        {
            ProgramObjectArb = 0x8B40,
            ProgramObjectExt = 0x8B40,
        }

        public enum AttributeType
        {
            FloatVec2 = 0x8B50,
            FloatVec2Arb = 0x8B50,
            FloatVec3 = 0x8B51,
            FloatVec3Arb = 0x8B51,
            FloatVec4 = 0x8B52,
            FloatVec4Arb = 0x8B52,
            IntVec2 = 0x8B53,
            IntVec2Arb = 0x8B53,
            IntVec3 = 0x8B54,
            IntVec3Arb = 0x8B54,
            IntVec4 = 0x8B55,
            IntVec4Arb = 0x8B55,
            Bool = 0x8B56,
            BoolArb = 0x8B56,
            BoolVec2 = 0x8B57,
            BoolVec2Arb = 0x8B57,
            BoolVec3 = 0x8B58,
            BoolVec3Arb = 0x8B58,
            BoolVec4 = 0x8B59,
            BoolVec4Arb = 0x8B59,
            FloatMat2 = 0x8B5A,
            FloatMat2Arb = 0x8B5A,
            FloatMat3 = 0x8B5B,
            FloatMat3Arb = 0x8B5B,
            FloatMat4 = 0x8B5C,
            FloatMat4Arb = 0x8B5C,
            Sampler1d = 0x8B5D,
            Sampler1dArb = 0x8B5D,
            Sampler2d = 0x8B5E,
            Sampler2dArb = 0x8B5E,
            Sampler3d = 0x8B5F,
            Sampler3dArb = 0x8B5F,
            Sampler3dOes = 0x8B5F,
            SamplerCube = 0x8B60,
            SamplerCubeArb = 0x8B60,
            Sampler1dShadow = 0x8B61,
            Sampler1dShadowArb = 0x8B61,
            Sampler2dShadow = 0x8B62,
            Sampler2dShadowArb = 0x8B62,
            Sampler2dShadowExt = 0x8B62,
            Sampler2dRect = 0x8B63,
            Sampler2dRectArb = 0x8B63,
            Sampler2dRectShadow = 0x8B64,
            Sampler2dRectShadowArb = 0x8B64,
            FloatMat2x3 = 0x8B65,
            FloatMat2x3Nv = 0x8B65,
            FloatMat2x4 = 0x8B66,
            FloatMat2x4Nv = 0x8B66,
            FloatMat3x2 = 0x8B67,
            FloatMat3x2Nv = 0x8B67,
            FloatMat3x4 = 0x8B68,
            FloatMat3x4Nv = 0x8B68,
            FloatMat4x2 = 0x8B69,
            FloatMat4x2Nv = 0x8B69,
            FloatMat4x3 = 0x8B6A,
            FloatMat4x3Nv = 0x8B6A,
        }
    }
}
