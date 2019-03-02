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

            var inputOutputModule = new InputOutputModule(DefaultInputFilePath, DefaultOutputFilePath);
            var scanner = new Scanner(inputOutputModule);

            var tokens = new List<Token>();
            while (!scanner.IsEndOfTokens)
                 tokens.Add(scanner.GetNextToken());

            inputOutputModule.OutputStream.WriteLine($"{Environment.NewLine}Tokens:");

            foreach (var token in tokens)
                inputOutputModule.OutputStream.WriteLine(token);

            inputOutputModule.Dispose();

            Console.WriteLine(File.ReadAllText(DefaultOutputFilePath));
            Console.ReadKey();
        }
    }
}
