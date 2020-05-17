using DataBaseRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataBaseRepository
{
    static class Helper
    {
        public static readonly Dictionary<eRepositoryObjectType, string> FileExtensions =
            new Dictionary<eRepositoryObjectType, string>()
            {
                { eRepositoryObjectType.Function, "FNC" },
                { eRepositoryObjectType.Procedure, "PRC" },
                { eRepositoryObjectType.Package_Body, "BDY" },
                { eRepositoryObjectType.Package_Spec, "SPC" }, 
                { eRepositoryObjectType.Trigger, "TRG" },
                { eRepositoryObjectType.View, "VWS" },
                { eRepositoryObjectType.Type_Spec, "TPS" },
                { eRepositoryObjectType.Type_Body, "TPB" },
                { eRepositoryObjectType.Text, "TXT" },
                { eRepositoryObjectType.Sql, "SQL" }
            };
    }
}


