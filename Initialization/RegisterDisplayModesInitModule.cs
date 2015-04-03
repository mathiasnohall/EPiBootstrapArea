﻿using System.Diagnostics;
using System.Linq;
using EPiServer.Data;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace EPiBootstrapArea.Initialization
{
    [ModuleDependency(typeof(DataInitialization))]
    [ModuleDependency(typeof(DisplayModeFallbackProviderInitModule))]
    public class RegisterDisplayModesInitModule : IInitializableModule
    {
        private IDisplayModeFallbackProvider _provider;

        public void Initialize(InitializationEngine context)
        {
            _provider = ServiceLocator.Current.GetInstance<IDisplayModeFallbackProvider>();

            if (_provider != null)
            {
                _provider.Initialize();
            }

            RegisterDisplayOptions();
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }

        private void RegisterDisplayOptions()
        {
            var options = ServiceLocator.Current.GetInstance<DisplayOptions>();
            var localizationService = ServiceLocator.Current.GetInstance<LocalizationService>();
            var modes = _provider.GetAll();

            Debug.WriteLine("Number " + modes.Count());

            foreach (var mode in modes)
            {
                var name = "/displayoptions/" + mode.Tag;
                string translatedName;
                translatedName = !localizationService.TryGetString(name, out translatedName) ? mode.Name : name;

                options.Add(new DisplayOption
                {
                    Id = mode.Tag,
                    Name = translatedName,
                    Tag = mode.Tag,
                    IconClass = mode.Icon
                });
            }
        }
    }
}
