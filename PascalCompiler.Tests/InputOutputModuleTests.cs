using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PascalCompiler.Tests
{
    [TestClass]
    public class InputOutputModuleTests
    {
        [TestMethod]
        public void GetCharacterTest()
        {
            const string inputFileName = "input.txt";
            const string outputFileName = "output.txt";
            var inputText = string.Join(Environment.NewLine, new[] { "ab1", "qwe2", "asdf4" });
            File.WriteAllText(inputFileName, inputText);

            var inputOutputModule = new IoManager(inputFileName, outputFileName);
            var stringBuilder = new StringBuilder();

            while (!inputOutputModule.IsEndOfFile)
                stringBuilder.Append(inputOutputModule.GetNextCharacter());

            inputOutputModule.Dispose();
        }

        [TestMethod]
        public void InsertErrorTest()
        {
            const string inputFileName = "input.txt";
            const string outputFileName = "output.txt";
            var inputText = string.Join(Environment.NewLine, new[] { "ab1", "qwe2", "asdf4" });
            File.WriteAllText(inputFileName, inputText);

            var inputOutputModule = new IoManager(inputFileName, outputFileName);
            var stringBuilder = new StringBuilder();

            while (!inputOutputModule.IsEndOfFile)
            {
                stringBuilder.Append(inputOutputModule.GetNextCharacter());
                inputOutputModule.InsertError(inputOutputModule.CurrentCharacterNumber, inputOutputModule.CurrentLineNumber);
            }

            inputOutputModule.Dispose();
        }
    }
}