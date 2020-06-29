using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OracleParser.Model.PackageModel
{
    [Flags]
    public enum ePackageElementType
    {
        [Description("None")]
        None                            = 0,
        // [Description("Метод")]
        // Method                          = 1 << 0,
        [Description("Переменная")]
        Variable                        = 1 << 1,
        [Description("Тип")]
        Type                            = 1 << 2,
        [Description("Курсор")]
        Cursor                          = 1 << 3,
        [Description("Процедура")]
        Procedure                       = 1 << 4,
        [Description("Функция")]
        Function                        = 1 << 5,

    }
}
