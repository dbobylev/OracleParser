using OracleParser.Model.PackageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    class PackageElementTypeAttribute : Attribute
    {
        public ePackageElementType ElementType { get; private set; }
        public PackageElementTypeAttribute(ePackageElementType elementType)
        {
            ElementType = elementType;
        }
    }
}
