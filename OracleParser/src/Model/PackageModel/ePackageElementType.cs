using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    public enum ePackageElementType
    {
        [Description("Метод")]
        Method,
        [Description("Переменная")]
        Variable
    }
}
