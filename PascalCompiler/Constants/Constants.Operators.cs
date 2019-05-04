using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler.Constants
{
    public partial class Constants
    {
        public static HashSet<Symbol> RelationalOperators = new HashSet<Symbol>
        {
            Symbol.Equals,
            Symbol.Greater,
            Symbol.GreaterOrEqual,
            Symbol.NotEqual,
            Symbol.Less,
            Symbol.LessOrEqual,
            Symbol.In,
        };

        public static HashSet<Symbol> MultiplicativeOperators = new HashSet<Symbol>
        {
            Symbol.Asterisk,
            Symbol.Slash,
            Symbol.And,
            Symbol.Div,
            Symbol.Mod,
        };

        public static HashSet<Symbol> AdditiveOperators = new HashSet<Symbol>
        {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Or
        };
    }
}
