using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer
    {
        private ScalarType AnalyzeExpression(HashSet<Symbol> followers)
        {
            var resultType = ScalarType.Unknown;

            if (!Belongs(Starters.Expression))
            {
                InsertError(1004);
                SkipTo(Starters.Expression, followers);
            }

            if (!Belongs(Starters.Expression)) return resultType;

            switch (CurrentSymbol)
            {
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.Identifier:
                case Symbol.IntegerConstant:
                case Symbol.FloatConstant:
                case Symbol.CharConstant:
                case Symbol.StringConstant:
                case Symbol.True:
                case Symbol.False:
                case Symbol.LeftRoundBracket:
                case Symbol.Not:
                    resultType = AnalyzeSimpleExpression(Union(RelationalOperators, followers));
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1004);
                    break;
            }

            if (RelationalOperators.Contains(CurrentSymbol))
            {
                var operation = AnalyzeRelationalOperators(Followers.RelationalOperators);
                var secondType = AnalyzeSimpleExpression(Union(Followers.SimpleExpression, followers));
                if (resultType != ScalarType.Boolean || secondType != ScalarType.Boolean)
                {
                    InsertError(144);
                    resultType = ScalarType.Unknown;
                }
                else
                {
                    resultType = ScalarType.Boolean;
                }
            }

            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }

            return resultType;
        }

        private ScalarType AnalyzeSimpleExpression(HashSet<Symbol> followers)
        {
            var resultType = ScalarType.Unknown;

            if (!Belongs(Starters.SimpleExpression))
            {
                InsertError(1004);
                SkipTo(Starters.SimpleExpression, followers);
            }

            if (!Belongs(Starters.SimpleExpression)) return resultType;

            var wasSign = false;
            switch (CurrentSymbol)
            {
                case Symbol.Plus:
                    AcceptTerminal(Symbol.Plus);
                    wasSign = true;
                    break;
                case Symbol.Minus:
                    AcceptTerminal(Symbol.Minus);
                    wasSign = true;
                    break;
            }

            resultType = AnalyzeAddend(Union(Followers.Addend, followers));
            if (wasSign && (resultType != ScalarType.Real || resultType != ScalarType.Integer))
            {
                InsertError(184);
            }
            while (AdditiveOperators.Contains(CurrentSymbol))
            {
                var operation = AnalyzeAdditiveOperators(Union(Followers.AdditiveOperators, followers));
                var secondType = AnalyzeAddend(Union(Followers.Addend, followers));
                switch (operation)
                {
                    case Symbol.Plus:
                    case Symbol.Minus:
                        var acceptedTypes = new[] {ScalarType.Integer, ScalarType.Real};
                        if (!acceptedTypes.Contains(resultType) || !acceptedTypes.Contains(secondType))
                        {
                            InsertError(211);
                            resultType = ScalarType.Unknown;
                        }
                        else
                        {
                            if (resultType == ScalarType.Real || secondType == ScalarType.Real)
                            {
                                resultType = ScalarType.Real;
                            }
                            else
                            {
                                resultType = ScalarType.Integer;
                            }
                        }
                        break;
                    case Symbol.Or:
                        if (resultType != ScalarType.Boolean || secondType != ScalarType.Boolean)
                        {
                            InsertError(210);
                            resultType = ScalarType.Unknown;
                        }
                        else
                        {
                            resultType = ScalarType.Boolean;
                        }
                        break;
                }
            }

            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }

            return resultType;
        }

        private Symbol AnalyzeRelationalOperators(HashSet<Symbol> followers)
        {
            var symbol = Symbol.EndOfLine;

            if (!Belongs(RelationalOperators))
            {
                InsertError(1001);
                SkipTo(RelationalOperators, followers);
            }

            if (!Belongs(RelationalOperators)) return symbol;

            switch (CurrentSymbol)
            {
                case Symbol.Equals:
                case Symbol.NotEqual:
                case Symbol.Less:
                case Symbol.LessOrEqual:
                case Symbol.Greater:
                case Symbol.GreaterOrEqual:
                case Symbol.In:
                    symbol = CurrentSymbol;
                    GetNextToken();
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1001);
                    break;
            }

            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }

            return symbol;
        }

        private Symbol AnalyzeAdditiveOperators(HashSet<Symbol> followers)
        {
            var symbol = Symbol.EndOfLine;

            if (!Belongs(AdditiveOperators))
            {
                InsertError(1002);
                SkipTo(AdditiveOperators, followers);
            }

            if (!Belongs(AdditiveOperators)) return symbol;

            switch (CurrentSymbol)
            {
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.Or:
                    symbol = CurrentSymbol;
                    GetNextToken();
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1002);
                    break;
            }


            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }

            return symbol;
        }

        private Symbol AnalyzeMultiplicativeOperators(HashSet<Symbol> followers)
        {
            var symbol = Symbol.EndOfLine;

            if (!Belongs(MultiplicativeOperators))
            {
                InsertError(1003);
                SkipTo(MultiplicativeOperators, followers);
            }

            if (!Belongs(MultiplicativeOperators)) return symbol;

            switch (CurrentSymbol)
            {
                case Symbol.Asterisk:
                case Symbol.Slash:
                case Symbol.And:
                case Symbol.Div:
                case Symbol.Mod:
                    symbol = CurrentSymbol;
                    GetNextToken();
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1003);
                    break;
            }

            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }

            return symbol;
        }

        private ScalarType AnalyzeAddend(HashSet<Symbol> followers)
        {
            var resultType = ScalarType.Unknown;

            if (!Belongs(Starters.Addend))
            {
                InsertError(1006);
                SkipTo(Starters.Addend, followers);
            }

            if (!Belongs(Starters.Addend)) return resultType;

            resultType = AnalyzeMultiplicand(Union(Followers.Multiplicand, followers));
            while (MultiplicativeOperators.Contains(CurrentSymbol))
            {
                var operation = AnalyzeMultiplicativeOperators(Union(Followers.MultiplicativeOperators, followers));
                var secondType = AnalyzeMultiplicand(Union(Followers.Multiplicand, followers));
                switch (operation)
                {
                    case Symbol.And:
                        if (resultType != ScalarType.Boolean || secondType != ScalarType.Boolean)
                        {
                            InsertError(210);
                            resultType = ScalarType.Unknown;
                        }
                        else
                        {
                            resultType = ScalarType.Boolean;
                        }
                        break;
                    case Symbol.Div:
                    case Symbol.Mod:
                        if (resultType != ScalarType.Integer || secondType != ScalarType.Integer)
                        {
                            InsertError(212);
                            resultType = ScalarType.Unknown;
                        }
                        else
                        {
                            resultType = ScalarType.Integer;
                        }
                        break;
                    case Symbol.Asterisk:
                    case Symbol.Slash:
                        var acceptedTypes = new[] { ScalarType.Integer, ScalarType.Real };
                        if (!acceptedTypes.Contains(resultType) || !acceptedTypes.Contains(secondType))
                        {
                            InsertError(1012);
                            resultType = ScalarType.Unknown;
                        }
                        else
                        {
                            if (resultType == ScalarType.Real || secondType == ScalarType.Real)
                            {
                                resultType = ScalarType.Real;
                            }
                            else
                            {
                                resultType = ScalarType.Integer;
                            }
                        }
                        break;
                }
            }

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }

            return resultType;
        }

        private ScalarType AnalyzeMultiplicand(HashSet<Symbol> followers)
        {
            var type = ScalarType.Unknown;

            if (!Belongs(Starters.Multiplicand))
            {
                InsertError(1005);
                SkipTo(Starters.Multiplicand, followers);
            }

            if (!Belongs(Starters.Multiplicand)) return type;

            switch (CurrentSymbol)
            {
                case Symbol.IntegerConstant:
                    AcceptTerminal(Symbol.IntegerConstant);
                    type = ScalarType.Integer;
                    break;
                case Symbol.FloatConstant:
                    AcceptTerminal(Symbol.FloatConstant);
                    type = ScalarType.Real;
                    break;
                case Symbol.CharConstant:
                    AcceptTerminal(Symbol.CharConstant);
                    type = ScalarType.Char;
                    break;
                case Symbol.StringConstant:
                    AcceptTerminal(Symbol.StringConstant);
                    type = ScalarType.String;
                    break;
                case Symbol.True:
                    AcceptTerminal(Symbol.True);
                    type = ScalarType.Boolean;
                    break;
                case Symbol.False:
                    AcceptTerminal(Symbol.False);
                    type = ScalarType.Boolean;
                    break;
                case Symbol.Identifier:
                    AcceptTerminal(Symbol.Identifier);
                    var entity = Search(AcceptedToken.TextValue);
                    if (entity != null)
                    {
                        switch (entity.IdentifierClass)
                        {
                            case IdentifierClass.Constant:
                                type = SearchConstant(AcceptedToken.TextValue).Type;
                                break;
                            case IdentifierClass.Variable:
                                var variableType = AnalyzeVariable();
                                type = variableType.ScalarType ?? ScalarType.Unknown;
                                break;
                        }
                    }

                    break;
                case Symbol.Not:
                    AcceptTerminal(Symbol.Not);
                    type = AnalyzeMultiplicand(Union(Followers.Multiplicand, followers));
                    if (type != ScalarType.Boolean)
                    {
                        IoManager.InsertError(AcceptedToken.CharacterNumber, 135);
                    }
                    break;
                case Symbol.LeftRoundBracket:
                    AcceptTerminal(Symbol.LeftRoundBracket);
                    type = AnalyzeExpression(Union(Followers.Expression, followers));
                    AcceptTerminal(Symbol.RightRoundBracket);
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1007);
                    break;
            }

            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }

            return type;
        }
    }
}
