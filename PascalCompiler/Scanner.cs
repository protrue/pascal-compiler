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

        public void ScanNumericConstant(string currentCharacter)
        {
            var symbolBuilder = new StringBuilder();
            symbolBuilder.Append(currentCharacter);

            CurrentToken = GetTokenWithCurrentPosition();

            while (true)
            {
                var nextCharacter = ' ';
                if (!InputOutputModule.IsEndOfFile)
                    nextCharacter = InputOutputModule.GetNextCharacter();

                if (char.IsDigit(nextCharacter) || nextCharacter == '.')
                    symbolBuilder.Append(nextCharacter);
                else
                {
                    var symbol = symbolBuilder.ToString();

                    if (symbol.Last() == '.')
                    {
                        _storedCharacters.Enqueue(symbol.Last().ToString());
                        symbol = symbol.Remove(symbol.Length - 1, 1);
                        _storedToken = new Token()
                        {
                            CharacterNumber = InputOutputModule.CurrentCharacterNumber - 1,
                            LineNumber = InputOutputModule.CurrentLineNumber - 1
                        };
                    }
                    else
                        _storedToken = GetTokenWithCurrentPosition();

                    if (nextCharacter != ' ')
                        _storedCharacters.Enqueue(nextCharacter.ToString());

                    var isParsed = double.TryParse(symbol, NumberStyles.AllowDecimalPoint,
                        NumberFormatInfo.InvariantInfo, out var numericConstantValue);

                    CurrentToken.NumericValue = numericConstantValue;

                    CurrentToken.Symbol =
                        symbol.Contains('.')
                        ? Constants.Symbol.FloatConstant
                        : Constants.Symbol.IntegerConstant;

                    if (CurrentToken.Symbol == Constants.Symbol.IntegerConstant && CurrentToken.NumericValue > Constants.MaximumIntegerValue)
                        InputOutputModule.InsertError(CurrentToken.LineNumber, CurrentToken.CharacterNumber, 97);

                    break;
                }
            }
        }

        public void ScanKeywordOrIdentifier(string currentCharacter)
        {
            var symbolBuilder = new StringBuilder();
            symbolBuilder.Append(currentCharacter);

            CurrentToken = GetTokenWithCurrentPosition();

            while (true)
            {
                var nextCharacter = ' ';
                if (!InputOutputModule.IsEndOfFile)
                    nextCharacter = InputOutputModule.GetNextCharacter();

                if (char.IsLetterOrDigit(nextCharacter) || nextCharacter == '_')
                    symbolBuilder.Append(nextCharacter);
                else
                {
                    if (nextCharacter != ' ')
                    {
                        _storedCharacters.Enqueue(nextCharacter.ToString());
                        _storedToken = GetTokenWithCurrentPosition();
                    }
                    
                    var symbol = symbolBuilder.ToString();

                    if (Constants.StringSymbolMap.ContainsKey(symbol.ToLower()))
                        CurrentToken.Symbol = Constants.StringSymbolMap[symbol];
                    else
                    {
                        CurrentToken.Symbol = Constants.Symbol.Identifier;
                        CurrentToken.TextValue = symbol;
                    }

                    break;
                }
            }
        }

        public bool ScanStringOrCharConstant()
        {
            var isSuccess = true;

            var nextChar = ' ';
            var textConstantBuilder = new StringBuilder();
            while (!InputOutputModule.IsEndOfFile)
            {
                nextChar = InputOutputModule.GetNextCharacter();
                if (nextChar == '\'')
                    break;
                textConstantBuilder.Append(nextChar);
            }

            if (textConstantBuilder.Length > 0)
            {
                CurrentToken.TextValue = textConstantBuilder.ToString();
                if (textConstantBuilder.Length == 1)
                    CurrentToken.Symbol = Constants.Symbol.CharConstant;
                else if (textConstantBuilder.Length > 1)
                    CurrentToken.Symbol = Constants.Symbol.StringConstant;
            }
            else
            {
                InputOutputModule.InsertError(CurrentToken.LineNumber, CurrentToken.CharacterNumber, 30);
                isSuccess = false;
            }

            return isSuccess;
        }

        private void SkipComment()
        {
            _storedCharacters.Clear();
            var storedCharacter = ' ';
            while (!InputOutputModule.IsEndOfFile &&
                   InputOutputModule.CurrentLineNumber == CurrentToken.LineNumber)
                storedCharacter = InputOutputModule.GetNextCharacter();
            if (!InputOutputModule.IsEndOfFile)
                _storedCharacters.Enqueue(storedCharacter.ToString());
        }

        public Token GetNextToken()
        {
            if (IsEndOfTokens)
                throw new EndOfStreamException(
                    "Невозможно получить следующий символ: достигнут конец файла");

            var currentCharacter = GetCurrentCharacter();

            if (Constants.TypographicalSymbols.Contains(currentCharacter))
            {
                ScanTypographicalSymbol(currentCharacter);

                if (CurrentToken.Symbol == Constants.Symbol.Comment)
                {
                    SkipComment();
                    if (!IsEndOfTokens) return GetNextToken();
                }

                if (CurrentToken.Symbol == Constants.Symbol.LeftComment)
                {
                    while (GetNextToken().Symbol != Constants.Symbol.RightComment) ;
                    if (!IsEndOfTokens) return GetNextToken();
                }

                if (CurrentToken.Symbol == Constants.Symbol.Quote)
                {
                    var isSuccess = ScanStringOrCharConstant();
                    if (!isSuccess && !IsEndOfTokens) return GetNextToken();
                }
            }
            else
            if (char.IsDigit(currentCharacter, 0))
                ScanNumericConstant(currentCharacter);
            else
            if (char.IsLetter(currentCharacter, 0))
                ScanKeywordOrIdentifier(currentCharacter);
            else
            {
                InputOutputModule.InsertError(InputOutputModule.CurrentLineNumber,
                    InputOutputModule.CurrentCharacterNumber, 5);
                if (!IsEndOfTokens)
                    return GetNextToken();
            }

            return CurrentToken;
        }
    }
}
