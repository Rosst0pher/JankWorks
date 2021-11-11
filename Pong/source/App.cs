﻿using System;

using JankWorks.Interface;
using JankWorks.Drivers;
using JankWorks.Game;
using JankWorks.Game.Assets;
using JankWorks.Game.Local;

using Pong.MainMenu;
using Pong.Match;
using JankWorks.Game.Hosting;

namespace Pong
{
    class App : Application
    {
        public override string Name => "Pong";

        public override AssetManager RegisterAssetManager()
        {
            var assets = new AssetManager();
            assets.RegisterBundle(new EmbeddedBundle(typeof(App).Assembly), "embedded");
            return assets;
        }

        protected override DriverConfiguration RegisterDrivers()
        {
            return DriverConfiguration.Initialise
            (
                "JankWorks.Glfw",
                "JankWorks.DotNet",
                "JankWorks.FreeType",
                "JankWorks.OpenGL",
                "JankWorks.OpenAL"
            );
        }

        protected override Func<Scene>[] RegisterScenes()
        {
            return new Func<Scene>[]
            {
                () => new MenuScene(),
                () => new MatchScene()
            };
        }

        public override ClientParameters ClientParameters
        {
            get
            {
                var p = base.ClientParameters;
                p.UpdateRate = 60;
                return p;
            }
        }

        public override HostParameters HostParameters
        {
            get
            {
                var p = base.HostParameters;
                p.TickRate = 60;
                return p;
            }
        }

        public override ApplicationConfiguration DefaultApplicationConfiguration
        {
            get
            {
                var conf = base.DefaultApplicationConfiguration;
                conf.PerformanceMetricsEnabled = true;
                return conf;
            }
        }

        public override ClientConfgiuration DefaultClientConfiguration
        {
            get
            {
                var conf = base.DefaultClientConfiguration;
                conf.DisplayMode = new DisplayMode(1024, 768, 32, conf.DisplayMode.RefreshRate);
                conf.WindowStyle = WindowStyle.Windowed;
                //conf.Vsync = true;
                conf.FrameRate = 60;
                return conf;                     
            }
        }
        

        static void Main(string[] args) => Application.RunWithoutHost<App>(0);               
    }
}