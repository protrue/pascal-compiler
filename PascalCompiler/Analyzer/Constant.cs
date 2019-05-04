using System;
using System.Collections.Generic;
using System.Text;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public class Constant
    {
        public string Identifier { get; set; }
        public ScalarType Type { get; set; }
    }
}
