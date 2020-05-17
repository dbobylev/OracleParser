using DataBaseRepository.Model;
using Newtonsoft.Json;
using OracleParser.Model;
using OracleParser.Saver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace OracleParser.src.Saver
{
    class PackageManager
    {
        private const string SAVED_FOLDER = "ParsedPackages";
        private readonly string SAVED_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SAVED_FOLDER);

        private string GetSavedInstancePath(RepositoryPackage package)
        {
            return Path.Combine(SAVED_PATH, $"{package.ObjectName}.parsed");
        }

        public void SaveParsedPackage(RepositoryPackage repPackage, Package parsedPackage)
        {
            if (!Directory.Exists(SAVED_PATH))
                Directory.CreateDirectory(SAVED_PATH);

            var sha = MD5Utils.RepositoryPackageMD5(repPackage);
            var packageResult = new PackageResult(parsedPackage, sha);
            var json = JsonConvert.SerializeObject(packageResult);

            File.WriteAllText(GetSavedInstancePath(repPackage), json);
        }

        public bool CheckParsedPackage(RepositoryPackage repPackage, out Package savedParcedPackage)
        {
            savedParcedPackage = null;
            var SavedInstancePath = GetSavedInstancePath(repPackage);
            if (!File.Exists(SavedInstancePath))
                return false;

            var currentSha = MD5Utils.RepositoryPackageMD5(repPackage);
            var packageResult = JsonConvert.DeserializeObject<PackageResult>(File.ReadAllText(SavedInstancePath));
            savedParcedPackage = packageResult._package;

            return packageResult.SHA == currentSha;
        }
    }
}
