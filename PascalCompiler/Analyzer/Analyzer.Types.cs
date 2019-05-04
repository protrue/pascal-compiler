using System.Collections.Generic;
using System.Linq;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer
    {
        private void AnalyzeType(HashSet<Symbol> followers)
        {
            _currentType = new Type();

            if (!Belongs(Starters.Type))
            {
                InsertError(331);
                SkipTo(Starters.Type, followers);
            }

            if (!Belongs(Starters.Type)) return;

            switch (CurrentSymbol)
            {
                case Symbol.Identifier:
                case Symbol.LeftRoundBracket:
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.IntegerConstant:
                case Symbol.FloatConstant:
                case Symbol.CharConstant:
                case Symbol.StringConstant:
                    AnalyzeSimpleType(Union(Followers.SimpleType, followers));
                    break;
                case Symbol.Record:
                case Symbol.Array:
                case Symbol.Set:
                case Symbol.File:
                case Symbol.Packed:
                    AnalyzeComplexType(Union(Followers.ComplexType, followers));
                    break;
                case Symbol.Caret:
                    AnalyzeReferenceType(followers);
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 331);
                    break;
            }

            if (!Belongs(followers))
            {
                InsertError(10);
                SkipTo(followers);
            }
        }

        private void AnalyzeSimpleType(HashSet<Symbol> followers)
        {
            switch (CurrentSymbol)
            {
                case Symbol.Identifier:
                    AcceptTerminal(Symbol.Identifier);
                    if (AcceptedToken != null)
                    {
                        var type = SearchType(AcceptedToken.TextValue);
                        if (type == null)
                        {
                            IoManager.InsertError(AcceptedToken.CharacterNumber, 104);
                        }
                        else
                        {
                            _currentType.BaseType = BaseType.Scalar;
                            _currentType.ScalarType = type.ScalarType;
                        }
                    }
                    break;
                case Symbol.LeftRoundBracket:
                    AnalyzeEnumerableType(followers);
                    break;
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.IntegerConstant:
                case Symbol.FloatConstant:
                case Symbol.CharConstant:
                case Symbol.StringConstant:
                    AnalyzeLimitedType(followers);
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1);
                    break;
            }

            if (!Belongs(followers))
            {
                InsertError(1);
                SkipTo(followers);
            }
        }

        private void AnalyzeEnumerableType(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.LeftRoundBracket);
            AcceptTerminal(Symbol.Identifier);
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AcceptTerminal(Symbol.Identifier);
            }
            AcceptTerminal(Symbol.RightRoundBracket);

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeLimitedType(HashSet<Symbol> followers)
        {
            AnalyzeConstant(Union(Followers.Constant, followers));
            AcceptTerminal(Symbol.TwoPoints);
            AnalyzeConstant(Union(Followers.Constant, followers));

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }

        private void AnalyzeComplexType(HashSet<Symbol> followers)
        {
            if (CurrentSymbol == Symbol.Packed)
                AcceptTerminal(Symbol.Packed);
            AnalyzeUnpackedComplexType(followers);
        }

        private void AnalyzeUnpackedComplexType(HashSet<Symbol> followers)
        {
            switch (CurrentSymbol)
            {
                case Symbol.Array:
                    AnalyzeRegularType(followers);
                    break;
                case Symbol.Record:
                    AnalyzeCombinedType(followers);
                    break;
                case Symbol.Set:
                    AnalyzeSetType(followers);
                    break;
                case Symbol.File:
                    AnalyzeFileType(followers);
                    break;
                default:
                    //IoManager.InsertError(CurrentToken.CharacterNumber, ???);
                    break;
            }
        }

        private void AnalyzeRegularType(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.Array);
            AcceptTerminal(Symbol.LeftSquareBracket);
            AnalyzeSimpleType(followers);
            while (CurrentSymbol == Symbol.Comma) ;
            {
                AcceptTerminal(Symbol.Colon);
                AnalyzeSimpleType(followers);
            }
            AcceptTerminal(Symbol.RightSquareBracket);
            AcceptTerminal(Symbol.Of);
            AcceptTerminal(Symbol.Identifier);

            if (!Belongs(followers))
            {
                InsertError(10);
                SkipTo(followers);
            }
        }

        private void AnalyzeCombinedType(HashSet<Symbol> followers)
        {
            _currentType.BaseType = BaseType.Record;
            _currentType.Record = new Record();
            _currentRecord = _currentType;

            AcceptTerminal(Symbol.Record);
            AnalyzeRecordFields(followers);
            AcceptTerminal(Symbol.End);

            _currentType = _currentRecord;

            if (!Belongs(followers))
            {
                InsertError(10);
                SkipTo(followers);
            }
        }

        private void AnalyzeRecordFields(HashSet<Symbol> followers)
        {
            AnalyzeRecordFixedParts(followers);
        }

        private void AnalyzeRecordFixedParts(HashSet<Symbol> followers)
        {
            AnalyzeRecordSection(followers);
            while (CurrentSymbol == Symbol.Semicolon)
            {
                AcceptTerminal(Symbol.Semicolon);
                AnalyzeRecordSection(followers);
            }
        }

        private void AnalyzeRecordSection(HashSet<Symbol> followers)
        {
            var record = _currentRecord;

            if (CurrentSymbol == Symbol.Identifier)
            {
                AcceptTerminal(Symbol.Identifier);
                if (AcceptedToken != null && record.Record.Fields.Count(f => f.Identifier == AcceptedToken.TextValue) > 0)
                {
                    IoManager.InsertError(AcceptedToken.CharacterNumber,101);
                }
                if (AcceptedToken != null)
                {
                    record.Record.Fields.Add(new Variable() { Identifier = AcceptedToken.TextValue });
                }

                while (CurrentSymbol == Symbol.Comma)
                {
                    AcceptTerminal(Symbol.Comma);
                    AcceptTerminal(Symbol.Identifier);
                    if (AcceptedToken != null && record.Record.Fields.Count(f => f.Identifier == AcceptedToken.TextValue) > 0)
                    {
                        IoManager.InsertError(AcceptedToken.CharacterNumber, 101);
                    }
                    if (AcceptedToken != null)
                    {
                        record.Record.Fields.Add(new Variable() { Identifier = AcceptedToken.TextValue });
                    }
                }
                AcceptTerminal(Symbol.Colon);
                AnalyzeType(followers);
                foreach (var variable in record.Record.Fields)
                {
                    variable.Type = _currentType;
                }
            }
        }

        private void AnalyzeSetType(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.Set);
            AcceptTerminal(Symbol.Of);
            AnalyzeType(followers);
        }

        private void AnalyzeFileType(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.File);
            AcceptTerminal(Symbol.Of);
            AnalyzeType(followers);
        }

        private void AnalyzeReferenceType(HashSet<Symbol> followers)
        {
            AcceptTerminal(Symbol.Caret);
            AcceptTerminal(Symbol.Identifier);

            if (!Belongs(followers))
            {
                SkipTo(followers);
            }
        }
    }
}
