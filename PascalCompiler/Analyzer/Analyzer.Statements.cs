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
                case Symbol.Semicolon:
                    AcceptTerminal(Symbol.Semicolon);
                    break;
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

            var leftType = AnalyzeVariable();
            AcceptTerminal(Symbol.Assign);
            var rightType = AnalyzeExpression(Union(Followers.Expression, followers));

            switch (rightType)
            {
                case ScalarType.Unknown:
                    break;
                case ScalarType.Integer:
                    if (leftType.ScalarType != ScalarType.Integer && leftType.ScalarType != ScalarType.Real)
                    {
                        if (_previousToken != null)
                            IoManager.InsertError(_previousToken.CharacterNumber, 145);
                        else
                            InsertError(145);
                    }
                    break;
                case ScalarType.Real:
                    if (leftType.ScalarType != ScalarType.Integer && leftType.ScalarType != ScalarType.Real)
                    {
                        if (_previousToken != null)
                            IoManager.InsertError(_previousToken.CharacterNumber, 145);
                        else
                            InsertError(145);
                    }
                    break;
                case ScalarType.Char:
                    if (leftType.ScalarType != ScalarType.Char)
                    {
                        if (_previousToken != null)
                            IoManager.InsertError(_previousToken.CharacterNumber, 145);
                        else
                            InsertError(145);
                    }
                    break;
                case ScalarType.String:
                    if (leftType.ScalarType != ScalarType.String)
                    {
                        if (_previousToken != null)
                            IoManager.InsertError(_previousToken.CharacterNumber, 145);
                        else
                            InsertError(145);
                    }
                    break;
                case ScalarType.Boolean:
                    if (leftType.ScalarType != ScalarType.Boolean)
                    {
                        if (_previousToken != null)
                            IoManager.InsertError(_previousToken.CharacterNumber, 145);
                        else
                            InsertError(145);
                    }
                    break;
            }

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

            var type = AnalyzeVariable();
            if (type.BaseType != BaseType.Record)
            {
                InsertError(140);
            }
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                type = AnalyzeVariable();
                if (type.BaseType != BaseType.Record)
                {
                    InsertError(140);
                }
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
            var type = AnalyzeExpression(Union(Followers.Expression, followers, Followers.IfStatementExpression));
            if (type != ScalarType.Boolean)
            {
                InsertError(135);
            }
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
            var type = AnalyzeExpression(Union(Followers.Expression, followers, Followers.WhileStatementExpression));
            if (type != ScalarType.Boolean)
            {
                InsertError(135);
            }
            AcceptTerminal(Symbol.Do);
            AnalyzeStatement(followers);
        }
    }
}
