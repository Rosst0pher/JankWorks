﻿using System;
using System.Threading;

using JankWorks.Graphics;

using JankWorks.Game.Assets;

namespace JankWorks.Game.Threading
{
    sealed class RenderableSynchronizationContext : QueueSynchronizationContext, IRenderable
    {
        private readonly IRenderable renderable;
        private readonly IntervalBehavior interval;

        public RenderableSynchronizationContext(IRenderable renderable)
        {
            this.renderable = renderable;
            this.interval = renderable.RenderInterval;
        }

        public string GetName() => this.renderable.GetName();

        public void InitialiseGraphicsResources(GraphicsDevice device, AssetManager assets) => this.renderable.InitialiseGraphicsResources(device, assets);

        public void DisposeGraphicsResources(GraphicsDevice device) => this.renderable.DisposeGraphicsResources(device);

        public void Render(Surface surface, Frame frame)
        {
            try
            {
                SynchronizationContext.SetSynchronizationContext(this);


                switch (this.interval)
                {
                    case IntervalBehavior.Asynchronous:

                        if (this.Pending)
                        {
                            this.Yield();
                        }
                        else
                        {
                            this.renderable.Render(surface, frame);
                        }

                        break;

                    case IntervalBehavior.Synchronous:
                        this.renderable.Render(surface, frame);
                        this.Join();
                        break;

                    case IntervalBehavior.Overlapped:
                        this.Yield();
                        this.renderable.Render(surface, frame);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }
    }
}