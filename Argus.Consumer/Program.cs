using Qa.Core.Qa;
using Qa.Core.Structure;
using Qa.Core.Excel;

namespace Qa.Argus.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var _settingsProvider = new SettingsProvider().Load();
            _settingsProvider.FileMask = "*.csv";
            new QaPrompt(_settingsProvider, new Exporter(new CommonPage())).Start();
        }
    }
}
