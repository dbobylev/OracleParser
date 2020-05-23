using DataBaseRepository.Model;
using Newtonsoft.Json;
using OracleParser.Model;
using OracleParser.Model.PackageModel;
using OracleParser.Saver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OracleParser.src.Saver
{
    class PackageManager
    {
        private const string SAVED_FOLDER = "ParsedPackages";
        private readonly string SAVED_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SAVED_FOLDER);

        public void SaveParsedPackage(RepositoryPackage repPackage, Package package)
        {
            if (!Directory.Exists(SAVED_PATH))
                Directory.CreateDirectory(SAVED_PATH);

            var sha = MD5Utils.RepositoryPackageMD5(repPackage);
            var packageResult = new PackageResult(package, repPackage.ObjectName, sha);
            var json = JsonConvert.SerializeObject(packageResult);

            File.WriteAllText(GetSavedInstancePath(repPackage), json);
        }

        public bool CheckPackage(RepositoryPackage repPackage, out Package package)
        {
            Seri.Log.Verbose($"CheckParsedPackage begin repPackage={repPackage}");
            bool answer;
            package = null;

            var SavedInstancePath = GetSavedInstancePath(repPackage);
            Seri.Log.Verbose($"SavedInstancePath={SavedInstancePath}");
            if (!File.Exists(SavedInstancePath))
                answer = false;
            else
            {
                var currentSha = MD5Utils.RepositoryPackageMD5(repPackage);
                string fileText = File.ReadAllText(SavedInstancePath);
                var packageResult = JsonConvert.DeserializeObject<PackageResult>(fileText);
                package = packageResult._package;
                answer = packageResult.SHA == currentSha;
            }
            Seri.Log.Verbose($"CheckParsedPackage answer:{answer}");
            return answer;
        }

        private string GetSavedInstancePath(RepositoryPackage package)
        {
            return Path.Combine(SAVED_PATH, $"{package.ObjectName}.parsed");
        }
    }
}
