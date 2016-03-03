using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Qa.Core.Structure
{
    public class StructureLoader
    {
        public List<FileStructure> Load(string folder)
        {
            var content = File.ReadAllText(folder + "/structure.json");
            return JsonConvert.DeserializeObject<List<FileStructure>>(content);
        }
    }
}
