using System;
using System.Collections.Generic;
using System.Text;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer
    {
        private void AnalyzeVariable()
        {
            AcceptTerminal(Symbol.Identifier);
        }

        private void AnalyzeFullVariable()
        {
            AnalyzeVariableName();
        }

        private void AnalyzeVariableName()
        {
            AcceptTerminal(Symbol.Identifier);
        }

        private void AnalyzeVariableComponent()
        {

        }

        private void AnalyzeIndexedVariable()
        {
            AnalyzeVariable();
            AcceptTerminal(Symbol.LeftSquareBracket);
            AnalyzeExpression(Followers.Expression);
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AnalyzeExpression(Followers.Expression);
            }
            AcceptTerminal(Symbol.RightSquareBracket);
        }

        private void AnalyzeRecordFieldVariable()
        {
            AnalyzeVariable();
            AcceptTerminal(Symbol.Point);
            AcceptTerminal(Symbol.Identifier);
        }

        private void AnalyzeReferencedVariable()
        {
            AnalyzeVariable();
            AcceptTerminal(Symbol.Caret);
        }

        private void AnalyzeFileBufferVariable()
        {
            AnalyzeVariable();
            AcceptTerminal(Symbol.Caret);
        }
    }
}
