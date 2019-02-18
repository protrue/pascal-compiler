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
        public StreamReader InputStream { get; private set; }
        public StreamWriter OutputStream { get; private set; }

        public int CurrentLineNumber { get; private set; }
        public int CurrentCharacterNumber { get; private set; }

        public string CurrentLine { get; private set; }
        public string CurrentCharacter { get; private set; }

        public InputOutputModule(string inputFileName, string outputFileName)
        {
            InputStream = new StreamReader(inputFileName);
            OutputStream = new StreamWriter(outputFileName);
            CurrentLineNumber = 0;
            CurrentCharacterNumber = 0;
        }

        public char GetNextCharacter()
        {
            throw new NotImplementedException();
        }

        public void InsertError(int line, int column)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            InputStream?.Dispose();
            OutputStream?.Dispose();
        }
    }
}
