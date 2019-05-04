using System.Collections.Generic;

namespace PascalCompiler.Constants
{
    public static partial class Constants
    {
        public static class Followers
        {
            public static readonly HashSet<Symbol> ProgramBlock = new HashSet<Symbol>
            {
                Symbol.Label,
                Symbol.Const,
                Symbol.Type,
                Symbol.Var,
                Symbol.Function,
                Symbol.Procedure,
                Symbol.Begin
            };

            public static readonly HashSet<Symbol> LabelsBlock = new HashSet<Symbol>
            {
                Symbol.Const,
                Symbol.Type,
                Symbol.Var,
                Symbol.Function,
                Symbol.Procedure,
                Symbol.Begin
            };

            public static readonly HashSet<Symbol> ConstantsBlock = new HashSet<Symbol>
            {
                Symbol.Type,
                Symbol.Var,
                Symbol.Function,
                Symbol.Procedure,
                Symbol.Begin
            };
            public static readonly HashSet<Symbol> ConstantDeclaration = new HashSet<Symbol>
            {
                Symbol.Identifier
            };

            public static readonly HashSet<Symbol> Constant = new HashSet<Symbol>
            {
                Symbol.Semicolon,
            };

            public static readonly HashSet<Symbol> TypesBlock = new HashSet<Symbol>
            {
                Symbol.Var,
                Symbol.Function,
                Symbol.Procedure,
                Symbol.Begin
            };

            public static readonly HashSet<Symbol> Type = new HashSet<Symbol>
            {
                Symbol.Semicolon,
                Symbol.End,
            };

            public static readonly HashSet<Symbol> SimpleType = new HashSet<Symbol>
            {
                Symbol.Comma,
                Symbol.RightSquareBracket
            };

            public static readonly HashSet<Symbol> ComplexType = new HashSet<Symbol>
            {

            };

            public static readonly HashSet<Symbol> VariablesBlock = new HashSet<Symbol>
            {
                Symbol.Function,
                Symbol.Procedure,
                Symbol.Begin
            };

            public static readonly HashSet<Symbol> SameTypeVariablesDeclaration = new HashSet<Symbol>
            {
                Symbol.Semicolon,
            };

            public static readonly HashSet<Symbol> FunctionsBlock = new HashSet<Symbol>
            {
                Symbol.Begin
            };

            public static readonly HashSet<Symbol> StatementsBlock = new HashSet<Symbol>
            {
                Symbol.Point
            };


            public static readonly HashSet<Symbol> Statement = new HashSet<Symbol>
            {
                Symbol.Semicolon,
                Symbol.End
            };

            public static readonly HashSet<Symbol> RecordVariablesList = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.Do
            };

            public static readonly HashSet<Symbol> Case = new HashSet<Symbol>
            {
                Symbol.Of,
            };

            public static readonly HashSet<Symbol> CaseElement = new HashSet<Symbol>
            {
                Symbol.End,
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

            public static readonly HashSet<Symbol> CaseConstant = new HashSet<Symbol>
            {
                Symbol.Comma,
                Symbol.Colon,
            };

            public static readonly HashSet<Symbol> AssignLeftPart = new HashSet<Symbol>
            {
                Symbol.Assign
            };

            public static readonly HashSet<Symbol> VariableExpression = new HashSet<Symbol>
            {
                Symbol.Comma,
                Symbol.RightSquareBracket
            };

            //public static readonly HashSet<Symbol> SimpleExpression = new HashSet<Symbol>
            //{
            //    Symbol.Equals,
            //    Symbol.NotEqual,
            //    Symbol.Less,
            //    Symbol.LessOrEqual,
            //    Symbol.Greater,
            //    Symbol.GreaterOrEqual,
            //};

            public static readonly HashSet<Symbol> Expression = new HashSet<Symbol>
            {
                Symbol.RightRoundBracket,
                Symbol.RightSquareBracket,
            };

            public static readonly HashSet<Symbol> SimpleExpression = PascalCompiler.Constants.Constants.RelationalOperators;

            public static readonly HashSet<Symbol> Addend = PascalCompiler.Constants.Constants.AdditiveOperators;

            public static readonly HashSet<Symbol> Multiplicand = PascalCompiler.Constants.Constants.MultiplicativeOperators;

            public static readonly HashSet<Symbol> SimpleExpressionSign = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.Nil,
                Symbol.LeftRoundBracket
            };

            public static readonly HashSet<Symbol> SimpleExpressionAddend = new HashSet<Symbol>
            {
                Symbol.Plus,
                Symbol.Minus,
                Symbol.Or
            };

            public static readonly HashSet<Symbol> AddendMultiplier = new HashSet<Symbol>
            {
                Symbol.Asterisk,
                Symbol.Slash,
                Symbol.Div,
                Symbol.Mod,
                Symbol.And,
            };

            public static readonly HashSet<Symbol> RelationalOperators = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.LeftRoundBracket,
                Symbol.Not,
            };


            public static readonly HashSet<Symbol> AdditiveOperators = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.LeftRoundBracket,
                Symbol.Not,
            };

            public static readonly HashSet<Symbol> MultiplicativeOperators = new HashSet<Symbol>
            {
                Symbol.Identifier,
                Symbol.IntegerConstant,
                Symbol.FloatConstant,
                Symbol.CharConstant,
                Symbol.StringConstant,
                Symbol.True,
                Symbol.False,
                Symbol.LeftRoundBracket,
                Symbol.Not,
            };

            public static readonly HashSet<Symbol> FactorExpression = new HashSet<Symbol>
            {
                Symbol.RightRoundBracket
            };

            public static readonly HashSet<Symbol> ConditionalStatementExpression = new HashSet<Symbol>
            {
                Symbol.Then
            };

            public static readonly HashSet<Symbol> ConditionalStatementStatement = new HashSet<Symbol>
            {
                Symbol.Else
            };

            public static readonly HashSet<Symbol> WhileStatementExpression = new HashSet<Symbol>
            {
                Symbol.Do
            };

            public static readonly HashSet<Symbol> IfStatementExpression = new HashSet<Symbol>
            {
                Symbol.Then
            };

            public static readonly HashSet<Symbol> IfStatementTrueBlock = new HashSet<Symbol>
            {
                Symbol.Else
            };

            /// <summary>
            /// Символы, ожидаемы сразу после вызова simpletype() во время анализа типа "массив"
            /// </summary>
            public static readonly HashSet<Symbol> AcodesSimpletype = new HashSet<Symbol>
            {
                Symbol.Comma,
                Symbol.RightSquareBracket,
                Symbol.EndOfLine
            };

            /// <summary>
            /// Символы ожидаемые сразу после анализа конструкции fixpart
            /// </summary>
            public static readonly HashSet<Symbol> AcodesFixpart = new HashSet<Symbol>
            {
                Symbol.Case,
                Symbol.RightRoundBracket,
                Symbol.End,
                Symbol.EndOfLine
            };

            /// <summary>
            /// Символы, ожидаемые сразу после анализа списка полей при вызове reestrfields() из casefield()
            /// </summary>
            public static readonly HashSet<Symbol> AcodesReestrfields = new HashSet<Symbol>
            {
                Symbol.RightRoundBracket,
                Symbol.End,
                Symbol.EndOfLine
            };

            /* ( а при вызове из complextype() ожи-	*/
            /* даемый символ только endsy )				*/

            /// <summary>
            /// Символы, ожидаемые сразу после анализа конструкции type при вызове функции type() из fixpart()
            /// </summary>
            public static readonly HashSet<Symbol> AcodesTyp = new HashSet<Symbol>
            {
                Symbol.End,
                Symbol.RightRoundBracket,
                Symbol.Semicolon,
                Symbol.EndOfLine
            };

            /// <summary>
            /// Символы, ожидаемые сразу после анализа константы при вызове функции constant() из casefield() и variant()	
            /// </summary>
            public static readonly HashSet<Symbol> Acodes_2Constant = new HashSet<Symbol>
            {
                Symbol.Comma,
                Symbol.Colon,
                Symbol.EndOfLine
            };

            /// <summary>
            /// Коды символов, ожидаемых сразу после анализа константы
            /// </summary>
            public static readonly HashSet<Symbol> Acodes_3Const = new HashSet<Symbol>
            {
                Symbol.TwoPoints,
                Symbol.Comma,
                Symbol.RightSquareBracket,
                Symbol.EndOfLine
            };

            /// <summary>
            /// Символы, ожидаемые сразу после списка параметров ( символы functionsy,proceduresy,beginsy уже есть в followers) 
            /// </summary>
            public static readonly HashSet<Symbol> AcodesListparam = new HashSet<Symbol>
            {
                Symbol.Colon,
                Symbol.Semicolon,
                Symbol.Forward,
                Symbol.Const,
                Symbol.Var,
                Symbol.EndOfLine
            };

            /// <summary>
            /// Символы, ожидаемые сразу после разбора фактических параметров процедур и функций
            /// </summary>
            public static readonly HashSet<Symbol> AcodesFactparam = new HashSet<Symbol>
            {
                Symbol.Comma,
                Symbol.RightRoundBracket,
                Symbol.EndOfLine
            };

            /// <summary>
            /// символ, ожидаемый сразу после переменной в операторе присваивания и в операторе for 
            /// </summary>
            public static readonly HashSet<Symbol> AcodesAssign = new HashSet<Symbol>
            {
                Symbol.Assign,
                Symbol.EndOfLine
            };

            /// <summary>
            /// символы, ожидаемые сразу после оператора в составном операторе и после варианта в операторе варианта
            /// </summary>
            public static readonly HashSet<Symbol> AcodesCompcase = new HashSet<Symbol>
            {
                Symbol.Semicolon,
                Symbol.End,
                Symbol.EndOfLine
            };

            /// <summary>
            /// символ, ожидаемый сразу после условного выражения в операторе if
            /// </summary>
            public static readonly HashSet<Symbol> AcodesIftrue = new HashSet<Symbol>
            {
                Symbol.Then,
            };

            /// <summary>
            /// символ, ожидаемый сразу после оператора ветви "истина" в операторе if
            /// </summary>
            public static readonly HashSet<Symbol> AcodesIffalse = new HashSet<Symbol>
            {
                Symbol.Else,
            };

            /// <summary>
            /// символы, ожидаемые сразу после переменной в заголовке оператора with
            /// </summary>
            public static readonly HashSet<Symbol> AcodesWiwifor = new HashSet<Symbol>
            {
                Symbol.Comma,
                Symbol.Do,
                Symbol.EndOfLine
            };

            /// <summary>
            /// символ, ожидаемый сразу после условного выражения в операторе while 
            /// и сразу после выражения-второй границы изменения параметра цикла в операторе for
            /// </summary>
            public static readonly HashSet<Symbol> AcodesWhile = new HashSet<Symbol>
            {
                Symbol.Do,
                Symbol.EndOfLine
            };

            /// <summary>
            /// cимволы, ожидаемые сразу после оператора в теле оператора repeat
            /// </summary>
            public static readonly HashSet<Symbol> AcodesRepeat = new HashSet<Symbol>
            {
                Symbol.Until,
                Symbol.Semicolon,
                Symbol.EndOfLine
            };

            /// <summary>
            /// символ, ожидаемый сразу после выбирающего выражения в операторе case
            /// </summary>
            public static readonly HashSet<Symbol> AcodesCase1 = new HashSet<Symbol>
            {
                Symbol.Of,
                Symbol.EndOfLine
            };

            /// <summary>
            /// символы, ожидаемые сразу после выражения-первой границы изменения пераметра цикла в операторе for
            /// </summary>
            public static readonly HashSet<Symbol> AcodesFor1 = new HashSet<Symbol>
            {
                Symbol.To,
                Symbol.DownTo,
                Symbol.EndOfLine
            };

            /// <summary>
            /// после идентификатора в переменной
            /// </summary>
            public static readonly HashSet<Symbol> AcodesIdent = new HashSet<Symbol>
            {
                Symbol.LeftSquareBracket,
                Symbol.Caret,
                Symbol.Point,
                Symbol.EndOfLine
            };

            /// <summary>
            /// после индексного выражения при разборе массива
            /// </summary>
            public static readonly HashSet<Symbol> AcodesIndex = new HashSet<Symbol>
            {
                Symbol.RightSquareBracket,
                Symbol.Comma,
                Symbol.EndOfLine
            };

            /// <summary>
            /// после 1-го выражения в конструкторе множества
            /// </summary>
            public static readonly HashSet<Symbol> AcodesSet1 = new HashSet<Symbol>
            {
                Symbol.RightSquareBracket,
                Symbol.TwoPoints,
                Symbol.Comma,
                Symbol.EndOfLine
            };
        }
    }
}