using System;
using System.Collections.Generic;
using System.Text;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public class Entity
    {
        public string Identifier { get; set; }
        public IdentifierClass IdentifierClass { get; set; }
    }
}
