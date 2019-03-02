using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PascalCompiler
{
    public class Scanner
    {
        public InputOutputModule InputOutputModule { get; set; }

        public bool IsEndOfTokens => InputOutputModule.IsEndOfFile && _storedCharacters.Count == 0;

        public Token CurrentToken { get; private set; }

        private Queue<string> _storedCharacters;
        private Token _storedToken;

        public Scanner(InputOutputModule inputOutputModule)
        {
            InputOutputModule = inputOutputModule;
            _storedCharacters = new Queue<string>();
            _storedToken = new Token();
        }

        private Token GetTokenWithCurrentPosition() => new Token
        {
            LineNumber = InputOutputModule.CurrentLineNumber,
            CharacterNumber = InputOutputModule.CurrentCharacterNumber,
        };

        private string GetCurrentCharacter()
        {
            string currentCharacter;

            do
            {
                if (_storedCharacters.Count != 0)
                {
                    currentCharacter = _storedCharacters.Dequeue();
                    CurrentToken = _storedToken;
                }
                else
                {
                    currentCharacter = InputOutputModule.GetNextCharacter().ToString();
                    CurrentToken = GetTokenWithCurrentPosition();
                }
            } while (!InputOutputModule.IsEndOfFile && string.IsNullOrWhiteSpace(currentCharacter));

            return currentCharacter;
        }

        private void ScanTypographicalSymbol(string currentCharacter)
        {
            if (!Constants.PrefixSymbols.Contains(currentCharacter))
                CurrentToken.Symbol = Constants.StringSymbolMap[currentCharacter];
            else if (InputOutputModule.IsEndOfFile)
                CurrentToken.Symbol = Constants.StringSymbolMap[currentCharacter];
            else
            {
                var nextCharacter = InputOutputModule.GetNextCharacter().ToString();
                var symbol = currentCharacter + nextCharacter;

                if (Constants.StringSymbolMap.ContainsKey(symbol))
                    CurrentToken.Symbol = Constants.StringSymbolMap[symbol];
                else
                {
                    _storedCharacters.Enqueue(nextCharacter);
                    _storedToken = GetTokenWithCurrentPosition();
                    CurrentToken.Symbol = Constants.StringSymbolMap[currentCharacter];
                }
            }
        }

        private string ScanSymbol(string currentCharacter, Func<char, bool> appendPredicate)
        {
            var symbolBuilder = new StringBuilder();
            symbolBuilder.Append(currentCharacter);

            CurrentToken = GetTokenWithCurrentPosition();

            var nextCharacter = ' ';
            while (!InputOutputModule.IsEndOfFile)
            {
                nextCharacter = InputOutputModule.GetNextCharacter();

                if (appendPredicate(nextCharacter))
                    symbolBuilder.Append(nextCharacter);
                else break;
            }

            if (nextCharacter != ' ' && !appendPredicate(nextCharacter))
            {
                _storedCharacters.Enqueue(nextCharacter.ToString());
                _storedToken = GetTokenWithCurrentPosition();
            }

            return symbolBuilder.ToString();
        }

        private void ScanNumericConstant(string currentCharacter)
        {
            var onlyOneDot = true;
            var symbol = ScanSymbol(currentCharacter, c =>
            {
                var result = char.IsDigit(c) || c == '.' && onlyOneDot;
                if (c == '.') onlyOneDot = false;
                return result;
            });

            var isParsed = double.TryParse(symbol, NumberStyles.AllowDecimalPoint,
                NumberFormatInfo.InvariantInfo, out var numericConstantValue);

            CurrentToken.NumericValue = numericConstantValue;

            CurrentToken.Symbol =
                symbol.Contains('.')
                    ? Constants.Symbol.FloatConstant
                    : Constants.Symbol.IntegerConstant;

            if (CurrentToken.Symbol == Constants.Symbol.IntegerConstant && CurrentToken.NumericValue > Constants.MaximumIntegerValue)
                InputOutputModule.InsertError(CurrentToken.LineNumber, CurrentToken.CharacterNumber, 97);
        }

        private void ScanKeywordOrIdentifier(string currentCharacter)
        {
            var symbol = ScanSymbol(currentCharacter, c => char.IsLetterOrDigit(c) || c == '_');

            if (Constants.StringSymbolMap.ContainsKey(symbol.ToLower()))
                CurrentToken.Symbol = Constants.StringSymbolMap[symbol.ToLower()];
            else
            {
                CurrentToken.Symbol = Constants.Symbol.Identifier;
                CurrentToken.TextValue = symbol;
            }
        }

        private bool ScanStringOrCharConstant()
        {
            var textConstantBuilder = new StringBuilder();

            var nextChar = ' ';
            while (!InputOutputModule.IsEndOfFile)
            {
                nextChar = InputOutputModule.GetNextCharacter();
                if (nextChar == '\'') break;
                textConstantBuilder.Append(nextChar);
            }

            if (nextChar != '\'')
            {
                InputOutputModule.InsertError(CurrentToken.LineNumber, CurrentToken.CharacterNumber, 30);
                return false;
            }

            if (textConstantBuilder.Length > 0)
            {
                CurrentToken.TextValue = textConstantBuilder.ToString();
                if (textConstantBuilder.Length == 1) CurrentToken.Symbol = Constants.Symbol.CharConstant;
                else if (textConstantBuilder.Length > 1) CurrentToken.Symbol = Constants.Symbol.StringConstant;
            }
            else
            {
                InputOutputModule.InsertError(CurrentToken.LineNumber, CurrentToken.CharacterNumber, 30);
                return false;
            }

            return true;
        }

        private void SkipDoubleSlashComment()
        {
            _storedCharacters.Clear();
            var storedCharacter = ' ';
            while (!InputOutputModule.IsEndOfFile &&
                   InputOutputModule.CurrentLineNumber == CurrentToken.LineNumber)
                storedCharacter = InputOutputModule.GetNextCharacter();
            if (!InputOutputModule.IsEndOfFile)
                _storedCharacters.Enqueue(storedCharacter.ToString());
        }

        private void SkipAsteriskParenthesisComment()
        {
            try
            {
                while (true)
                {
                    if (InputOutputModule.GetNextCharacter() != '*') continue;
                    if (InputOutputModule.GetNextCharacter() == ')')
                        break;
                }
            }
            catch (EndOfStreamException endOfStreamException)
            {
                InputOutputModule.InsertError(InputOutputModule.CurrentLineNumber, InputOutputModule.CurrentCharacterNumber,
                    32);
            }
        }

        private void SkipCurlyBracketComment()
        {
            try
            {
                while (InputOutputModule.GetNextCharacter() != '}') ;
            }
            catch (EndOfStreamException endOfStreamException)
            {
                InputOutputModule.InsertError(InputOutputModule.CurrentLineNumber, InputOutputModule.CurrentCharacterNumber,
                    32);
            }
        }

        public Token GetNextToken()
        {
            if (IsEndOfTokens) return null;

            var currentCharacter = GetCurrentCharacter();

            if (currentCharacter == "'")
            {
                var isSuccess = ScanStringOrCharConstant();
                if (!isSuccess) return GetNextToken();
            }
            else if (char.IsDigit(currentCharacter, 0))
                ScanNumericConstant(currentCharacter);
            else if (char.IsLetter(currentCharacter, 0))
                ScanKeywordOrIdentifier(currentCharacter);
            else if (Constants.TypographicalSymbols.Contains(currentCharacter))
            {
                ScanTypographicalSymbol(currentCharacter);

                switch (CurrentToken.Symbol)
                {
                    case Constants.Symbol.Comment:
                        SkipDoubleSlashComment();
                        return GetNextToken();
                    case Constants.Symbol.LeftComment:
                        SkipAsteriskParenthesisComment();
                        return GetNextToken();
                    case Constants.Symbol.LeftCurlyBracket:
                        SkipCurlyBracketComment();
                        return GetNextToken();
                }
            }
            else
            {
                InputOutputModule.InsertError(InputOutputModule.CurrentLineNumber,
                    InputOutputModule.CurrentCharacterNumber, 5);
                return GetNextToken();
            }

            return CurrentToken;
        }
    }
}
