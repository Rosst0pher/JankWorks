﻿using System;
using System.Diagnostics;

using System.Threading;
using System.Threading.Tasks;

using JankWorks.Game.Local;
using JankWorks.Game.Diagnostics;
using JankWorks.Game.Platform;
using JankWorks.Game.Threading;

using JankWorks.Game.Hosting.Messaging;
using JankWorks.Game.Hosting.Messaging.Memory;

namespace JankWorks.Game.Hosting
{
    public sealed class OfflineHost : ClientHost, IRunner<object, object>
    {
        private HostScene scene;
        private Client client;

        private ulong tick;

        private volatile HostState state;

        private NewHostSceneRequest newHostSceneRequest;

        private HostParameters parameters;

        private Counter tickCounter;
        private HostMetrics metrics;
        private Dispatcher dispatcher;
        private Thread thread;

        private Stopwatch timer;
        private TimeSpan tickTime;

        public override bool IsRemote => false;

        public override bool IsConnected => true;

        public override HostState State => this.state;

        public override bool IsHostLoaded => this.state == HostState.RunningScene;

        public override Dispatcher Dispatcher => this.dispatcher;

        public override HostMetrics Metrics => this.metrics;

        Stopwatch IRunner<object, object>.Timer => this.timer;

        TimeSpan IRunner<object, object>.TotalElapsed { get; set; }
        TimeSpan IRunner<object, object>.Accumulated { get; set; }
        TimeSpan IRunner<object, object>.TargetElapsed => this.tickTime;

        long IRunner<object, object>.LastRunTick { get; set; }

        public OfflineHost(Application application) : base(application, application.GetClientSettings())
        {
            var parms = application.HostParameters;
            this.parameters = parms;
            this.tickTime = TimeSpan.FromMilliseconds((1f / this.parameters.TickRate) * 1000);

            this.tickCounter = new Counter(TimeSpan.FromSeconds(1));
            this.metrics = new HostMetrics();
            this.state = HostState.Constructed;

            this.dispatcher = new MemoryDispatcher(application);
            this.thread = new Thread(new ThreadStart(this.Run));
            this.timer = new Stopwatch();
        }
      
        public override Task RunAsync(Client client)
        {
            this.client = client;
            var task = new Task(() => this.thread.Join());
            this.thread.Start();
            return task;
        }

        public override Task RunAsync(Client client, int scene, object initState = null)
        {
            this.client = client;
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                SceneName = scene,
                InitState = initState
            };
            this.state = HostState.LoadingScene;
            Thread.MemoryBarrier();
            var task = new Task(() => this.thread.Join());
            this.thread.Start();
            return task;
        }

        public override void Start(Client client)
        {
            this.client = client;
            this.thread.Start();
        }

        public override void Start(Client client, int scene, object initState = null)
        {
            this.thread = new Thread(new ThreadStart(() => this.Run(client, scene, initState)));
            this.thread.Start();
        }

        public override void Run(Client client, int scene, object initState = null)
        {
            this.client = client;
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                SceneName = scene,
                InitState = initState
            };
            this.state = HostState.LoadingScene;

            this.Run();
        }


        private void Run()
        {
            var hostThread = Thread.CurrentThread;
            hostThread.Name = $"{this.Application.Name} Host Thread";
            Threads.HostThread = hostThread;
            this.tickCounter.Start();

            var runner = this as IRunner<object, object>;

            while (true)
            {
                var state = this.state;

                switch (state)
                {
                    case HostState.RunningScene:
                        var tickDuration = runner.Run(null, null);
                        this.Metrics.TickLag = 1d * (tickDuration.TotalMilliseconds / this.tickTime.TotalMilliseconds);
                        continue;

                    case HostState.LoadingScene:
                        runner.StopRun();
                        this.LoadScene();
                        continue;

                    case HostState.UnloadingScene:
                        runner.StopRun();

                        using (var sync = new ScopedSynchronizationContext(true))
                        {
                            this.scene.HostDispose(this);
                            sync.Join();

                            this.scene.SharedDispose(this, this.client);
                            sync.Join();
                        }

                        this.scene.SharedDisposed();

                        this.dispatcher.ClearChannels();
                        this.scene = null;
                        this.state = HostState.Constructed;
                        continue;

                    case HostState.WaitingOnClients:

                        if (this.client.State == ClientState.WaitingOnHost)
                        {
                            this.tick = 0;
                            this.state = HostState.RunningScene;
                            runner.BeginRun();
                        }
                        else
                        {
                            Thread.Yield();
                        }
                        continue;

                    case HostState.Constructed:
                        Thread.Yield();
                        continue;

                    case HostState.BeginShutdown:
                        runner.StopRun();
                        this.Dispose();
                        return;
                    case HostState.Shutdown:
                        return;
                }
            }
        }


        void IRunner<object, object>.Simulate(GameTime time, object state)
        {
            this.metrics.TicksPerSecond = this.tickCounter.Frequency;
            lock (this) { this.scene.Tick(this.tick++, time); }
            this.tickCounter.Count();            
        }

        void IRunner<object, object>.Interpolate(GameTime time, object state) { }        

        public override void SynchroniseClientUpdate() 
        {
            lock (this)
            {
                this.dispatcher.Synchronise();
            }            
        }

        public override void UnloadScene()
        {
            if(this.state == HostState.RunningScene)
            {
                this.state = HostState.UnloadingScene;
            }
        }

        private void LoadScene()
        {
            this.scene = this.newHostSceneRequest.Scene;
            
            using(var sync = new ScopedSynchronizationContext(true))
            {
                this.scene.HostInitialise(this);
                sync.Join();

                this.scene.SharedInitialise(this, this.client);
                sync.Join();

                this.scene.InternalHostInitialise();
                sync.Join();
            }

            var initState = this.newHostSceneRequest.InitState;

            this.scene.HostInitialised(initState);

            this.scene.SharedInitialised(initState);

            this.metrics.TickMetricCounters = this.scene.TickMetricCounters;
            this.metrics.ParallelTickMetricCounters = this.scene.ParallelTickMetricCounters;

            this.newHostSceneRequest = default;            
            this.state = HostState.WaitingOnClients;
        }

        public override void LoadScene(HostScene scene, object initState = null)
        {
            this.newHostSceneRequest = new NewHostSceneRequest()
            {
                Scene = scene,
                InitState = initState
            };
            Thread.MemoryBarrier();
            this.state = HostState.LoadingScene;
        }

        protected override void Dispose(bool disposing)
        {
            if(Thread.CurrentThread.ManagedThreadId == this.thread.ManagedThreadId)
            {
                this.dispatcher.Dispose();
                Threads.HostThread = null;
                this.state = HostState.Shutdown;
            }
            else
            {
                this.DisposeAsync().Wait();
            }
            
            base.Dispose(disposing);
        }

        public override Task DisposeAsync()
        {            
            if (Thread.CurrentThread.ManagedThreadId == this.thread.ManagedThreadId)
            {
                // returning a async task of yourself doing something is pretty deep
                throw new InvalidOperationException();
            }

            return Task.Run(() =>
            {
                var currentState = this.state;

                // If the host thread is currently running we need to signal it to unload first
                if (currentState == HostState.RunningScene || currentState == HostState.WaitingOnClients)
                {
                    this.UnloadScene();
                }

                // host thread will reset state to constructed once its finished unloading
                while (this.state != HostState.Constructed)
                {
                    Thread.Yield();
                }

                // only signal to shutdown once host thread is finished unloading
                this.state = HostState.BeginShutdown;

                // important this join only happens once the host thread is actually shutting down
                this.thread.Join();
            });                 
        }
    }

    public enum HostState
    {
        Constructed = 0,

        BeginShutdown,

        Shutdown,

        RunningScene,

        LoadingScene,

        UnloadingScene,

        WaitingOnClients
    }
}