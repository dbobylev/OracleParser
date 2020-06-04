﻿using Newtonsoft.Json;
using OracleParser.Model;
using OracleParser.Model.PackageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Model
{
    [PackageElementType(ePackageElementType.Variable)]
    public class ParsedVariable : PieceOfCode
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string plType { get; private set; }
        [JsonProperty]
        public PieceOfCode NameIdentifierPart { get; set;}

        public ParsedVariable(string name, string pltype)
        {
            Name = name;
            plType = pltype;
        }

        public ParsedVariable()
        {

        }
    }
}
