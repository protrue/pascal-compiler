using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PascalCompiler
{
    public class Compiler
    {
        public IoManager InputOutputModule { get; set; }
        public Tokenizer Tokenizer { get; set; }
        public Parser Parser { get; set; }

        public Compiler(Stream inputStream, Stream outputStream)
        {
            InputOutputModule = new IoManager(inputStream, outputStream);
        }
    }
}
