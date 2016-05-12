using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Q2.Core.Extensions;
using Q2.Core.System;
using Qa.Core.Structure.Json;

namespace Qa.Core.Structure
{
    public class SettingsProvider
    {
        private Configuration _file;

        public Settings Load()
        {
            var projectName = get("defaultProject");
            if (!File.Exists(getProjectFilePath(projectName)))
            {
                Lo.Wl(string.Format("JSON file for {0} is absent.", projectName), ConsoleColor.Red);
                projectName = null;
            }
            if (projectName.IsEmpty())
            {
                projectName = new ProjectSelector().Select(getBinFolder());
            }

            var config = new Settings
            {
                Project = projectName,
                WorkingFolder = getWorkingFolder(projectName),
                FileStructures = new JsonStructureLoader().Load(getProjectFilePath(projectName)),
                QaFileName = get(projectName + "FileName")
            };

            return config;
        }

        private string getProjectFilePath(string project)
        {
            return getBinFolder() + project + ".json";
        }

        private string getWorkingFolder(string project)
        {
            var folder = get(project + "Folder");
            if (!Path.IsPathRooted(folder))
            {
                folder = Path.GetFullPath(Path.Combine(getBinFolder(), folder));
            }
            return folder;
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
            return getSettings()[name]?.Value;
        }
    }
}