﻿using System.Runtime.InteropServices;
using System.Numerics;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Tests.SurfaceTest
{
    sealed class TextureSurfaceTest : Test
    {
        private VertexBuffer<Vertex2> triangle;
        private VertexLayout triangleLayout;
        private Shader triangleProgram;


        private TextureSurface surface;
        private VertexBuffer<Vertex2> surfaceQuad;        
        private VertexLayout surfaceLayout;
        private IndexBuffer surfaceIndexes;
        private Shader surfaceProgram;

        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.SetupTriangle(graphics);
            this.SetupSurface(graphics);
        }

        private void SetupTriangle(GraphicsDevice graphics)
        {
            triangle = graphics.CreateVertexBuffer<Vertex2>();
            triangle.Usage = BufferUsage.Static;

            Vertex2[] vertices =
            {
                new Vertex2(new Vector2(-0.5f, -0.5f), Colour.Green, default),
                new Vertex2(new Vector2(0.0f, 0.5f), Colour.Red, default),
                new Vertex2(new Vector2(0.5f, -0.5f), Colour.Blue, default),
            };

            triangle.Write(vertices);


            triangleLayout = graphics.CreateVertexLayout();

            var positionAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Stride = Marshal.SizeOf<Vertex2>(),
                Offset = 0,
                Index = 0,
                Usage = VertexAttributeUsage.Position
            };

            var colourAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector3f,
                Stride = Marshal.SizeOf<Vertex2>(),
                Offset = Marshal.SizeOf<Vector2>(),
                Index = 1,
                Usage = VertexAttributeUsage.Colour
            };

            triangleLayout.SetAttribute(positionAttrib);
            triangleLayout.SetAttribute(colourAttrib);

            triangleProgram = graphics.CreateShader(ShaderFormat.GLSL, this.GetEmbeddedStream("SurfaceTest.Triangle.vert.glsl"), this.GetEmbeddedStream("SurfaceTest.Triangle.frag.glsl"));
            triangleProgram.SetVertexData(triangle, triangleLayout);
        }

        private void SetupSurface(GraphicsDevice device)
        {
            var surfacesettings = new SurfaceSettings()
            {
                Size = device.Viewport.Size,
                ClearColour = Colour.Black
            };
            surface = device.CreateTextureSurface(surfacesettings);

            surfaceQuad = device.CreateVertexBuffer<Vertex2>();
            surfaceQuad.Usage = BufferUsage.Static;
            Vertex2[] quadData =
            {
                new Vertex2(new Vector2(1f,  1f), Colour.White, new Vector2(1f, 1f)),
                new Vertex2(new Vector2(1f, -1f), Colour.White, new Vector2(1f, 0f)),
                new Vertex2(new Vector2(-1f, -1f), Colour.White, new Vector2(0f, 0f)),
                new Vertex2(new Vector2(-1f,  1f), Colour.White, new Vector2(0f, 1f)),

            };
            surfaceQuad.Write(quadData);


            surfaceLayout = device.CreateVertexLayout();
            var positionAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Stride = Marshal.SizeOf<Vertex2>(),
                Offset = 0,
                Index = 0,
                Usage = VertexAttributeUsage.Position
            };

            var texAttrib = new VertexAttribute()
            {
                Format = VertexAttributeFormat.Vector2f,
                Stride = Marshal.SizeOf<Vertex2>(),
                Offset = Marshal.SizeOf<Vector2>() + Marshal.SizeOf<Vector3>(),
                Index = 1,
                Usage = VertexAttributeUsage.TextureCoordinate
            };

            surfaceLayout.SetAttribute(positionAttrib);
            surfaceLayout.SetAttribute(texAttrib);


            surfaceIndexes = device.CreateIndexBuffer();
            surfaceIndexes.Usage = BufferUsage.Static;

            uint[] indexValues =
            {
                0, 1, 3,
                1, 2, 3
            };

            surfaceIndexes.Write(indexValues);

            surfaceProgram = device.CreateShader(ShaderFormat.GLSL, this.GetEmbeddedStream("SurfaceTest.Surface.vert.glsl"), this.GetEmbeddedStream("SurfaceTest.Surface.frag.glsl"));
            surfaceProgram.SetVertexData(surfaceQuad, surfaceLayout, surfaceIndexes);
            surfaceProgram.SetUniform("rtexture", surface.Texture, 0);
        }

        public override void Draw(GraphicsDevice device)
        {
            surface.Clear();
            surface.DrawPrimitives(this.triangleProgram, DrawPrimitiveType.Triangles, 0, 3);
            surface.Display();

            device.DrawIndexedPrimitives(this.surfaceProgram, DrawPrimitiveType.Triangles, 6);
        }

        public override void Dispose(GraphicsDevice device, AudioDevice audio, Window window)
        {
            surface.Dispose();
            surfaceProgram.Dispose();
            surfaceIndexes.Dispose();
            surfaceLayout.Dispose();
            surfaceQuad.Dispose();

            triangleProgram.Dispose();
            triangleLayout.Dispose();
            triangle.Dispose();
        }
    }
}
