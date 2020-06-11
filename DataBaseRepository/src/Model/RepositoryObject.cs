using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DataBaseRepository.Model
{
    public class RepositoryObject
    {
        [JsonProperty]
        public string Owner { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public eRepositoryObjectType DbObjectType;

        [JsonIgnore]
        public string RepFilePath 
        {
            get
            {
                if (!Helper.FileExtensions.Keys.Contains(DbObjectType))
                    throw new NotImplementedException("Wrong dbObjectFileType");
                return Path.Combine(Owner, $"{Owner}.{Name}.{Helper.FileExtensions[DbObjectType]}");
            }
        }

        [JsonIgnore]
        public string ObjectName
        {
            get => $"{Owner}.{Name}";
        }

        public RepositoryObject(string name, string owner, eRepositoryObjectType dbObjectType)
        {
            Name = name.ToUpper();
            Owner = owner.ToUpper();
            DbObjectType = dbObjectType;
        }

        public RepositoryObject()
        {

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

        public override string ToString()
        {
            return ObjectName;
        }
    }
}
