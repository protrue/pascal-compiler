using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace PascalCompiler.Cli
{
    public class Program
    {
        public static string DefaultInputFilePath = ConfigurationManager.AppSettings["DefaultInputFilePath"];
        public static string DefaultOutputFilePath = ConfigurationManager.AppSettings["DefaultOutputFilePath"];

        public static void Main(string[] args)
        {
            if (!File.Exists(DefaultInputFilePath))
                File.Create(DefaultInputFilePath);

            var ioManager = new IoManager(DefaultInputFilePath, DefaultOutputFilePath);
            var tokenizer = new Tokenizer(ioManager);

            var tokens = new List<Token>();
            while (!tokenizer.IsEndOfTokens)
                 tokens.Add(tokenizer.GetNextToken());

            ioManager.OutputStream.WriteLine($"{Environment.NewLine}Tokens:");

            foreach (var token in tokens)
                ioManager.OutputStream.WriteLine(token);

            ioManager.Dispose();

            Console.WriteLine(File.ReadAllText(DefaultOutputFilePath));
            Console.ReadKey();
        }
    }
}
