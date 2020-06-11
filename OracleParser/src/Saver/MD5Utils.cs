using DataBaseRepository.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OracleParser.Saver
{
    public static class MD5Utils
    {
        public static string CalculateMD5path(string filename)
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

        public static string CalculateMD5string(string input)
        {
            using(var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hash = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string RepositoryPackageMD5(RepositoryPackage package)
        {
            var sha1 = CalculateMD5path(package.SpecRepFullPath);
            var sha2 = CalculateMD5path(package.BodyRepFullPath);
            return CalculateMD5string(sha1 + sha2);
        }
    }
}
