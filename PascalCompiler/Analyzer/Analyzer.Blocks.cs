using System.Collections.Generic;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer
    {
        private void AnalyzeBlocks()
        {
            AnalyzeProgramBlock(Followers.ProgramBlock);
            AnalyzeLabelsBlock(Followers.LabelsBlock);
            AnalyzeConstantsBlock(Followers.ConstantsBlock);
            AnalyzeTypesBlock(Followers.TypesBlock);
            AnalyzeVariablesBlock(Followers.VariablesBlock);
            AnalyzeFunctionsBlock(Followers.FunctionsBlock);
            AnalyzeStatementsBlock(Followers.StatementsBlock);
        }

        private void AnalyzeProgramBlock(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.ProgramBlock))
            {
                SkipTo(Starters.ProgramBlock, followers);
            }

            if (!Belongs(Starters.ProgramBlock)) return;

            AcceptTerminal(Symbol.Program);
            AcceptTerminal(Symbol.Identifier);
            if (AcceptedToken != null)
            {
                CurrentScope.Entities.Add(new Entity() { Identifier = AcceptedToken.TextValue, IdentifierClass = IdentifierClass.Program });
            }
            if (CurrentSymbol == Symbol.LeftRoundBracket)
            {
                AcceptTerminal(Symbol.LeftRoundBracket);
                AcceptTerminal(Symbol.Identifier);
                while (CurrentSymbol == Symbol.Comma)
                {
                    AcceptTerminal(Symbol.Comma);
                    AcceptTerminal(Symbol.Identifier);
                }
            }
            AcceptTerminal(Symbol.Semicolon);

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeLabelsBlock(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.LabelsBlock))
            {
                SkipTo(Starters.LabelsBlock, followers);
            }

            if (!Belongs(Starters.LabelsBlock)) return;

            AcceptTerminal(Symbol.Label);
            AcceptTerminal(Symbol.IntegerConstant);

            if (!Belongs(followers))
            {
                InsertError(6);
                SkipTo(followers);
            }
        }

        private void AnalyzeConstantsBlock(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.ConstantsBlock))
            {
                SkipTo(Starters.ConstantsBlock, followers);
            }

            if (!Belongs(Starters.ConstantsBlock)) return;

            AcceptTerminal(Symbol.Const);
            do
            {
                AnalyzeConstantDeclaration(Union(Followers.ConstantDeclaration, followers));
            } while (CurrentSymbol == Symbol.Identifier);

            if (!Belongs(followers))
            {
                InsertError(50);
                SkipTo(followers);
            }
        }

        private void AnalyzeConstantDeclaration(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.ConstantDeclaration))
            {
                SkipTo(Starters.ConstantDeclaration, followers);
            }

            if (!Belongs(Starters.ConstantDeclaration)) return;

            var constant = new Constant();

            AcceptTerminal(Symbol.Identifier);
            if (AcceptedToken != null && Search(AcceptedToken.TextValue) != null)
            {
                IoManager.InsertError(AcceptedToken.CharacterNumber, 101);
            }
            if (AcceptedToken != null)
            {
                CurrentScope.Entities.Add(new Entity() { Identifier = AcceptedToken.TextValue, IdentifierClass = IdentifierClass.Constant });
                constant.Identifier = AcceptedToken.TextValue;
                CurrentScope.Constants.Add(constant);
            }
            AcceptTerminal(Symbol.Equals);
            AnalyzeConstant(Union(Followers.Constant, followers));
            if (AcceptedToken != null)
            {
                switch (AcceptedToken.Symbol)
                {
                    case Symbol.IntegerConstant:
                        constant.Type = ScalarType.Integer;
                        break;
                    case Symbol.FloatConstant:
                        constant.Type = ScalarType.Real;
                        break;
                    case Symbol.StringConstant:
                        constant.Type = ScalarType.String;
                        break;
                    case Symbol.CharConstant:
                        constant.Type = ScalarType.Char;
                        break;
                    case Symbol.True:
                    case Symbol.False:
                        constant.Type = ScalarType.Boolean;
                        break;
                }
            }
            AcceptTerminal(Symbol.Semicolon);


            if (!Belongs(followers))
            {
                InsertError(50);
                SkipTo(followers);
            }
        }

        private void AnalyzeConstant(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.Constant))
            {
                InsertError(1008);
                SkipTo(Starters.ConstantDeclaration, followers);
            }

            if (!Belongs(Starters.Constant)) return;

            if (CurrentSymbol == Symbol.Plus || CurrentSymbol == Symbol.Minus)
            {
                GetNextToken();
                switch (CurrentSymbol)
                {
                    case Symbol.Identifier:
                        AcceptTerminal(Symbol.Identifier);
                        break;
                    case Symbol.FloatConstant:
                        AcceptTerminal(Symbol.FloatConstant);
                        break;
                    case Symbol.IntegerConstant:
                        AcceptTerminal(Symbol.IntegerConstant);
                        break;
                    default:
                        IoManager.InsertError(CurrentToken.CharacterNumber, 50);
                        break;
                }
            }
            else
            {
                switch (CurrentSymbol)
                {
                    case Symbol.Identifier:
                        AcceptTerminal(Symbol.Identifier);
                        break;
                    case Symbol.FloatConstant:
                        AcceptTerminal(Symbol.FloatConstant);
                        break;
                    case Symbol.IntegerConstant:
                        AcceptTerminal(Symbol.IntegerConstant);
                        break;
                    case Symbol.CharConstant:
                        AcceptTerminal(Symbol.CharConstant);
                        break;
                    case Symbol.StringConstant:
                        AcceptTerminal(Symbol.StringConstant);
                        break;
                    default:
                        IoManager.InsertError(CurrentToken.CharacterNumber, 83);
                        break;
                }
            }

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeTypesBlock(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.TypesBlock))
            {
                SkipTo(Starters.TypesBlock, followers);
            }

            if (!Belongs(Starters.TypesBlock)) return;

            AcceptTerminal(Symbol.Type);
            do
            {
                _currentType = new Type();
                var type = new Type();
                AcceptTerminal(Symbol.Identifier);
                if (AcceptedToken != null && Search(AcceptedToken.TextValue) != null)
                {
                    IoManager.InsertError(AcceptedToken.CharacterNumber, 101);
                }
                if (AcceptedToken != null)
                {
                    CurrentScope.Entities.Add(new Entity() { Identifier = AcceptedToken.TextValue, IdentifierClass = IdentifierClass.Type });
                    type.Identifier = AcceptedToken.TextValue;
                    CurrentScope.Types.Add(type);
                }
                AcceptTerminal(Symbol.Equals);
                AnalyzeType(Union(Followers.Type, followers));
                type.BaseType = _currentType.BaseType;
                type.Record = _currentType.Record;
                type.ScalarType = _currentType.ScalarType;
                AcceptTerminal(Symbol.Semicolon);
            }
            while (CurrentSymbol == Symbol.Identifier);

            if (!Belongs(followers))
            {
                InsertError(10);
                SkipTo(followers);
            }
        }

        private void AnalyzeVariablesBlock(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.VariablesBlock))
            {
                SkipTo(Starters.VariablesBlock, followers);
            }

            if (!Belongs(Starters.VariablesBlock)) return;

            AcceptTerminal(Symbol.Var);
            do
            {
                AnalyzeSameTypeVariablesDeclaration(Union(Followers.SameTypeVariablesDeclaration, followers));
                AcceptTerminal(Symbol.Semicolon);
            } while (CurrentSymbol == Symbol.Identifier);

            if (!Belongs(followers))
            {
                InsertError(21);
                SkipTo(followers);
            }
        }

        private void AnalyzeSameTypeVariablesDeclaration(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.SameTypeVariableDeclaration))
            {
                InsertError(18);
                SkipTo(Starters.SameTypeVariableDeclaration, followers);
            }

            if (!Belongs(Starters.SameTypeVariableDeclaration)) return;

            _currentVariables = new List<Variable>();
            var variables = new List<Variable>();
            AcceptTerminal(Symbol.Identifier);
            if (AcceptedToken != null && Search(AcceptedToken.TextValue) != null)
            {

                IoManager.InsertError(AcceptedToken.CharacterNumber, 101);
            }
            if (AcceptedToken != null)
            {
                CurrentScope.Entities.Add(new Entity() { Identifier = AcceptedToken.TextValue, IdentifierClass = IdentifierClass.Variable });
                variables.Add(new Variable() { Identifier = AcceptedToken.TextValue });
            }
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AcceptTerminal(Symbol.Identifier);
                if (AcceptedToken != null && Search(AcceptedToken.TextValue) != null)
                {
                    IoManager.InsertError(AcceptedToken.CharacterNumber, 101);
                }
                if (AcceptedToken != null)
                {
                    CurrentScope.Entities.Add(new Entity() { Identifier = AcceptedToken.TextValue, IdentifierClass = IdentifierClass.Variable });
                    variables.Add(new Variable() { Identifier = AcceptedToken.TextValue });
                }
            }
            AcceptTerminal(Symbol.Colon);
            AnalyzeType(Union(Followers.Type, followers));
            foreach (var variable in variables)
            {
                variable.Type = _currentType;
                CurrentScope.Variables.Add(variable);
            }

            if (!Belongs(followers))
            {
                InsertError(21);
                SkipTo(followers);
            }
        }

        private void AnalyzeFunctionsBlock(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.FunctionsBlock))
            {
                SkipTo(Starters.FunctionsBlock, followers);
            }

            if (!Belongs(Starters.FunctionsBlock)) return;

            switch (CurrentSymbol)
            {
                case Symbol.Function:
                case Symbol.Procedure:
                    break;
            }

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeStatementsBlock(HashSet<Symbol> followers)
        {
            if (!Belongs(Starters.StatementsBlock))
            {
                InsertError(18);
                SkipTo(Starters.StatementsBlock, followers);
            }

            if (!Belongs(Starters.StatementsBlock)) return;

            AcceptTerminal(Symbol.Begin);
            AnalyzeStatement(Union(Followers.Statement, followers));
            while (CurrentSymbol == Symbol.Semicolon)
            {
                AcceptTerminal(Symbol.Semicolon);
                AnalyzeStatement(Union(Followers.Statement, followers));
            }
            AcceptTerminal(Symbol.End);
            AcceptTerminal(Symbol.Point);
        }
    }
}
