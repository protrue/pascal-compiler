using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalCompiler
{
    public class Compiler : IDisposable
    {
        public IoManager.IoManager IoManager { get; set; }
        public Tokenizer.Tokenizer Tokenizer { get; set; }
        public Analyzer.Analyzer Analyzer { get; set; }

        private Compiler()
        {
            Tokenizer = new Tokenizer.Tokenizer(IoManager);
            Analyzer = new Analyzer.Analyzer(IoManager, Tokenizer);
        }

        public Compiler(Stream inputStream, Stream outputStream)
        {
            IoManager = new IoManager.IoManager(inputStream, outputStream);
            Tokenizer = new Tokenizer.Tokenizer(IoManager);
            Analyzer = new Analyzer.Analyzer(IoManager, Tokenizer);
        }

        public Compiler(string inputPath, string outputPath)
        {
            IoManager = new IoManager.IoManager(inputPath, outputPath);
            Tokenizer = new Tokenizer.Tokenizer(IoManager);
            Analyzer = new Analyzer.Analyzer(IoManager, Tokenizer);
        }

        public void Dispose()
        {
            Analyzer.Dispose();
        }
    }
}
