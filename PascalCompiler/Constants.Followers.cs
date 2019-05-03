using static PascalCompiler.Constants;

public static partial class Constants
{
    public static class Followers
    {
        /// <summary>
        /// Символы следующие за конструкцией блока в основной программе
        /// </summary>
        public static readonly Symbol[] Block = new[]
        {
            Symbol.Point
        };

        public static readonly Symbol[] ConstPart = new[]
        {
            Symbol.Type,
            Symbol.Var,
            Symbol.Begin
        };

        public static readonly Symbol[] ConstDeclaration = new[]
        {
            Symbol.Semicolon
        };

        public static readonly Symbol[] TypePart = new[]
        {
            Symbol.Var,
            Symbol.Begin
        };

        public static readonly Symbol[] TypeDeclaration = new[]
        {
            Symbol.Semicolon
        };

        public static readonly Symbol[] VarPart = new[]
        {
            Symbol.Begin
        };

        public static readonly Symbol[] LimitedTypeFirstConst = new[]
        {
            Symbol.TwoPoints
        };

        public static readonly Symbol[] SimpleType = new[]
        {
            Symbol.Comma,
            Symbol.RightSquareBracket
        };

        public static readonly Symbol[] VarDeclaration = new[]
        {
            Symbol.Semicolon
        };

        public static readonly Symbol[] Statement = new[]
        {
            Symbol.Semicolon,
            Symbol.End
        };

        public static readonly Symbol[] AssignmentStatementVariable = new[]
        {
            Symbol.Assign
        };

        public static readonly Symbol[] VariableExpression = new[]
        {
            Symbol.Comma,
            Symbol.RightSquareBracket
        };

        public static readonly Symbol[] ExpressionSimpleExpression = new[]
        {
            Symbol.Equals,
            Symbol.NotEqual,
            Symbol.Less,
            Symbol.LessOrEqual,
            Symbol.Greater,
            Symbol.GreaterOrEqual,
        };

        public static readonly Symbol[] SimpleExpressionSign = new[]
        {
            Symbol.Identifier,
            Symbol.IntegerConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant,
            Symbol.Nil,
            Symbol.LeftRoundBracket
        };

        public static readonly Symbol[] SimpleExpressionAddend = new[]
        {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Or
        };

        public static readonly Symbol[] AddendMultiplier = new[]
        {
            Symbol.Asterisk,
            Symbol.Slash,
            Symbol.Div,
            Symbol.Mod,
            Symbol.And,
        };

        public static readonly Symbol[] FactorExpression = new[]
        {
            Symbol.RightRoundBracket
        };

        public static readonly Symbol[] ConditionalStatementExpression = new[]
        {
            Symbol.Then
        };

        public static readonly Symbol[] ConditionalStatementStatement = new[]
        {
            Symbol.Else
        };

        public static readonly Symbol[] WhileStatementExpression = new[]
        {
            Symbol.Do
        };

        /// <summary>
        /// Символы, ожидаемы сразу после вызова simpletype() во время анализа типа "массив"
        /// </summary>
        public static readonly Symbol[] acodes_simpletype = new[]
        {
            Symbol.Comma,
            Symbol.RightSquareBracket,
            Symbol.EndOfLine
        };

        /// <summary>
        /// Символы ожидаемые сразу после анализа конструкции fixpart
        /// </summary>
        public static readonly Symbol[] acodes_fixpart = new[]
        {
            Symbol.Case,
            Symbol.RightRoundBracket,
            Symbol.End,
            Symbol.EndOfLine
        };

        /// <summary>
        /// Символы, ожидаемые сразу после анализа списка полей при вызове reestrfields() из casefield()
        /// </summary>
        public static readonly Symbol[] acodes_reestrfields = new[]
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
        public static readonly Symbol[] acodes_typ = new[]
        {
            Symbol.End,
            Symbol.RightRoundBracket,
            Symbol.Semicolon,
            Symbol.EndOfLine
        };

        /// <summary>
        /// Символы, ожидаемые сразу после анализа константы при вызове функции constant() из casefield() и variant()	
        /// </summary>
        public static readonly Symbol[] acodes_2constant = new[]
        {
            Symbol.Comma,
            Symbol.Colon,
            Symbol.EndOfLine
        };

        /// <summary>
        /// Коды символов, ожидаемых сразу после анализа константы
        /// </summary>
        public static readonly Symbol[] acodes_3const = new[]
        {
            Symbol.TwoPoints,
            Symbol.Comma,
            Symbol.RightSquareBracket,
            Symbol.EndOfLine
        };

        /// <summary>
        /// Символы, ожидаемые сразу после списка параметров ( символы functionsy,proceduresy,beginsy уже есть в followers) 
        /// </summary>
        public static readonly Symbol[] acodes_listparam = new[]
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
        public static readonly Symbol[] acodes_factparam = new[]
        {
            Symbol.Comma,
            Symbol.RightRoundBracket,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символ, ожидаемый сразу после переменной в операторе присваивания и в операторе for 
        /// </summary>
        public static readonly Symbol[] acodes_assign = new[]
        {
            Symbol.Assign,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символы, ожидаемые сразу после оператора в составном операторе и после варианта в операторе варианта
        /// </summary>
        public static readonly Symbol[] acodes_compcase = new[]
        {
            Symbol.Semicolon,
            Symbol.End,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символ, ожидаемый сразу после условного выражения в операторе if
        /// </summary>
        public static readonly Symbol[] acodes_iftrue = new[]
        {
            Symbol.Then,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символ, ожидаемый сразу после оператора ветви "истина" в операторе if
        /// </summary>
        public static readonly Symbol[] acodes_iffalse = new[]
        {
            Symbol.Else,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символы, ожидаемые сразу после переменной в заголовке оператора with
        /// </summary>
        public static readonly Symbol[] acodes_wiwifor = new[]
        {
            Symbol.Comma,
            Symbol.Do,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символ, ожидаемый сразу после условного выражения в операторе while 
        /// и сразу после выражения-второй границы изменения параметра цикла в операторе for
        /// </summary>
        public static readonly Symbol[] acodes_while = new[]
        {
            Symbol.Do,
            Symbol.EndOfLine
        };

        /// <summary>
        /// cимволы, ожидаемые сразу после оператора в теле оператора repeat
        /// </summary>
        public static readonly Symbol[] acodes_repeat = new[]
        {
            Symbol.Until,
            Symbol.Semicolon,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символ, ожидаемый сразу после выбирающего выражения в операторе case
        /// </summary>
        public static readonly Symbol[] acodes_case1 = new[]
        {
            Symbol.Of,
            Symbol.EndOfLine
        };

        /// <summary>
        /// символы, ожидаемые сразу после выражения-первой границы изменения пераметра цикла в операторе for
        /// </summary>
        public static readonly Symbol[] acodes_for1 = new[]
        {
            Symbol.To,
            Symbol.DownTo,
            Symbol.EndOfLine
        };

        /// <summary>
        /// после идентификатора в переменной
        /// </summary>
        public static readonly Symbol[] acodes_ident = new[]
        {
            Symbol.LeftSquareBracket,
            Symbol.Caret,
            Symbol.Point,
            Symbol.EndOfLine
        };

        /// <summary>
        /// после индексного выражения при разборе массива
        /// </summary>
        public static readonly Symbol[] acodes_index = new[]
        {
            Symbol.RightSquareBracket,
            Symbol.Comma,
            Symbol.EndOfLine
        };

        /// <summary>
        /// после 1-го выражения в конструкторе множества
        /// </summary>
        public static readonly Symbol[] acodes_set1 = new[]
        {
            Symbol.RightSquareBracket,
            Symbol.TwoPoints,
            Symbol.Comma,
            Symbol.EndOfLine
        };
    }
}