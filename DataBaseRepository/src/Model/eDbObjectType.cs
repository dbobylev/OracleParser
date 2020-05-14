using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseRepository.Model
{
    public enum eRepositoryObjectType
    {
        None,
        Package,
        Package_Spec,
        Package_Body,
        Procedure,
        Function,
        Trigger,
        View,
        Type,
        Type_Spec,
        Type_Body,
        Text,
        Sql
    }
}
