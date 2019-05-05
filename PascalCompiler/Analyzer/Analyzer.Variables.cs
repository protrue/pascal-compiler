using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer
    {
        private Type AnalyzeVariable()
        {
            var type = new Type();
            
            AcceptTerminal(Symbol.Identifier);
            if (AcceptedToken != null)
            {
                var entity = Search(AcceptedToken.TextValue);
                if (entity == null)
                {
                    IoManager.InsertError(AcceptedToken.CharacterNumber, 104);
                }
                else
                {
                    if (entity.IdentifierClass != IdentifierClass.Variable)
                    {
                        IoManager.InsertError(AcceptedToken.CharacterNumber, 1011);
                    }
                    else
                    {
                        var variable = SearchVariable(entity.Identifier);

                        switch (variable.Type.BaseType)
                        {
                            case BaseType.Scalar:
                                type.BaseType = BaseType.Scalar;
                                type.ScalarType = variable.Type.ScalarType;
                                break;
                            case BaseType.Record:
                                StepBack();
                                AnalyzeRecordVariable(variable);
                                break;
                        }
                    }
                }
            }

            return type;
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

        private Type AnalyzeRecordVariable(Variable variable)
        {
            var type = new Type();

            AcceptTerminal(Symbol.Identifier);
            var record = SearchVariable(AcceptedToken.TextValue);
            AcceptTerminal(Symbol.Point);
            AcceptTerminal(Symbol.Identifier);
            if (AcceptedToken != null)
            {
                var field = record.Type.Record.Fields.FirstOrDefault(f => f.Identifier == AcceptedToken.TextValue);
                if (field == null)
                {
                    IoManager.InsertError(AcceptedToken.CharacterNumber,152);
                }
                else
                {
                    switch (field.Type.BaseType)
                    {
                        case BaseType.Scalar:
                            type.BaseType = BaseType.Scalar;
                            type.ScalarType = field.Type.ScalarType;
                            break;
                        case BaseType.Record:
                            StepBack();
                            type = AnalyzeRecordVariable(field);
                            break;
                    }
                }
            }

            return type;
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
