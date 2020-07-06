using DataBaseRepository.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

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

        public string RepositoryPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

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
            return GetTextOfFile(file.RepFullPath, LineBeg, LineEnd, LastPos);
        }
        public string GetTextOfFile(string filepath, int LineBeg, int LineEnd, int? LastPos = null)
        {
            LineBeg--;
            var lines = File.ReadLines(filepath).Skip(LineBeg).Take(LineEnd - LineBeg).ToArray();
            if (LastPos != null)
                lines[lines.Length - 1] = lines.Last().Substring(0, (int)LastPos + 1);

            var answer = string.Join("\r\n", lines);
            return answer;
        }

        public string GetWordInFile(string filepath, int Line, int ColumnBeg, int ColumnEnd)
        {
            Line--;
            return File.ReadAllLines(filepath).Skip(Line).Take(1).First().Substring(ColumnBeg, ColumnEnd - ColumnBeg + 1);
        }

        public IEnumerable<string> GetOwners()
        {
            return Directory.GetDirectories(RepositoryPath)
                .Select(x => x.Split(Path.DirectorySeparatorChar).Last().ToUpper());
        }

        public int GetEmptyLine(string filePath, int LineBeg, int LineEnd)
        {
            var answer = LineEnd + 1;
            if (LineBeg < LineEnd)
                throw new NotImplementedException();
            else if (LineBeg - LineEnd > 1)
            {
                var lines = File.ReadAllLines(filePath);
                for (int i = LineBeg; i > LineEnd; i--)
                    if (string.IsNullOrWhiteSpace((lines[i - 1])))
                    {
                        answer = i + 1;
                        break;
                    }
            }
            return answer;
        }

        public void SaveTextToFile(string text, RepositoryObject repositoryObject)
        {
            var ownerPath = Path.Combine(RepositoryPath, repositoryObject.Owner);
            if (!Directory.Exists(ownerPath))
                Directory.CreateDirectory(ownerPath);
            File.WriteAllText(repositoryObject.RepFullPath, text);
        }
    }
}
