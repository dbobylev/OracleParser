using DataBaseRepository.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataBaseRepository
{
    public class DBRep
    {
        private static DBRep _instance;
        public static DBRep Instance()
        {
            if (_instance == null)
                _instance = new DBRep();
            return _instance;
        }

        public string RepositoryPath { get; set; }

        private DBRep()
        { 
            var configuration = new ConfigurationBuilder().AddJsonFile("DataBaseRepository.json").Build();
            Seri.InitConfig(configuration);
        }

        public IEnumerable<RepositoryObject> GetFiles(SelectRequest request)
        {
            FileFilter fileFilter = new FileFilter(request, RepositoryPath);
            return fileFilter.Search().Select(x=> new RepositoryObject(x));
        }

        public string GetTextOfFile(RepositoryObject file, int LineBeg, int LineEnd, int? LastPos = null)
        {
            return GetTextOfFile(Path.Combine(RepositoryPath, file.RepFilePath), LineBeg, LineEnd, LastPos);
        }
        public string GetTextOfFile(string filepath, int LineBeg, int LineEnd, int? LastPos = null)
        {
            LineBeg--;
            var lines = File.ReadLines(filepath).Skip(LineBeg).Take(LineEnd - LineBeg).ToArray();
            if (LastPos != null)
                lines[lines.Length - 1] = lines.Last().Substring(0, (int)LastPos + 1);

            return string.Join("\r\n", lines);
        }


        public IEnumerable<string> GetOwners()
        {
            return Directory.GetDirectories(RepositoryPath)
                .Select(x => x.Split(Path.DirectorySeparatorChar).Last().ToUpper());
        }
    }
}
