﻿using System.Numerics;

using JankWorks.Audio;
using JankWorks.Graphics;
using JankWorks.Interface;

namespace Tests.TextTest
{
    class TextTest : Test
    {
        private OrthoCamera camera;
        private Font font;
        private TextRenderer txtrenderer;

        private Vector2 center;
        private Vector2 cursorpos;
        private float cursortextRotation;

        private Bounds kerningTextBounds;
        private RGBA kerningTextColour;

        public override void Setup(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            this.kerningTextColour = Colour.White;
            this.camera = new OrthoCamera(graphics.Viewport.Size);
            this.center = (Vector2)graphics.Viewport.Size / 2;
            this.font = Font.LoadFromStream(GetEmbeddedStream("TextTest.ibm-plex-mono.regular.ttf"), FontFormat.TrueType);
            this.font.FontSize = 38;
            this.txtrenderer = graphics.CreateTextRenderer(this.camera, this.font);

            var ds = new DrawState()
            {
                Blend = BlendMode.Alpha,
                DepthTest = DepthTestMode.None
            };
            graphics.DefaultDrawState = ds;
            window.OnMouseMoved += OnMouseMoved;
        }

        private void OnMouseMoved(Vector2 pos)
        {
            var translated = this.camera.TranslateScreenCoordinate(pos);
            this.cursorpos = new Vector2(translated.X, translated.Y);
            this.txtrenderer.Clear();



            if(this.kerningTextBounds.Contains(pos))
            {
                this.kerningTextColour = Colour.Red;
            }
            else
            {
                this.kerningTextColour = Colour.White;
            }
            
        }

        public override void Draw(GraphicsDevice graphics)
        {
            cursortextRotation = (cursortextRotation + 0.5f) % 360f;

            this.txtrenderer.BeginDraw();

            this.txtrenderer.Draw("Cursor -->", this.cursorpos, new Vector2(1, 0.5f), cursortextRotation, new RGBA(255, 100, 100, 100));
            this.kerningTextBounds = this.txtrenderer.Draw("Kerning!?! Whats That...", new Vector2(100, 100), Vector2.Zero, 0f, this.kerningTextColour);
            this.txtrenderer.Draw("Rainbow!", this.center, new Vector2(0.5f), 0f, static (c, i)  =>
            {
                var n = i % 6;
                return n switch
                {
                    0 => Colour.Red,
                    1 => Colour.Pink,
                    2 => Colour.Blue,
                    3 => Colour.Cyan,
                    4 => Colour.Green,
                    5 => Colour.Yellow,
                    _ => Colour.Black
                };
            });
            this.txtrenderer.EndDraw(graphics);
        }

        public override void Dispose(GraphicsDevice graphics, AudioDevice audio, Window window)
        {
            window.OnMouseMoved -= this.OnMouseMoved;
            this.txtrenderer.Dispose();            
            this.font.Dispose();
        }
    }
}