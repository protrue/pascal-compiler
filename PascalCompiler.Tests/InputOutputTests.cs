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
            var inputFileName = "input.txt";
            var inputOutputModule = new InputOutputModule(inputFileName, "output.txt");

            var stringBuilder = new StringBuilder();

            while (!inputOutputModule.IsEndOfFile)
                stringBuilder.Append(inputOutputModule.GetNextCharacter());

            var rawText = string.Join(string.Empty, File.ReadAllLines(inputFileName));
            var readText = stringBuilder.ToString();

            rawText.Should().BeEquivalentTo(readText);
        }
    }
}

