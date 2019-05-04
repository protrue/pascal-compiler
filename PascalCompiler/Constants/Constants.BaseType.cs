using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PascalCompiler.Constants
{
    public partial class Constants
    {
        public enum BaseType
        {
            Scalar = 401,
            Limited,
            Enum,
            Array,
            Reference,
            Set,
            File,
            Record
        }
    }
}
