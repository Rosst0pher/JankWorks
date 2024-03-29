﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Numerics;

using JankWorks.Graphics;

using static JankWorks.Drivers.OpenGL.Native.Constants;

namespace JankWorks.Drivers.OpenGL.Graphics
{
    sealed class GLSpriteRenderer : SpriteRenderer
    {  
        private readonly struct Batch
        {
            public readonly GCHandle texture;
            public readonly int offset;
            public readonly int count;

            public Batch(Texture2D texture, int offset, int count)
            {
                this.texture = GCHandle.Alloc(texture, GCHandleType.Normal);
                this.offset = offset;
                this.count = count;
            }

            public Batch(GCHandle textureHandle, int offset, int count)
            {
                this.texture = textureHandle;
                this.offset = offset;
                this.count = count;
            }
        }

        public override DrawOrder Order { get; set; }

        public override Camera Camera { get; set; }

        private const int dataSize = 128;
        private const int verticesPerSprite = 6;       

        private Vertex2[] vertices;
        private int vertexCount;

        private Batch[] batches;
        private int batchCount;

        private GLBuffer<Vertex2> vertexBuffer;

        private RendererState state;

        private GLShader program;
        private IntPtr textureParameter;
        private VertexLayout layout;

        public GLSpriteRenderer(GraphicsDevice device, Camera camera, DrawOrder order)
        {
            this.Camera = camera;
            this.Order = order;

            this.vertices = new Vertex2[dataSize];
            this.vertexCount = 0;

            this.batches = new Batch[dataSize];
            this.batchCount = 0;

            this.SetupBuffers();
            this.SetupLayout(device);
            this.SetupShaderProgram(device);

            this.program.SetVertexData(this.vertexBuffer, this.layout);
            this.textureParameter = this.program.GetUniformNameHandle("Texture");
            this.state.Setup();            
        }

        private void SetupBuffers()
        {
            this.vertexBuffer.Generate();
            this.vertexBuffer.Write(GL_ARRAY_BUFFER, BufferUsage.Dynamic, this.vertices);           
        }

        private void SetupLayout(GraphicsDevice device)
        {
            var layout = device.CreateVertexLayout();

            var attribute = new VertexAttribute();

            attribute.Index = 0;
            attribute.Offset = 0;
            attribute.Stride = Marshal.SizeOf<Vertex2>();
            attribute.Format = VertexAttributeFormat.Vector2f;            
            attribute.Usage = VertexAttributeUsage.Position;
            layout.SetAttribute(attribute);


            attribute.Index = 1;
            attribute.Offset = Marshal.SizeOf<Vector2>();
            attribute.Stride = Marshal.SizeOf<Vertex2>();
            attribute.Format = VertexAttributeFormat.Vector2f;
            attribute.Usage = VertexAttributeUsage.TextureCoordinate;
            layout.SetAttribute(attribute);

            attribute.Index = 2;
            attribute.Offset = Marshal.SizeOf<Vector2>() * 2;
            attribute.Stride = Marshal.SizeOf<Vertex2>();
            attribute.Format = VertexAttributeFormat.Vector4f;
            attribute.Usage = VertexAttributeUsage.Colour;
            layout.SetAttribute(attribute);

            this.layout = layout;
        }

        private void SetupShaderProgram(GraphicsDevice device)
        {
            var asm = typeof(GLSpriteRenderer).Assembly;
            var vertpath = $"{nameof(GLSpriteRenderer)}.vert.glsl";
            var fragpath = $"{nameof(GLSpriteRenderer)}.frag.glsl";
            this.program = (GLShader)device.CreateShader(ShaderFormat.GLSL, asm.GetManifestResourceStream(vertpath), asm.GetManifestResourceStream(fragpath));
        }

        public override void Reserve(int spriteCount)
        {
            ref var rstate = ref this.state;
            if (rstate.drawing) { throw new InvalidOperationException(); }

            var requestedVerticesCount = verticesPerSprite * spriteCount;
            var remaining = this.vertices.Length - this.vertexCount;

            if (requestedVerticesCount > remaining)
            {
                requestedVerticesCount += this.vertices.Length;
                var diff = requestedVerticesCount % dataSize;
                var newSize = (diff == 0) ? requestedVerticesCount : (dataSize - diff) + requestedVerticesCount;

                Array.Resize(ref this.vertices, newSize);
            }
        }

        public override void Clear()
        {
            unsafe
            {
                var currentBatch = 0;

                fixed (Batch* batchptr = this.batches)
                {
                    while (currentBatch < this.batchCount)
                    {
                        Batch* batch = batchptr + currentBatch;

                        batch->texture.Free();
                        *batch = default;
                        currentBatch++;
                    }
                }
            }

            this.batchCount = 0;

            // vertices is just values so we don't need to iterate or clear
            this.vertexCount = 0;
        }

        public override void BeginDraw()
        {
            this.state.BeginDraw(this.Camera, null);
            this.Clear();
        }

        public override void BeginDraw(DrawState state)
        {
            this.state.BeginDraw(this.Camera, state);
            this.Clear();
        }

        public override void Draw(Texture2D texture, Vector2 position, Vector2 size, Vector2 origin, float rotation, RGBA colour, Bounds textureBounds)
        {
            ref readonly var rstate = ref this.state;

            if(!rstate.drawing)
            {
                throw new InvalidOperationException();
            }
            else if (texture == null)
            {
                throw new NullReferenceException("texture");
            }
            else if(texture.Disposed)
            {
                throw new ObjectDisposedException("texture");
            }

            var vecColour = (Vector4)colour;
            var radians = MathF.PI / 180f * rotation;

            var model = Matrix4x4.Identity;            
            model = model * Matrix4x4.CreateScale(new Vector3(size, 0));            
            model = model * Matrix4x4.CreateTranslation(-new Vector3(size * origin, 0));
            model = model * Matrix4x4.CreateRotationZ(radians);
            model = model * Matrix4x4.CreateTranslation(new Vector3(position, 0));            
                                 
            var mvp = model * rstate.view * rstate.projection;

            var tl = new Vertex2(Vector2.Transform(new Vector2(0, 0), mvp), textureBounds.TopLeft, vecColour);
            var tr = new Vertex2(Vector2.Transform(new Vector2(1, 0), mvp), textureBounds.TopRight, vecColour);
            var bl = new Vertex2(Vector2.Transform(new Vector2(0, 1), mvp), textureBounds.BottomLeft, vecColour);
            var br = new Vertex2(Vector2.Transform(new Vector2(1, 1), mvp), textureBounds.BottomRight, vecColour);

            this.Queue(tl, tr, bl, br, texture);
        }

        private void Queue(Vertex2 tl, Vertex2 tr, Vertex2 bl, Vertex2 br, Texture2D texture)
        {
            if((this.vertices.Length - this.vertexCount) < verticesPerSprite)
            {
                Array.Resize(ref this.vertices, this.vertices.Length + dataSize);
            }

            var offset = this.vertexCount;

            /*
            quad draw order
            tl, tr, bl, bl, tr, br
            */

            unsafe
            {
                fixed(Vertex2* vertices = this.vertices.AsSpan(this.vertexCount))
                {
                    vertices[0] = tl;
                    vertices[1] = tr;
                    vertices[2] = bl;
                    vertices[3] = bl;
                    vertices[4] = tr;
                    vertices[5] = br;
                }
            }

            this.vertexCount += verticesPerSprite;

            var batchUpperBound = this.batchCount - 1;
            Batch batch = (this.batchCount == 0) ? new Batch() : this.batches[batchUpperBound];

            if(batch.texture.IsAllocated && object.ReferenceEquals(batch.texture.Target, texture))
            {
                batch = new Batch(batch.texture, batch.offset, batch.count + verticesPerSprite);                
                this.batches[batchUpperBound] = batch;
            }
            else
            {
                batch = new Batch(texture, offset, verticesPerSprite);

                if(this.batchCount == this.batches.Length)
                {
                    Array.Resize(ref this.batches, this.batches.Length + dataSize);
                }

                this.batches[batchCount++] = batch;
            }
        }

        public override void EndDraw(Surface surface)
        {
            this.state.EndDraw();

            if (this.Order == DrawOrder.Texture)
            {
                Array.Sort(this.batches, 0, this.batchCount, BatchComparer.Sorter);
                // Improvement possible, reorder vertex data to remove duplicate texture batches                
            }

            this.Flush();
            this.DrawToSurface(surface);
        }        

        public override bool ReDraw(Surface surface)
        {
            var canReDraw = this.batchCount > 0 && this.state.CanReDraw(this.Camera);

            if (canReDraw)
            {
                this.DrawToSurface(surface);
            }

            return canReDraw;
        }

        private void Flush()
        {            
            var vertexCount = this.vertexCount;

            if (this.vertexBuffer.ElementCount < vertexCount)
            {
                this.vertexBuffer.Write(GL_ARRAY_BUFFER, BufferUsage.Dynamic, this.vertices);
            }
            else
            {
                this.vertexBuffer.Update(GL_ARRAY_BUFFER, BufferUsage.Dynamic, this.vertices.AsSpan(0, vertexCount), 0);
            }         
        }
       
        private void DrawToSurface(Surface surface)
        {
            var batchCount = this.batchCount;

            if(batchCount > 0)
            {
                ref readonly var rstate = ref this.state;
                var drawState = rstate.drawState;

                int batchDirection;
                int currentBatch;
                int lastBatch;
                
                if (this.Order == DrawOrder.Reversed)
                {
                    batchDirection = -1;
                    currentBatch = batchCount - 1;
                    lastBatch = -1;
                }
                else
                {
                    batchDirection = 1;
                    currentBatch = 0;
                    lastBatch = batchCount;
                }

                Texture2D currentTexture = null;

                unsafe
                {
                    fixed (Batch* batchesPtr = this.batches)
                    {
                        Batch* batch = batchesPtr + currentBatch;

                        var batchTexture = (Texture2D)batch->texture.Target;

                        do
                        {
                            if (!object.ReferenceEquals(currentTexture, batchTexture))
                            {
                                if(batchTexture.Disposed)
                                {
                                    throw new ObjectDisposedException("texture");
                                }
                                else
                                {
                                    currentTexture = batchTexture;
                                    this.program.SetUniform(this.textureParameter, currentTexture, 0);
                                }                                
                            }

                            if (drawState != null)
                            {
                                var ds = drawState.Value;
                                surface.DrawPrimitives(this.program, DrawPrimitiveType.Triangles, batch->offset, batch->count, in ds);
                            }
                            else
                            {
                                surface.DrawPrimitives(this.program, DrawPrimitiveType.Triangles, batch->offset, batch->count);
                            }                            
                        }
                        while ((currentBatch += batchDirection) != lastBatch);
                    }
                }
            }            
        }

        protected override void Dispose(bool disposing)
        {
            this.Clear();
            this.program.Dispose();
            this.layout.Dispose();
            this.vertexBuffer.Delete();
        
            base.Dispose(disposing);
        }


        private sealed class BatchComparer : IComparer<Batch>
        {
            public int Compare(Batch left, Batch right)
            {
                var leftTexture = (GLTexture2D)left.texture.Target;
                var rightTexture = (GLTexture2D)right.texture.Target;

                return leftTexture.Id.CompareTo(rightTexture.Id);
            }

            public static readonly BatchComparer Sorter = new BatchComparer();
        }       
    }
}