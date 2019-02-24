using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
    public class InputOutputModule : IDisposable
    {
        private const int IndentLength = 7;
        public const int MaximumErrorsCount = 99;

        public StreamReader InputStream { get; }
        public StreamWriter OutputStream { get; }

        public int CurrentLineNumber { get; private set; }
        public int CurrentCharacterNumber { get; private set; }

        public string CurrentLine { get; private set; }
        public char CurrentCharacter { get; private set; }

        public List<CompilationError> CompilationErrors { get; }

        public bool IsEndOfFile =>
            InputStream.EndOfStream
            && CurrentCharacterNumber >= CurrentLine.Length - 1;

        public InputOutputModule(string inputFileName, string outputFileName)
        {
            InputStream = new StreamReader(inputFileName);
            OutputStream = new StreamWriter(outputFileName);

            CompilationErrors = new List<CompilationError>();

            CurrentLineNumber = -1;
            CurrentCharacterNumber = -1;

            CurrentLine = string.Empty;
        }

        private string CreateIndent(int length, char filler = ' ') =>
            new string(Enumerable.Repeat(filler, length).ToArray());

        private string CreateIndentWithNumber(int length, int number, char filler = ' ')
        {
            var numberAsString = number.ToString();
            var subIndentLength = (length - numberAsString.Length) / 2.0;
            var leftSubIndent = new string(Enumerable.Repeat(filler, (int)Math.Floor(subIndentLength)).ToArray());
            var rightSubIndent = new string(Enumerable.Repeat(filler, (int)Math.Ceiling(subIndentLength)).ToArray());
            var indent = $"{leftSubIndent}{numberAsString}{rightSubIndent}";

            return indent;
        }

        private void WriteSourceCodeLine()
        {
            var indent = CreateIndentWithNumber(IndentLength, CurrentLineNumber);
            var outputLine = $"{indent} {CurrentLine}";

            OutputStream.WriteLine(outputLine);
        }

        public char GetNextCharacter()
        {
            if (CurrentCharacterNumber >= CurrentLine.Length - 1 || CurrentLineNumber == -1)
            {
                if (IsEndOfFile)
                    throw new EndOfStreamException(
                        "Невозможно получить следующую литеру: достигнут конец файла");

                CurrentLine = InputStream.ReadLine();
                CurrentLineNumber++;
                CurrentCharacterNumber = -1;
                WriteSourceCodeLine();
            }

            CurrentCharacter = CurrentLine[++CurrentCharacterNumber];

            return CurrentCharacter;
        }

        public void InsertError(int lineNumber, int characterNumber, int errorCode) =>
            InsertError(new CompilationError(lineNumber, characterNumber, errorCode));

        public void InsertError(CompilationError compilationError)
        {
            CompilationErrors.Add(compilationError);

            if (CompilationErrors.Count >= MaximumErrorsCount)
                return;

            var offset = CreateIndent(CurrentCharacterNumber);

            var indentWithNumber = CreateIndentWithNumber(IndentLength,
                                       CompilationErrors.Count, '*') + offset;
            var asteriskIndent = CreateIndent(IndentLength, '*') + offset;

            OutputStream.WriteLine(
                $"{indentWithNumber} ^ cтрока: {compilationError.LineNumber + 1}, " +
                $"символ: {compilationError.CharacterNumber + 1}, " +
                $"код ошибки: {compilationError.ErrorCode}");
            OutputStream.WriteLine(
                $"{asteriskIndent} " +
                $"{Constants.CompilationsErrorMessages[compilationError.ErrorCode]}");
        }

        public void Dispose()
        {
            InputStream?.Dispose();
            OutputStream?.Dispose();
        }
    }
}
