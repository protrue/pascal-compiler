using System;
using System.Collections.Generic;
using System.Text;

namespace PascalCompiler
{
    public class Analyzer : IDisposable
    {
        public IoManager IoManager { get; private set; }
        public Tokenizer Tokenizer { get; private set; }

        public Analyzer(IoManager ioManager, Tokenizer tokenizer)
        {
            IoManager = ioManager;
            Tokenizer = tokenizer;
        }

        public void Analyze()
        {

        }

        public void Dispose()
        {
            Tokenizer.Dispose();
        }
    }
}
