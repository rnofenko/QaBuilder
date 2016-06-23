﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Qa.Core.System
{
    public class PathFinder
    {
        private readonly List<string> _exceptions = new List<string> {".exe", ".config", ".json", ".dll", ".xml", ".pdb", ".xlsx" };

        public List<string> Find(string folder, string mask = "*.*")
        {
            return Directory.GetFiles(folder, mask)
                .Where(x => !_exceptions.Contains(Path.GetExtension(x)))
                .OrderBy(x => x)
                .ToList();
        }
    }
}