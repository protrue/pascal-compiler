using static PascalCompiler.Constants;

public static partial class Constants
{

    public static class Starters
    {
        /// <summary>
        /// Стартовые символы разделов описаний и раздела операторов
        /// </summary>
        public static readonly Symbol[] Block = new[]
        {
            Symbol.Const,
            Symbol.Type,
            Symbol.Var,
            Symbol.Begin
        };

        /// <summary>
        /// Правая скобка
        /// </summary>
        public static readonly Symbol[] RightPar = new[]
        {
            Symbol.RightRoundBracket,
            Symbol.EndOfLine
        };

        /// <summary>
        /// Стартовые символы конструкции constant
        /// </summary>
        public static readonly Symbol[] Const = new[] {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.CharConstant,
            Symbol.StringConstant,
            Symbol.Identifier,
            Symbol.IntegerConstant,
            Symbol.FloatConstant
        };

        public static readonly Symbol[] ConstPart = new[]
        {
            Symbol.Const,
            Symbol.Type,
            Symbol.Var,
            Symbol.Begin,
        };

        public static readonly Symbol[] ConstDeclaration = new[]
        {
            Symbol.Identifier
        };

        public static readonly Symbol[] TypePart = new[]
        {
            Symbol.Type,
            Symbol.Var,
            Symbol.Begin,
        };

        public static readonly Symbol[] TypeDeclaration = new[]
        {
            Symbol.Identifier
        };

        /// <summary>
        /// Стартовые символы конструкции описания типа
        /// </summary>
        public static readonly Symbol[] Type = new[]
        {
            Symbol.Array,
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Identifier,
            Symbol.LeftRoundBracket,
            Symbol.IntegerConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant,
        };

        public static readonly Symbol[] SimpleType = new[]
        {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Identifier,
            Symbol.LeftRoundBracket,
            Symbol.IntegerConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant
        };

        public static readonly Symbol[] EnumerationType = new[]
        {
            Symbol.LeftRoundBracket
        };

        public static readonly Symbol[] LimitedType = new[]
        {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Identifier,
            Symbol.IntegerConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant
        };

        public static readonly Symbol[] CompositeType = new[]
        {
            Symbol.Array
        };

        public static readonly Symbol[] ArrayType = new[]
        {
            Symbol.Array
        };

        public static readonly Symbol[] VarPart = new[]
        {
            Symbol.Var
        };

        public static readonly Symbol[] VarDeclaration = new[]
        {
            Symbol.Identifier
        };

        public static readonly Symbol[] CompoundStatement = new[]
        {
            Symbol.Begin
        };

        public static readonly Symbol[] Statement = new[]
        {
            Symbol.Begin,
            Symbol.If,
            Symbol.While,
            Symbol.Identifier,
            Symbol.Semicolon
        };

        public static readonly Symbol[] SimpleStatement = new[]
        {
            Symbol.Identifier,
            Symbol.Semicolon
        };

        public static readonly Symbol[] AssignmentStatement = new[]
        {
            Symbol.Identifier
        };

        public static readonly Symbol[] Variable = new[]
        {
            Symbol.Identifier
        };

        public static readonly Symbol[] Expression = new[]
        {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Identifier,
            Symbol.LeftRoundBracket,
            Symbol.Not,
            Symbol.IntegerConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant,
            Symbol.Identifier,
            Symbol.Nil
        };

        public static readonly Symbol[] SimpleExpression = new[]
        {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Identifier,
            Symbol.LeftRoundBracket,
            Symbol.Not,
            Symbol.IntegerConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant,
            Symbol.Identifier,
            Symbol.Nil
        };

        public static readonly Symbol[] Sign = new[]
        {
            Symbol.Plus,
            Symbol.Minus,

            Symbol.Identifier
        };

        public static readonly Symbol[] Term = new[]
        {
            Symbol.Identifier,
            Symbol.IntegerConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant,
            Symbol.Nil,
            Symbol.LeftRoundBracket,
            Symbol.Not
        };

        public static readonly Symbol[] Factor = new[]
        {
            Symbol.Identifier,
            Symbol.StringConstant,
            Symbol.FloatConstant,
            Symbol.CharConstant,
            Symbol.StringConstant,
            Symbol.Nil,
            Symbol.LeftRoundBracket,
            Symbol.Not
        };

        public static readonly Symbol[] MultiplicativeOperation = new[]
        {
            Symbol.Asterisk,
            Symbol.Slash,
            Symbol.Div,
            Symbol.Mod,
            Symbol.And,
        };

        public static readonly Symbol[] AdditiveOperation = new[]
        {
            Symbol.Plus,
            Symbol.Minus,
            Symbol.Or
        };

        public static readonly Symbol[] ComplexStatement = new[]
        {
            Symbol.Begin,
            Symbol.If,
            Symbol.While,
        };

        public static readonly Symbol[] ConditionalStatement = new[]
        {
            Symbol.If,
        };

        public static readonly Symbol[] WhileStatement = new[]
        {
            Symbol.While,
        };
    }
}