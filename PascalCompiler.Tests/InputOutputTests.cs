using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace PascalCompiler.Tests
{
    [TestClass]
    public class InputOutputTests
    {
        [TestMethod]
        public void InputOutputModuleGetCharacterTest()
        {
            const string inputFileName = "input.txt";
            const string outputFileName = "output.txt";

            File.WriteAllLines(inputFileName, new[] { "abc1", "qwe2" });

            var inputOutputModule = new InputOutputModule(inputFileName, outputFileName);
            var stringBuilder = new StringBuilder();

            while (!inputOutputModule.IsEndOfFile)
                stringBuilder.Append(inputOutputModule.GetNextCharacter());

            inputOutputModule.Dispose();

            var inputText = File.ReadAllText(inputFileName);
            var outputText = File.ReadAllText(outputFileName);
            var readText = stringBuilder.ToString();

            inputText
                .Replace(Environment.NewLine, string.Empty)
                .Should()
                .BeEquivalentTo(readText);

            outputText
                .Should()
                .BeEquivalentTo(inputText);
        }
    }
}

