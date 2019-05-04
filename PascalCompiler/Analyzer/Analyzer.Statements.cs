using System;
using System.Collections.Generic;
using System.Text;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer
    {
        private void AnalyzeStatement(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.Statement))
            {
                InsertError(1009);
                SkipTo(Starters.Statement, followers);
            }

            if (!Belongs(Starters.Statement)) return;

            if (CurrentSymbol == Symbol.IntegerConstant)
            {
                AcceptTerminal(Symbol.IntegerConstant);
                AnalyzeNonLabeledStatement(followers);
            }
            else
            {
                AnalyzeNonLabeledStatement(followers);
            }

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeNonLabeledStatement(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.NonLabeledStatement))
            {
                InsertError(1009);
                SkipTo(Starters.NonLabeledStatement, followers);
            }

            if (!Belongs(Starters.NonLabeledStatement)) return;

            switch (CurrentSymbol)
            {
                case Symbol.Identifier:
                case Symbol.Goto:
                    AnalyzeSimpleStatement(followers);
                    break;
                case Symbol.Begin:
                case Symbol.For:
                case Symbol.While:
                case Symbol.Repeat:
                case Symbol.If:
                case Symbol.Case:
                case Symbol.With:
                    AnalyzeComplexStatement(followers);
                    break;
            }
        }

        private void AnalyzeSimpleStatement(HashSet<Symbol> followers)
        {
            switch (CurrentSymbol)
            {
                case Symbol.Identifier:
                    AnalyzeAssignStatement(followers);
                    break;
                case Symbol.Goto:
                    AnalyzeGotoStatement(followers);
                    break;
            }
        }

        private void AnalyzeAssignStatement(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.AssignStatement))
            {
                SkipTo(Starters.AssignStatement, followers);
            }

            if (!Belongs(Starters.Statement)) return;

            AnalyzeVariable();
            AcceptTerminal(Symbol.Assign);
            AnalyzeExpression(Union(Followers.Expression, followers));

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeProcedureStatement(HashSet<Symbol> followers)
        {

        }

        private void AnalyzeGotoStatement(HashSet<Symbol> followers)
        {

        }

        private void AnalyzeComplexStatement(HashSet<Symbol> followers)
        {
            switch (CurrentSymbol)
            {
                case Symbol.Begin:
                    AnalyzeCompoundStatement(followers);
                    break;
                case Symbol.While:
                    AnalyzeWhileStatement(followers);
                    break;
                case Symbol.If:
                    AnalyzeIfStatement(followers);
                    break;
                case Symbol.Case:
                    AnalyzeCaseStatement(followers);
                    break;
                case Symbol.With:
                    AnalyzeWithStatement(followers);
                    break;
            }
        }

        private void AnalyzeCompoundStatement(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.Begin);
            AnalyzeStatement(followers);
            while (CurrentSymbol == Symbol.Semicolon)
            {
                AcceptTerminal(Symbol.Semicolon);
                AnalyzeStatement(followers);
            }
            AcceptTerminal(Symbol.End);
        }

        private void AnalyzeWithStatement(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.With);
            AnalyzeRecordVariablesList(Union(Followers.RecordVariablesList, followers));
            AcceptTerminal(Symbol.Do);
            AnalyzeStatement(followers);
        }

        private void AnalyzeRecordVariablesList(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.RecordVariablesList))
            {
                InsertError(1009);
                SkipTo(Starters.RecordVariablesList, followers);
            }

            if (!Belongs(Starters.RecordVariablesList)) return;

            AnalyzeVariable();
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AnalyzeVariable();
            }

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeCaseStatement(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.Case);
            AnalyzeExpression(Union(Followers.Expression, followers, Followers.Case));
            AcceptTerminal(Symbol.Of);
            AnalyzeCaseElement(Union(Followers.CaseElement, followers));
            while (CurrentSymbol == Symbol.Semicolon)
            {
                AcceptTerminal(Symbol.Semicolon);
                AnalyzeCaseElement(Union(Followers.CaseElement, followers));
            }
            AcceptTerminal(Symbol.End);

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeCaseElement(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.Constant))
            {
                InsertError(1008);
                SkipTo(Starters.Constant, followers);
            }

            if (!Belongs(Starters.Constant)) return;

            AnalyzeConstant(Union(Followers.Constant, followers, Followers.CaseConstant));
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AnalyzeConstant(Union(Followers.Constant, followers, Followers.CaseConstant));
            }
            AcceptTerminal(Symbol.Colon);
            AnalyzeStatement(followers);

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeIfStatement(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.IfStatement))
            {
                InsertError(1008);
                SkipTo(Starters.IfStatement, followers);
            }

            if (!Belongs(Starters.IfStatement)) return;

            AcceptTerminal(Symbol.If);
            AnalyzeExpression(Union(Followers.Expression, followers, Followers.IfStatementExpression));
            AcceptTerminal(Symbol.Then);
            AnalyzeStatement(Union(followers, Followers.IfStatementTrueBlock));
            if (CurrentSymbol == Symbol.Else)
            {
                AcceptTerminal(Symbol.Else);
                AnalyzeStatement(followers);
            }

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeWhileStatement(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.WhileStatement))
            {
                SkipTo(Starters.WhileStatement, followers);
            }

            if (!Belongs(Starters.WhileStatement)) return;

            AcceptTerminal(Symbol.While);
            AnalyzeExpression(Union(Followers.Expression, followers, Followers.WhileStatementExpression));
            AcceptTerminal(Symbol.Do);
            AnalyzeStatement(followers);
        }
    }
}
