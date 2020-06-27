using DataBaseRepository.Model;
using OracleParser.Model.PackageModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OracleParser
{
    public interface IOraParser
    {
        event Action<eRepositoryObjectType> ObjectWasParsed;
        Task<Package> GetPackage(RepositoryPackage repositoryPackage, bool allowNationalChars);
        Package GetSavedPackage(RepositoryPackage repositoryPackage);
        Task<Package> ParsePackage(RepositoryPackage repositoryPackage, bool allowNationalChars);
    }
}
