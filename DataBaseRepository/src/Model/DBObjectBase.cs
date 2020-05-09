using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataBaseRepository.Model
{
    public class RepositoryObject
    {
        public string Owner { get; private set; }
        public string Name { get; private set; }
        public eRepositoryObjectType DbObjectType;

        public string RepFilePath { get => Path.Combine(Owner, $"{Owner}.{Name}.{Helper.FileExtensions[DbObjectType]}"); }

        public RepositoryObject(string name, string owner, eRepositoryObjectType dbObjectType)
        {
            Name = name.ToUpper();
            Owner = owner.ToUpper();
            DbObjectType = dbObjectType;
        }

        public RepositoryObject(string filePath)
        {
            string[] splittedName = Path.GetFileName(filePath).Split('.');

            Owner = Path.GetDirectoryName(filePath).Split(Path.DirectorySeparatorChar).Last().ToUpper();
            Name = splittedName.Reverse().Skip(1).First();
            DbObjectType = Helper.FileExtensions
                .Where(x => x.Value.ToUpper() == splittedName.Last().ToUpper())
                .First()
                .Key;
        }
    }
}
