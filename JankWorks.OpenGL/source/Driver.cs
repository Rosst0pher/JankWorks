﻿using JankWorks.Core;
using JankWorks.Graphics;

using JankWorks.Drivers;
using JankWorks.Drivers.Graphics;

using JankWorks.Drivers.OpenGL.Graphics;
using JankWorks.Drivers.OpenGL.Native;

[assembly: JankWorksDriver(typeof(JankWorks.Drivers.OpenGL.Driver))]

namespace JankWorks.Drivers.OpenGL
{
    public sealed class Driver : Disposable, IGraphicsDriver
    {
        public GraphicsApi GraphicsApi => GraphicsApi.OpenGL;

        public GraphicsDevice CreateGraphicsDevice(SurfaceSettings settings, IRenderTarget renderTarget)
        {
            renderTarget.Activate();
            Functions.Init();            
            return new GLGraphicsDevice(settings, renderTarget);
        }
        
        public bool IsShaderFormatSupported(ShaderFormat format) => format == ShaderFormat.GLSL;

        protected override void Dispose(bool disposing)
        {
            Functions.loader.Dispose();
            base.Dispose(disposing);
        }
    }
}