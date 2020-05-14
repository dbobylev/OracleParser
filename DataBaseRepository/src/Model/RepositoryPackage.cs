using DataBaseRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DataBaseRepository.Model
{
    public class RepositoryPackage :RepositoryObject
    {
        public RepositoryPackage(string name, string owner) :base(name, owner, eRepositoryObjectType.Package)
        {

        }

        public RepositoryPackage(RepositoryObject obj) : base(obj.Name, obj.Owner, eRepositoryObjectType.Package)
        {

        }

        public string BodyRepFilePath
        {
            get => Path.Combine(Owner, $"{Owner}.{Name}.{Helper.FileExtensions[eRepositoryObjectType.Package_Body]}");
        }

        public string SpecRepFilePath
        {
            get => Path.Combine(Owner, $"{Owner}.{Name}.{Helper.FileExtensions[eRepositoryObjectType.Package_Spec]}");
        }
    }
}
