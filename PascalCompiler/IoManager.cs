using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PascalCompiler
{
    public class IoManager : IDisposable
    {
        private const int IndentLength = 7;

        public StreamReader InputStream { get; }
        public StreamWriter OutputStream { get; }

        public int CurrentLineNumber { get; private set; }
        public int CurrentCharacterNumber { get; private set; }

        public string CurrentLine { get; private set; }
        public char CurrentCharacter { get; private set; }

        public List<CompilationError> CompilationErrors { get; }

        public bool IsEndOfFile =>
            InputStream.EndOfStream
            && CurrentCharacter == '\n';

        public IoManager(Stream inputStream, Stream outputStream)
        {
            InputStream = new StreamReader(inputStream);
            OutputStream = new StreamWriter(outputStream);

            CompilationErrors = new List<CompilationError>();

            CurrentLineNumber = -1;
            CurrentCharacterNumber = -1;

            CurrentLine = string.Empty;
        }

        public IoManager(string inputFileName, string outputFileName)
            : this(
                new FileStream(inputFileName, FileMode.OpenOrCreate, FileAccess.Read),
                new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
        {

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
            var indent = CreateIndentWithNumber(IndentLength, CurrentLineNumber + 1);
            var outputLine = $"{indent} {CurrentLine.Replace("\t", " ")}";

            OutputStream.WriteLine(outputLine);
        }

        public char GetNextCharacter()
        {
            if (CurrentCharacterNumber == CurrentLine.Length - 1)
            {
                CurrentCharacter = '\n';
                CurrentCharacterNumber++;
                return CurrentCharacter;
            }

            if (CurrentCharacterNumber > CurrentLine.Length - 1 || CurrentLineNumber == -1)
            {
                if (IsEndOfFile)
                    throw new EndOfStreamException("Невозможно получить следующую литеру: достигнут конец файла");

                do CurrentLine = InputStream.ReadLine();
                while (CurrentLine.Length == 0);
                CurrentLineNumber++;
                CurrentCharacterNumber = -1;
                WriteSourceCodeLine();
            }

            CurrentCharacter = CurrentLine[++CurrentCharacterNumber];

            return CurrentCharacter;
        }

        public void InsertError(int characterNumber, int errorCode) =>
            InsertError(new CompilationError(CurrentLineNumber, characterNumber, errorCode));

        public void InsertError(CompilationError compilationError)
        {
            CompilationErrors.Add(compilationError);

            if (CompilationErrors.Count >= Constants.MaximumErrorsCount)
                return;

            var offset = CreateIndent(compilationError.CharacterNumber);

            var indentWithNumber = CreateIndentWithNumber(IndentLength,
                                       CompilationErrors.Count, '*') + offset;
            var asteriskIndent = CreateIndent(IndentLength, '*') + offset;

            OutputStream.WriteLine(
                $"{indentWithNumber} ^ cтрока: {compilationError.LineNumber + 1}, " +
                $"символ: {compilationError.CharacterNumber + 1}, " +
                $"код ошибки: {compilationError.ErrorCode}");
            OutputStream.WriteLine(
                $"{asteriskIndent} " +
                $"{Constants.ErrorsMap[compilationError.ErrorCode]}");
        }

        public void Dispose()
        {
            InputStream?.Dispose();
            OutputStream?.Dispose();
        }
    }
}
