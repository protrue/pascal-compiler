using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler
{
    public static partial class Constants
    {
        /// <summary>
        /// Символы, которые являются префиксами других символов
        /// </summary>
        public static HashSet<string> PrefixSymbols = new HashSet<string>()
        {
            "*",    // *)
            "/",    // //
            ":",    // :=
            ".",    // ..
            "(",    // (*
            "<",    // <=
            ">",    // >=
        };
    }
}
