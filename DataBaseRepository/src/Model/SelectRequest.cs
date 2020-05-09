using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseRepository.Model
{
    public class SelectRequest
    {
        public List<eRepositoryObjectType> FileTypes { get; set; }
        public string Owner { get; set; }
        public string Pattern { get; set; }

        public override string ToString()
        {
            return $"SelectRequest: {(string.IsNullOrEmpty(Owner) ? string.Empty : $"[Owner: {Owner}] ")}"
                + $"{(string.IsNullOrEmpty(Pattern) ? string.Empty : $"[Pattern: {Pattern}] ")}"
                + $"[{(FileTypes == null ? string.Empty : string.Join(", ", FileTypes))}] ";
        }
    }
}
