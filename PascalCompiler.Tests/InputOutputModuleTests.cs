using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace PascalCompiler.Tests
{
    [TestClass]
    public class InputOutputModuleTests
    {
        [TestMethod]
        public void InputOutputModuleGetCharacterTest()
        {
            const string inputFileName = "input.txt";
            const string outputFileName = "output.txt";
            var inputText = string.Join(Environment.NewLine, new[] {"ab1", "qwe2", "asdf4"});
            File.WriteAllText(inputFileName, inputText);

            var inputOutputModule = new InputOutputModule(inputFileName, outputFileName);
            var stringBuilder = new StringBuilder();

            while (!inputOutputModule.IsEndOfFile)
                stringBuilder.Append(inputOutputModule.GetNextCharacter());

            inputOutputModule.Dispose();

            var outputText = File.ReadAllText(outputFileName);
            var readText = stringBuilder.ToString();

            inputText
                .Replace(Environment.NewLine, string.Empty)
                .Should()
                .BeEquivalentTo(readText);

            outputText
                .Trim()
                .Should()
                .BeEquivalentTo(inputText);
        }
    }
}

