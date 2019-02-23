﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler
{
    public class InputOutputModule : IDisposable
    {
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

        public char GetNextCharacter()
        {
            if (CurrentCharacterNumber >= CurrentLine.Length - 1 || CurrentLineNumber == -1)
            {
                if (IsEndOfFile)
                    throw new EndOfStreamException("Невозможно получить следующую литеру: достигнут конец файла");

                CurrentLine = InputStream.ReadLine();
                OutputStream.WriteLine(CurrentLine);
                CurrentLineNumber++;
                CurrentCharacterNumber = -1;
            }

            CurrentCharacter = CurrentLine[++CurrentCharacterNumber];

            return CurrentCharacter;
        }

        public void InsertError(int lineNumber, int characterNumber, int errorCode) =>
            InsertError(new CompilationError(lineNumber, characterNumber, errorCode));

        public void InsertError(CompilationError compilationError)
        {
            var indent = new string(Enumerable.Repeat(' ', compilationError.CharacterNumber).ToArray());

            CompilationErrors.Add(compilationError);
            OutputStream.WriteLine(
                $"{indent}^ {compilationError.ErrorCode}: " +
                $"{Constants.CompilationsErrorMessages[compilationError.ErrorCode]}");
        }

        public void Dispose()
        {
            InputStream?.Dispose();
            OutputStream?.Dispose();
        }
    }
}
