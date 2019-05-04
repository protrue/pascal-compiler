using System.Collections.Generic;

namespace PascalCompiler.Constants
{
    public static partial class Constants
    {
        /// <summary>
        /// Символы, которые являются префиксами других символов
        /// </summary>
        public static HashSet<string> PrefixSymbols = new HashSet<string>()
        {
            "/",    // //
            ":",    // :=
            ".",    // ..
            "(",    // (*
            "<",    // <=
            ">",    // >=
        };
    }
}
