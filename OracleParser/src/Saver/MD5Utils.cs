using DataBaseRepository.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OracleParser.src.Saver
{
    public static class MD5Utils
    {
        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static string RepositoryPackageMD5(RepositoryPackage package)
        {
            var sha = CalculateMD5(package.SpecRepFilePath);
            sha += CalculateMD5(package.BodyRepFilePath);
            return sha;
        }
    }
}
