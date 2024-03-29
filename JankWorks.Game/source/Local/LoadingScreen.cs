﻿using System;

using JankWorks.Graphics;
using JankWorks.Game.Assets;

namespace JankWorks.Game.Local
{
    public abstract class LoadingScreen : IRenderable, IUpdatable, IResource
    {
        public abstract void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets);

        public abstract void InitialiseResources(AssetManager assets);


        public abstract void DisposeGraphicsResources(GraphicsDevice device);

        public abstract void DisposeResources();


        public abstract void Render(Surface surface, GameTime time);

        public abstract void Update(GameTime time);


        public abstract void UpdateLoadingProgress(ClientState state);
    }
}