﻿using System;
using System.Text;
using System.Diagnostics;
using System.IO;

using JankWorks.Core;
using JankWorks.Drivers;
using JankWorks.Game.Assets;
using JankWorks.Game.Configuration;
using JankWorks.Game.Local;
using JankWorks.Game.Hosting;

namespace JankWorks.Game
{
    public abstract class Application : Disposable
    {
        protected readonly DirectoryInfo DataFolder;
        protected readonly DirectoryInfo SaveFolder;

        public Func<Scene>[] RegisteredScenes { get; private set; }

        public DriverConfiguration Drivers { get; private set; }

        public Settings Settings { get; private set; }

        public ApplicationConfiguration Configuration { get; private set; }

        protected Application()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), this.Name);
            this.DataFolder = new DirectoryInfo(path);
            if(!this.DataFolder.Exists)
            {
                this.DataFolder.Create();
            }

            path = Path.Combine(path, "saves");
            this.SaveFolder = new DirectoryInfo(path);

            if(!this.SaveFolder.Exists)
            {
                this.SaveFolder.Create();
            }

            this.RegisteredScenes = this.RegisterScenes();

            this.Drivers = this.RegisterDrivers();

            this.Settings = this.GetApplicationSettings();

            var conf = this.DefaultApplicationConfiguration;
            conf.Load(this.Settings);
            this.Configuration = conf;
        }

        public abstract string Name { get; }

        protected abstract DriverConfiguration RegisterDrivers();

        protected abstract Func<Scene>[] RegisterScenes();

        public abstract AssetManager RegisterAssetManager();

        public virtual LoadingScreen RegisterLoadingScreen() => null;


        public virtual ApplicationParameters ApplicationParameters => ApplicationParameters.Default;

        public virtual HostParameters HostParameters => HostParameters.Default;

        public virtual ClientParameters ClientParameters => ClientParameters.Default;


        public virtual ApplicationConfiguration DefaultApplicationConfiguration => ApplicationConfiguration.Default;

        public virtual ClientConfgiuration DefaultClientConfiguration => ClientConfgiuration.Default;


        public Settings GetApplicationSettings()
        {
            if (this.ApplicationParameters.AppSettingsMode == ApplicationParameters.SettingsMode.NoSettings)
            {
                return new Settings(SettingsSource.Transient);
            }
            else
            {
                return this.GetPersistentApplicationSettings();
            }
        }

        public Settings GetHostSettings()
        {
            if (this.ApplicationParameters.HostSettingsMode == ApplicationParameters.SettingsMode.NoSettings)
            {
                return new Settings(SettingsSource.Transient);
            }
            else
            {
                return this.GetPersistentHostSettings();
            }
        }

        public Settings GetClientSettings()
        {
            if (this.ApplicationParameters.ClientSettingsMode == ApplicationParameters.SettingsMode.NoSettings)
            {
                return new Settings(SettingsSource.Transient);                
            }
            else
            {
                return this.GetPersistentClientSettings();
            }
        }

        protected virtual Settings GetPersistentApplicationSettings()
        {
            var path = Path.Combine(this.DataFolder.FullName, "app.ini");
            var settings = new Settings(new IniSettingsSource(path, Encoding.UTF8));
            settings.Load();
            return settings;
        }

        protected virtual Settings GetPersistentHostSettings()
        {
            var path = Path.Combine(this.DataFolder.FullName, "host.ini");
            var settings = new Settings(new IniSettingsSource(path, Encoding.UTF8));
            settings.Load();
            return settings;
        }

        protected virtual Settings GetPersistentClientSettings()
        {
            var path = Path.Combine(this.DataFolder.FullName, "client.ini");
            var settings = new Settings(new IniSettingsSource(path, Encoding.UTF8));
            settings.Load();
            return settings;
        }

        public static void Run<App>(int scene, object state = null) where App : Application, new()
        {
            using var application = new App();
            Run(application, scene, state);
        }

        public static void RunWithoutHost<App>(int scene, object state = null) where App : Application, new()
        {
            using var application = new App();
            RunWithoutHost(application, scene, state);
        }

        public static void RunWithoutHost(Application application, int scene, object state = null)
        {
            var host = new NullHost(application);
            using var client = new Client(application, host);

            client.Run(scene, state);
        }

        public static void Run(Application application, int scene, object state = null)
        {
            var host = new OfflineHost(application);
            using var client = new Client(application, host);
            
            host.Start(client);
            client.Run(scene, state);
        }

        public static void Run(Application application, Host host, int scene, object state = null)
        {
            using var client = new Client(application, host);           
            client.Run(scene, state);
        }
    }

    public struct ApplicationParameters
    {
        public SettingsMode AppSettingsMode { get; set; }

        public SettingsMode ClientSettingsMode { get; set; }

        public SettingsMode HostSettingsMode { get; set; }

        public static ApplicationParameters Default => new ApplicationParameters()
        {
            AppSettingsMode = SettingsMode.Persisted,
            ClientSettingsMode = SettingsMode.Persisted,
            HostSettingsMode = SettingsMode.Persisted
        };
        
        [Flags]
        public enum SettingsMode
        {
            NoSettings = 0,

            Persisted = 1
        }
    }

    public struct ApplicationConfiguration
    {
        private const string DebugSection = "Debug";

        private const string PerformanceMetricsEnabledEntry = "PerformanceMetrics";

        public bool PerformanceMetricsEnabled { get; set; }

        public void Load(Settings settings)
        {
            this.PerformanceMetricsEnabled = settings.GetEntry(PerformanceMetricsEnabledEntry, (s) => bool.Parse(s), DebugSection, this.PerformanceMetricsEnabled);
        }

        public void Save(Settings settings)
        {
            settings.SetEntry(PerformanceMetricsEnabledEntry, this.PerformanceMetricsEnabled.ToString(), DebugSection);
        }

        public static ApplicationConfiguration Default
        {
            get
            {
                ApplicationConfiguration ac = default;
                ac.PerformanceMetricsEnabled = Debugger.IsAttached;
                return ac;
            }
        }
    }
}