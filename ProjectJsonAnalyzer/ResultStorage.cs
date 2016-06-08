﻿using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectJsonAnalyzer
{
    class ResultStorage
    {
        string _storageRoot;

        public ResultStorage(string storageRoot)
        {
            _storageRoot = storageRoot;
        }

        string GetRepoFolder(string owner, string name)
        {
            return Path.Combine(_storageRoot, owner, name);
        }

        public void StoreFile(string owner, string name, string path, string contents)
        {
            string storagePath = Path.Combine(GetRepoFolder(owner, name), path.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(storagePath));

            File.WriteAllText(storagePath, contents);
        }

        string GetResultsFilePath(string owner, string name)
        {
            return Path.Combine(GetRepoFolder(owner, name), "results.txt");
        }

        public bool HasRepoResults(string owner, string name)
        {
            return File.Exists(GetResultsFilePath(owner, name));
        }

        public void RecordRepoResults(string owner, string name, IEnumerable<SearchCode> results)
        {
            string storagePath = GetResultsFilePath(owner, name);
            Directory.CreateDirectory(Path.GetDirectoryName(storagePath));
            using (var sw = new StreamWriter(storagePath))
            {
                foreach (var result in results)
                {
                    sw.WriteLine(result.Path);
                }
            }
        }
    }
}
