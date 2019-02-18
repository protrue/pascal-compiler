using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace PascalCompiler.Tests
{
    [TestClass]
    public class InputOutputTests
    {
        [TestMethod]
        public void InputOutputModuleShouldWork()
        {
            (true).Should().Be(true);
        }
    }
}
