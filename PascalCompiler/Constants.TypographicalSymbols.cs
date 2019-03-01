using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler
{
    public static partial class Constants
    {
        public static HashSet<string> TypographicalSymbols = new HashSet<string>()
        {
            "*",
            "/",
            "=",
            ",",
            ";",
            ":",
            ".",
            "^",
            "'",
            "(",
            ")",
            "[",
            "]",
            "{",
            "}",
            "<",
            ">",
            "<=",
            ">=",
            "<>",
            "+",
            "-",
            "//",
            "(*",
            "*)",
            ":=",
            "..",
        };
    }
}
