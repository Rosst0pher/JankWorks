﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;

using JankWorks.Core;
using JankWorks.Drivers;
using System.Collections;

namespace JankWorks.Graphics
{
    public abstract class Font : Disposable, IEnumerable<Glyph>
    {
        public virtual uint FontSize { get; set; }

        public abstract int GlyphCount { get; }

        public abstract int LineSpacing { get; }

        public abstract int MaxAdvance { get; }

        public abstract IEnumerator<Glyph> GetGlyphs();

        public abstract Glyph GetGlyph(char character);

        public abstract GlyphBitmap GetGlyphBitmap(char character);

        public static Font LoadFromStream(Stream stream, FontFormat format) => DriverConfiguration.Drivers.fontApi.LoadFontFromStream(stream, format);

        public IEnumerator<Glyph> GetEnumerator() => this.GetGlyphs();

        IEnumerator IEnumerable.GetEnumerator() => this.GetGlyphs();
    }

    public enum FontFormat
    {
        TrueType,
        OpenType
    }

    public struct Glyph
    {
        public Vector2i Size;
        public Vector2i Bearing;
        public Vector2i Advance;
        public char Value;
    }

    public readonly ref struct GlyphBitmap
    {       
        public readonly ReadOnlySpan<byte> Pixels;

        public readonly Vector2i Size;

        public readonly PixelFormat Format;

        public GlyphBitmap(ReadOnlySpan<byte> pixels, Vector2i size, PixelFormat format)
        {
            this.Pixels = pixels;
            this.Size = size;
            this.Format = format;
        }
    }        
}
