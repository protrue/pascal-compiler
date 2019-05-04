using System.Collections.Generic;

namespace PascalCompiler.Constants
{
    public static partial class Constants
    {
        public static class Starters
        {
            public static readonly HashSet<Symbol> ProgramBlock = new HashSet<Symbol>
            {
                Symbol.Program
            };

            public static readonly HashSet<Symbol> LabelsBlock = new HashSet<Symbol>
            {
                Symbol.Label
            };

            public static readonly HashSet<Symbol> ConstantsBlock = new HashSet<Symbol>
            {
                Symbol.Const
            };

            public static readonly HashSet<Symbol> ConstantDeclaration = new HashSet<Symbol>
            {
                Symbol.Identifier
            };

            public static readonly HashSet<Symbol> Constant = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Identifier,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.True,
                Symbol.False,
            };

            public static readonly HashSet<Symbol> TypesBlock = new HashSet<Symbol>
            {
                Symbol.Type
            };

            public static readonly HashSet<Symbol> TypeDeclaration = new HashSet<Symbol>
            {
                Symbol.Identifier
            };

            public static readonly HashSet<Symbol> Type = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Identifier,
                Symbol.LeftRoundBracket,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.Array,
                Symbol.Record,
                Symbol.Record,
                Symbol.Array,
                Symbol.Set,
                Symbol.File,
                Symbol.Caret,
                Symbol.Packed
            };

            public static readonly HashSet<Symbol> SimpleType = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Identifier,
                Symbol.LeftRoundBracket,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
            };


            public static readonly HashSet<Symbol> EnumerationType = new HashSet<Symbol>
            {
                Symbol.LeftRoundBracket
            };

            public static readonly HashSet<Symbol> LimitedType = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
            };

            public static readonly HashSet<Symbol> CompositeType = new HashSet<Symbol>
            {
                Symbol.Array,
                Symbol.Record,
                Symbol.Set,
                Symbol.File,
                Symbol.Packed,
                Symbol.Caret,
            };

            public static readonly HashSet<Symbol> ArrayType = new HashSet<Symbol>
            {
                Symbol.Array
            };

            public static readonly HashSet<Symbol> RecordType = new HashSet<Symbol>
            {
                Symbol.Record
            };

            public static readonly HashSet<Symbol> VariablesBlock = new HashSet<Symbol>
            {
                Symbol.Var
            };

            public static readonly HashSet<Symbol> SameTypeVariableDeclaration = new HashSet<Symbol>
            {
                Symbol.Identifier
            };

            public static readonly HashSet<Symbol> FunctionsBlock = new HashSet<Symbol>
            {
                Symbol.Function,
                Symbol.Procedure
            };

            public static readonly HashSet<Symbol> StatementsBlock = new HashSet<Symbol>
            {
                Symbol.Begin
            };

            public static readonly HashSet<Symbol> RightRoundBracket = new HashSet<Symbol>
            {
                Symbol.RightRoundBracket,
                Symbol.EndOfLine
            };

            public static readonly HashSet<Symbol> Statement = new HashSet<Symbol>
            {
                Symbol.IntegerConstant,
                Symbol.Identifier,
                Symbol.Goto,
                Symbol.Begin,
                Symbol.For,
                Symbol.While,
                Symbol.Repeat,
                Symbol.If,
                Symbol.Case,
                Symbol.With,
            };

            public static readonly HashSet<Symbol> NonLabeledStatement = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.Goto,
                Symbol.Begin,
                Symbol.For,
                Symbol.While,
                Symbol.Repeat,
                Symbol.If,
                Symbol.Case,
                Symbol.With,
            };

            public static readonly HashSet<Symbol> SimpleStatement = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.Semicolon,
            };

            public static readonly HashSet<Symbol> CompoundStatement = new HashSet<Symbol>
            {
                Symbol.Begin,
            };
            
            public static readonly HashSet<Symbol> AssignStatement = new HashSet<Symbol>
            {
                Symbol.Identifier,
            };

            public static readonly HashSet<Symbol> Variable = new HashSet<Symbol>
            {
                Symbol.Identifier
            };

            public static readonly  HashSet<Symbol> RecordVariablesList = new HashSet<Symbol>
            {
                Symbol.Identifier
            };

            public static readonly HashSet<Symbol> Expression = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.LeftRoundBracket,
                Symbol.Not,
                Symbol.Nil
            };

            public static readonly HashSet<Symbol> SimpleExpression = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.LeftRoundBracket,
                Symbol.Not,
                Symbol.Nil
            };

            public static readonly HashSet<Symbol> Sign = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Identifier
            };

            public static readonly HashSet<Symbol> Addend = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.LeftRoundBracket,
                Symbol.Nil,
                Symbol.Not
            };

            public static readonly HashSet<Symbol> Multiplicand = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.LeftRoundBracket,
                Symbol.Nil,
                Symbol.Not
            };

            public static readonly HashSet<Symbol> MultiplicativeOperation = new HashSet<Symbol>
            {
                Symbol.Asterisk,
                Symbol.Slash,
                Symbol.Div,
                Symbol.Mod,
                Symbol.And,
            };

            public static readonly HashSet<Symbol> AdditiveOperation = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Or
            };

            public static readonly HashSet<Symbol> ComplexStatement = new HashSet<Symbol>
            {
                Symbol.Begin,
                Symbol.If,
                Symbol.While,
            };

            public static readonly HashSet<Symbol> IfStatement = new HashSet<Symbol>
            {
                Symbol.If,
            };

            public static readonly HashSet<Symbol> WhileStatement = new HashSet<Symbol>
            {
                Symbol.While,
            };
        }
    }
}