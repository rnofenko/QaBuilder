using System;
using System.Configuration;
using Qa.Extensions;
using Qa.Structure;

namespace Qa.System
{
    public class SettingsProvider
    {
        private Configuration _file;
        private const string WORKING_FOLDER = "workingFolder";

        public Settings Load()
        {
            var binFolder = AppDomain.CurrentDomain.BaseDirectory;
            var config = new Settings
            {
                WorkingFolder = get(WORKING_FOLDER, binFolder),
                FileStructures = new StructureLoader().Load(binFolder)
            };

            return config;
        }

        public void Save(Settings settings)
        {
            set(WORKING_FOLDER, settings.WorkingFolder);
            _file.Save();
        }

        private Configuration getFile()
        {
            if (_file == null)
            {
                var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = "Qa.exe.config"};
                _file = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            }
            return _file;
        }

        private KeyValueConfigurationCollection getSettings()
        {
            var file = getFile();
            return file.AppSettings.Settings;
        }

        private string get(string name)
        {
            return getSettings()[name].Value;
        }

        private void set(string name, string value)
        {
            getSettings()[name].Value = value;
        }

        private string get(string name, string defaultValue)
        {
            var value = get(name);
            if (value.IsEmpty())
            {
                return defaultValue;
            }
            return value;
        }
    }
}