using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Constants
{
    public partial class Constants
    {
        public enum IdentifierClass
        {
            Program = 300,
            Type,
            Constant,
            Variable,
            Procedure,
            Function
        }
    }
}
