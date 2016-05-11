using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Q2.Core.Structure.Json;

namespace Q2.Core.Structure
{
    public class SettingsProvider
    {
        private Configuration _file;
        private const string WORKING_FOLDER = "workingFolder";

        public Settings Load()
        {
            var config = new Settings
            {
                WorkingFolder = getWorkingFolder(),
                FileStructures = new JsonStructureLoader().Load(getBinFolder()),
                QaFileName = get("qaFileName")
            };

            return config;
        }

        private string getWorkingFolder()
        {
            var folder = get(WORKING_FOLDER);
            if (!Path.IsPathRooted(folder))
            {
                folder = Path.GetFullPath(Path.Combine(getBinFolder(), folder));
            }
            return folder;
        }

        public void Save(Settings settings)
        {
            set(WORKING_FOLDER, settings.WorkingFolder);
            _file.Save();
        }

        private string getBinFolder()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private Configuration getFile()
        {
            if (_file == null)
            {
                var filePath = Process.GetCurrentProcess().MainModule.FileName + ".config";
                filePath = Path.Combine(getBinFolder(), filePath);                
                if (!File.Exists(filePath))
                {
                    throw new InvalidOperationException("Configuration file wasn't found.");
                }
                var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = filePath };
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
            var parameter = getSettings()[name];
            if (parameter != null)
            {
                return parameter.Value;
            }
            return null;
        }

        private void set(string name, string value)
        {
            getSettings()[name].Value = value;
        }
    }
}