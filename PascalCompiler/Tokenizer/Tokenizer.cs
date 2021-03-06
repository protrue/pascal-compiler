﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace PascalCompiler.Tokenizer
{
    public class Tokenizer : IDisposable
    {
        public IoManager.IoManager IoManager { get; set; }

        public bool IsEndOfTokens => IoManager.IsEndOfFile && _storedCharacters.Count == 0;

        public Token CurrentToken { get; private set; }

        public HashSet<string> Names { get; private set; }

        private Queue<string> _storedCharacters;
        private Token _storedToken;

        public Tokenizer(IoManager.IoManager ioManager)
        {
            IoManager = ioManager;

            Names = new HashSet<string>();

            _storedCharacters = new Queue<string>();
            _storedToken = new Token();
        }

        private Token GetTokenWithCurrentPosition() => new Token
        {
            LineNumber = IoManager.CurrentLineNumber,
            CharacterNumber = IoManager.CurrentCharacterNumber,
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
                    currentCharacter = IoManager.GetNextCharacter().ToString();
                    CurrentToken = GetTokenWithCurrentPosition();
                }
            } while (!IoManager.IsEndOfFile && string.IsNullOrWhiteSpace(currentCharacter));

            return currentCharacter;
        }

        private void ScanTypographicalSymbol(string currentCharacter)
        {
            if (!Constants.Constants.PrefixSymbols.Contains(currentCharacter))
                CurrentToken.Symbol = Constants.Constants.StringSymbolMap[currentCharacter];
            else if (IoManager.IsEndOfFile)
                CurrentToken.Symbol = Constants.Constants.StringSymbolMap[currentCharacter];
            else
            {
                var nextCharacter = IoManager.GetNextCharacter().ToString();
                var symbol = currentCharacter + nextCharacter;

                if (Constants.Constants.StringSymbolMap.ContainsKey(symbol))
                    CurrentToken.Symbol = Constants.Constants.StringSymbolMap[symbol];
                else
                {
                    _storedCharacters.Enqueue(nextCharacter);
                    _storedToken = GetTokenWithCurrentPosition();
                    CurrentToken.Symbol = Constants.Constants.StringSymbolMap[currentCharacter];
                }
            }
        }

        private string ScanSymbol(string currentCharacter, Func<char, bool> appendPredicate)
        {
            var symbolBuilder = new StringBuilder();
            symbolBuilder.Append(currentCharacter);

            CurrentToken = GetTokenWithCurrentPosition();

            var nextCharacter = ' ';
            while (!IoManager.IsEndOfFile)
            {
                nextCharacter = IoManager.GetNextCharacter();

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
            var gotDot = false;
            var isExponential = false;
            var lastChar = currentCharacter[0];
            var symbol = ScanSymbol(currentCharacter, c =>
            {
                var result =
                    char.IsDigit(c) && (lastChar != 'E' || lastChar != 'e')
                    || c == '.' && !gotDot && !isExponential
                    || char.IsDigit(lastChar) && (c == 'E' || c == 'e') && !isExponential
                    || isExponential && (c == '+' || c == '-');
                if (c == '.') gotDot = true;
                if (c == 'E' || c == 'e') isExponential = true;
                lastChar = c;
                return result;
            });

            var isParsed = double.TryParse(symbol, NumberStyles.Float,
                NumberFormatInfo.InvariantInfo, out var numericConstantValue);

            if (!isParsed)
                IoManager.InsertError(CurrentToken.CharacterNumber, 50);

            CurrentToken.NumericValue = numericConstantValue;

            CurrentToken.Symbol =
                    gotDot || isExponential
                    ? Constants.Constants.Symbol.FloatConstant
                    : Constants.Constants.Symbol.IntegerConstant;

            if (CurrentToken.Symbol == Constants.Constants.Symbol.IntegerConstant && CurrentToken.NumericValue > Constants.Constants.MaximumIntegerValue)
                IoManager.InsertError(CurrentToken.CharacterNumber, 203);

            if (CurrentToken.Symbol == Constants.Constants.Symbol.FloatConstant && CurrentToken.NumericValue * Constants.Constants.MaximumFloatValue < 1)
                IoManager.InsertError(CurrentToken.CharacterNumber, 206);

            if (CurrentToken.Symbol == Constants.Constants.Symbol.FloatConstant && CurrentToken.NumericValue > Constants.Constants.MaximumFloatValue)
                IoManager.InsertError(CurrentToken.CharacterNumber, 207);
        }

        private void ScanKeywordOrIdentifier(string currentCharacter)
        {
            var symbol = ScanSymbol(currentCharacter, c => char.IsLetterOrDigit(c) || c == '_');

            if (Constants.Constants.StringSymbolMap.ContainsKey(symbol.ToLower()))
            {
                CurrentToken.Symbol = Constants.Constants.StringSymbolMap[symbol.ToLower()];
            }
            else
            {
                CurrentToken.Symbol = Constants.Constants.Symbol.Identifier;
                CurrentToken.TextValue = symbol;
                Names.Add(symbol);
            }
        }

        private bool ScanStringOrCharConstant()
        {
            var textConstantBuilder = new StringBuilder();

            var nextChar = ' ';
            while (!IoManager.IsEndOfFile)
            {
                nextChar = IoManager.GetNextCharacter();
                if (nextChar == '\'' || nextChar == '\n') break;
                textConstantBuilder.Append(nextChar);
            }

            if (nextChar != '\'')
            {
                IoManager.InsertError(CurrentToken.CharacterNumber, 75);
                return false;
            }

            if (textConstantBuilder.Length > 0)
            {
                CurrentToken.TextValue = textConstantBuilder.ToString();
                if (textConstantBuilder.Length == 1) CurrentToken.Symbol = Constants.Constants.Symbol.CharConstant;
                else if (textConstantBuilder.Length > 1) CurrentToken.Symbol = Constants.Constants.Symbol.StringConstant;

                if (textConstantBuilder.Length > Constants.Constants.MaximumStringLength)
                    IoManager.InsertError(CurrentToken.CharacterNumber, 76);
            }
            else
            {
                IoManager.InsertError(CurrentToken.CharacterNumber, 75);
                return false;
            }

            return true;
        }

        private void SkipDoubleSlashComment()
        {
            _storedCharacters.Clear();
            var nextCharacter = ' ';
            while (!IoManager.IsEndOfFile && nextCharacter != '\n')
                nextCharacter = IoManager.GetNextCharacter();
        }

        private void SkipAsteriskParenthesisComment()
        {
            try
            {
                while (true)
                {
                    if (IoManager.GetNextCharacter() != '*') continue;
                    if (IoManager.GetNextCharacter() == ')')
                        break;
                }
            }
            catch (EndOfStreamException endOfStreamException)
            {
                IoManager.InsertError(IoManager.CurrentCharacterNumber,
                    86);
            }
        }

        private void SkipCurlyBracketComment()
        {
            try
            {
                while (IoManager.GetNextCharacter() != '}') ;
            }
            catch (EndOfStreamException endOfStreamException)
            {
                IoManager.InsertError(IoManager.CurrentCharacterNumber,
                    86);
            }
        }

        public Token GetNextToken()
        {
            if (IsEndOfTokens) return null;

            var currentCharacter = GetCurrentCharacter();

            if (string.IsNullOrWhiteSpace(currentCharacter)) return GetNextToken();

            if (currentCharacter == "'")
            {
                var isSuccess = ScanStringOrCharConstant();
                if (!isSuccess) return GetNextToken();
            }
            else if (char.IsDigit(currentCharacter, 0))
                ScanNumericConstant(currentCharacter);
            else if (char.IsLetter(currentCharacter, 0))
                ScanKeywordOrIdentifier(currentCharacter);
            else if (Constants.Constants.TypographicalSymbols.Contains(currentCharacter))
            {
                ScanTypographicalSymbol(currentCharacter);

                switch (CurrentToken.Symbol)
                {
                    case Constants.Constants.Symbol.Comment:
                        SkipDoubleSlashComment();
                        return GetNextToken();
                    case Constants.Constants.Symbol.LeftComment:
                        SkipAsteriskParenthesisComment();
                        return GetNextToken();
                    case Constants.Constants.Symbol.LeftCurlyBracket:
                        SkipCurlyBracketComment();
                        return GetNextToken();
                }
            }
            else
            {
                IoManager.InsertError(IoManager.CurrentCharacterNumber, 6);
                return GetNextToken();
            }

            return CurrentToken;
        }

        public void Dispose()
        {
            IoManager.Dispose();
        }
    }
}
