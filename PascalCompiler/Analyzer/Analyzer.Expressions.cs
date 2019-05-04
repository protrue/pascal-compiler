using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer
    {
        private void AnalyzeExpression(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.Expression))
            {
                InsertError(1004);
                SkipTo(Starters.Expression, followers);
            }

            if (!Belongs(Starters.Expression)) return;

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
                    AnalyzeSimpleExpression(Union(RelationalOperators, followers));
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1004);
                    break;
            }

            if (RelationalOperators.Contains(CurrentSymbol))
            {
                AnalyzeRelationalOperators(Followers.RelationalOperators);
                AnalyzeSimpleExpression(Union(Followers.SimpleExpression, followers));
            }

            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }
        }

        private void AnalyzeSimpleExpression(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.SimpleExpression))
            {
                InsertError(1004);
                SkipTo(Starters.SimpleExpression, followers);
            }

            if (!Belongs(Starters.SimpleExpression)) return;

            switch (CurrentSymbol)
            {
                case Symbol.Plus:
                    AcceptTerminal(Symbol.Plus);
                    break;
                case Symbol.Minus:
                    AcceptTerminal(Symbol.Minus);
                    break;
            }

            AnalyzeAddend(Union(Followers.Addend, followers));
            while (AdditiveOperators.Contains(CurrentSymbol))
            {
                AnalyzeAdditiveOperators(Union(Followers.AdditiveOperators, followers));
                AnalyzeAddend(Union(Followers.Addend, followers));
            }

            if (!Belongs(followers))
            {
                InsertError(1010);
                SkipTo(followers);
            }
        }

        private void AnalyzeRelationalOperators(HashSet<Symbol> followers)
        {
            if (!Belongs(RelationalOperators))
            {
                InsertError(1001);
                SkipTo(RelationalOperators, followers);
            }

            if (!Belongs(RelationalOperators)) return;

            switch (CurrentSymbol)
            {
                case Symbol.Equals:
                case Symbol.NotEqual:
                case Symbol.Less:
                case Symbol.LessOrEqual:
                case Symbol.Greater:
                case Symbol.GreaterOrEqual:
                case Symbol.In:
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
        }

        private void AnalyzeAdditiveOperators(HashSet<Symbol> followers)
        {
            if (!Belongs(AdditiveOperators))
            {
                InsertError(1002);
                SkipTo(AdditiveOperators, followers);
            }

            if (!Belongs(AdditiveOperators)) return;
            
            switch (CurrentSymbol)
            {
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.Or:
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
        }

        private void AnalyzeMultiplicativeOperators(HashSet<Symbol> followers)
        {
            if (!Belongs(MultiplicativeOperators))
            {
                InsertError(1003);
                SkipTo(MultiplicativeOperators, followers);
            }

            if (!Belongs(MultiplicativeOperators)) return;
            
            switch (CurrentSymbol)
            {
                case Symbol.Asterisk:
                case Symbol.Slash:
                case Symbol.And:
                case Symbol.Div:
                case Symbol.Mod:
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
        }

        private void AnalyzeAddend(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.Addend))
            {
                InsertError(1006);
                SkipTo(Starters.Addend, followers);
            }

            if (!Belongs(Starters.Addend)) return;

            AnalyzeMultiplicand(Union(Followers.Multiplicand, followers));
            while (MultiplicativeOperators.Contains(CurrentSymbol))
            {
                AnalyzeMultiplicativeOperators(Union(Followers.MultiplicativeOperators, followers));
                AnalyzeMultiplicand(Union(Followers.Multiplicand, followers));
            }

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeMultiplicand(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.Multiplicand))
            {
                InsertError(1005);
                SkipTo(Starters.Multiplicand, followers);
            }

            if (!Belongs(Starters.Multiplicand)) return;

            switch (CurrentSymbol)
            {
                // TODO: Check semantics here
                case Symbol.IntegerConstant:
                case Symbol.FloatConstant:
                case Symbol.CharConstant:
                case Symbol.StringConstant:
                case Symbol.True:
                case Symbol.False:
                case Symbol.Identifier:
                    GetNextToken();
                    break;
                case Symbol.Not:
                    AcceptTerminal(Symbol.Not);
                    AnalyzeMultiplicand(Union(Followers.Multiplicand, followers));
                    break;
                case Symbol.LeftRoundBracket:
                    AcceptTerminal(Symbol.LeftRoundBracket);
                    AnalyzeExpression(Union(Followers.Expression, followers));
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
        }
    }
}
