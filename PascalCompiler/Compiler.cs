using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalCompiler
{
    public class Compiler : IDisposable
    {
        public IoManager IoManager { get; set; }
        public Tokenizer Tokenizer { get; set; }
        public Analyzer Analyzer { get; set; }

        private Compiler()
        {
            Tokenizer = new Tokenizer(IoManager);
            Analyzer = new Analyzer(IoManager, Tokenizer);
        }

        public Compiler(Stream inputStream, Stream outputStream)
        {
            IoManager = new IoManager(inputStream, outputStream);
            Tokenizer = new Tokenizer(IoManager);
            Analyzer = new Analyzer(IoManager, Tokenizer);
        }

        public Compiler(string inputPath, string outputPath)
        {
            IoManager = new IoManager(inputPath, outputPath);
            Tokenizer = new Tokenizer(IoManager);
            Analyzer = new Analyzer(IoManager, Tokenizer);
        }

        public void Dispose()
        {
            Analyzer.Dispose();
        }
    }
}
