﻿using System;
using System.IO;

using JankWorks.Core;
using JankWorks.Drivers;


namespace JankWorks.Graphics
{
    public abstract class Image : Disposable
    {
        public abstract Vector2i Size { get; }

        public abstract void CopyTo(Texture2D texture);

        public abstract void CopyTo(Texture2D texture, Vector2i position);

        public abstract void Read(Texture2D texture);

        public abstract void Save(Stream stream, ImageFormat format);

        public static Image Load(Stream stream, ImageFormat format) => DriverConfiguration.Drivers.imageApi.LoadFromStream(stream, format);

        public static Image Create(Vector2i size, ImageFormat format) => DriverConfiguration.Drivers.imageApi.Create(size, format);
    }


    public enum ImageFormat
    {
        BMP,
        JPG,
        PNG
    }
}
