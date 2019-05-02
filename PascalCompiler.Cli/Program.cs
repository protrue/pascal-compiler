using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;

namespace PascalCompiler.Cli
{
    public class Program
    {
        public static string InputFilePath = ConfigurationManager.AppSettings["InputFilePath"];
        public static string OutputFilePath = ConfigurationManager.AppSettings["OutputFilePath"];

        public static void TestIoManager()
        {
            var compiler = new Compiler(InputFilePath, OutputFilePath);

            Console.WriteLine("IoManager test");

            var letters = new List<char>();
            while (!compiler.IoManager.IsEndOfFile)
            {
                letters.Add(compiler.IoManager.GetNextCharacter());
            }

            compiler.Dispose();

            Console.WriteLine(string.Join("-", letters.Select(l => $"[{l}]")));
        }

        public static void TestTokenizer()
        {
            var compiler = new Compiler(InputFilePath, OutputFilePath);

            Console.WriteLine("Tokenizer test");

            var tokens = new List<Token>();

            while (!compiler.Tokenizer.IsEndOfTokens)
            {
                tokens.Add(compiler.Tokenizer.GetNextToken());
            }

            compiler.Dispose();

            var tokensString = string.Join(Environment.NewLine, tokens);
            var namesString = string.Join(Environment.NewLine, compiler.Tokenizer.Names);
            Console.WriteLine($"{tokensString}{Environment.NewLine}{namesString}");
        }

        public static void TestAnalyzer()
        {
            var compiler = new Compiler(InputFilePath, OutputFilePath);

            Console.WriteLine("Analyzer test");

            compiler.Analyzer.Analyze();

            compiler.Dispose();

            Console.WriteLine(File.ReadAllText(OutputFilePath));
        }

        public static void Main(string[] args)
        {
            TestIoManager();
            TestTokenizer();
            TestAnalyzer();
        }
    }
}
