using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using PascalCompiler.Tokenizer;

namespace PascalCompiler.Tests
{
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        public void ScannerTest()
        {
            const string inputFileName = "input.txt";
            const string outputFileName = "output.txt";
            //var inputText = "*  /  =  ,  ;  :  .  ^  (  )  [  ]  {  }  <  >  <=  >=  <>  +  -  //  (*  *)  :=  .. &";
            var inputText =
//@"function Even(x: integer): boolean;
//begin
//  Result := x mod 2 = 0
//end;

//begin
//  writeln(Even(3.0));
//  writeln(Even(4));
//  writeln(Even(7777777777777777));
//end.";
                @"// comment
(* comments 
 comments 
*)
while";
            File.WriteAllText(inputFileName, inputText);

            var inputOutputModule = new IoManager.IoManager(inputFileName, outputFileName);

            var scanner = new Tokenizer.Tokenizer(inputOutputModule);
            var tokens = new List<Token>();

            while (!scanner.IsEndOfTokens)
                tokens.Add(scanner.GetNextToken());

            inputOutputModule.OutputStream.WriteLine();

            foreach (var token in tokens)
            {
                inputOutputModule.OutputStream.WriteLine(token.ToString());
            }
            
            inputOutputModule.Dispose();
        }
    }
}
