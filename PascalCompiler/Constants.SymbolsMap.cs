﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler
{
    public static partial class Constants
    {
        public static readonly Dictionary<string, Symbol> StringSymbolMap = new Dictionary<string, Symbol>()
        {
            { "do", Symbol.Do },
            { "if", Symbol.If },
            { "in", Symbol.In },
            { "of", Symbol.Of },
            { "or", Symbol.Or },
            { "to", Symbol.To },
            { "and", Symbol.And },
            { "div", Symbol.Div },
            { "end", Symbol.End },
            { "for", Symbol.For },
            { "mod", Symbol.Mod },
            { "nil", Symbol.Nil },
            { "not", Symbol.Not },
            { "set", Symbol.Set },
            { "var", Symbol.Var },
            { "case", Symbol.Case },
            { "else", Symbol.Else },
            { "file", Symbol.File },
            { "goto", Symbol.Goto },
            { "only", Symbol.Only },
            { "then", Symbol.Then },
            { "type", Symbol.Type },
            { "unit", Symbol.Unit },
            { "uses", Symbol.Uses },
            { "with", Symbol.With },
            { "array", Symbol.Array },
            { "begin", Symbol.Begin },
            { "const", Symbol.Const },
            { "label", Symbol.Label },
            { "until", Symbol.Until },
            { "while", Symbol.While },
            { "export", Symbol.Export },
            { "import", Symbol.Import },
            { "downto", Symbol.DownTo },
            { "module", Symbol.Module },
            { "packed", Symbol.Packed },
            { "record", Symbol.Record },
            { "repeat", Symbol.Repeat },
            { "vector", Symbol.Vector },
            { "string", Symbol.String },
            { "forward", Symbol.Forward },
            { "process", Symbol.Process },
            { "program", Symbol.Program },
            { "segment", Symbol.Segment },
            { "function", Symbol.Function },
            { "separate", Symbol.Separate },
            { "interface", Symbol.Interface },
            { "procedure", Symbol.Procedure },
            { "qualified", Symbol.Qualified },
            { "implementation", Symbol.Implementation },
        };
    }
}
