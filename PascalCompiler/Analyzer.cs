﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static PascalCompiler.Constants;

namespace PascalCompiler
{
    public class Analyzer : IDisposable
    {
        public IoManager IoManager { get; private set; }
        public Tokenizer Tokenizer { get; private set; }

        public Token CurrentToken { get; private set; }
        public Symbol CurrentSymbol => CurrentToken.Symbol.Value;

        public StreamWriter Output { get; set; }

        public Analyzer(IoManager ioManager, Tokenizer tokenizer)
        {
            IoManager = ioManager;
            Tokenizer = tokenizer;
        }

        private void GetNextToken()
        {
            CurrentToken = Tokenizer.GetNextToken();
        }

        private void AcceptTerminal(Symbol expectedSymbol)
        {
            if (CurrentToken == null)
            {
                IoManager.InsertError(IoManager.CurrentCharacterNumber, (int)expectedSymbol);
                return;
            }

            if (CurrentSymbol == expectedSymbol)
            {
                GetNextToken();
            }
            else
            {
                IoManager.InsertError(CurrentToken.CharacterNumber, (int)expectedSymbol);
            }
        }

        private void AnalyzeProgram()
        {
            if (CurrentSymbol == Symbol.Program)
            {
                AcceptTerminal(Symbol.Program);
                AcceptTerminal(Symbol.Identifier);
                if (CurrentSymbol == Symbol.LeftRoundBracket)
                {
                    AcceptTerminal(Symbol.LeftRoundBracket);
                    AcceptTerminal(Symbol.Identifier);
                    while (CurrentSymbol == Symbol.Comma)
                    {
                        AcceptTerminal(Symbol.Comma);
                        AcceptTerminal(Symbol.Identifier);
                    }
                }
                AcceptTerminal(Symbol.Semicolon);
            }
        }

        private void AnalyzeStatements()
        {
            AcceptTerminal(Symbol.Begin);
            AnalyzeStatement();
            while (CurrentSymbol == Symbol.Semicolon)
            {
                AcceptTerminal(Symbol.Semicolon);
                AnalyzeStatement();
            }
            AcceptTerminal(Symbol.End);
        }

        private void AnalyzeStatement()
        {
            switch (CurrentSymbol)
            {
                case Symbol.IntegerConstant:
                    AcceptTerminal(Symbol.IntegerConstant);
                    AnalyzeNonLabledStatement();
                    break;
                default:
                    AnalyzeNonLabledStatement();
                    break;
            }
        }

        private void AnalyzeNonLabledStatement()
        {

        }

        private void AnalyzeSimpleStatement()
        {

        }

        private void AnalyzeAssignStatement()
        {

        }

        private void AnalyzeVariable()
        {

        }

        private void AnalyzeFullVariable()
        {
            AnalyzeVariableName();
        }

        private void AnalyzeVariableName()
        {
            AcceptTerminal(Symbol.Identifier);
        }

        private void AnalyzeVariableComponent()
        {

        }

        private void AnalyzeIndexedVariable()
        {
            AnalyzeArrayVariable();
            AcceptTerminal(Symbol.LeftSquareBracket);
            AnalyzeExpression();
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AnalyzeExpression();
            }
            AcceptTerminal(Symbol.RightSquareBracket);
        }

        private void AnalyzeArrayVariable()
        {
            AnalyzeVariable();
        }

        private void AnalyzeRecordFieldVariable()
        {
            AnalyzeRecordVariable();
            AcceptTerminal(Symbol.Point);
            AnalyzeRecordFieldName();
        }

        private void AnalyzeRecordFieldName()
        {
            AcceptTerminal(Symbol.Identifier);
        }

        private void AnalyzeRecordVariable()
        {
            AnalyzeVariable();
        }

        private void AnalyzeReferencedVariable()
        {
            AnalyzeVariableReference();
            AcceptTerminal(Symbol.Caret);
        }

        private void AnalyzeVariableReference()
        {
            AnalyzeVariable();
        }

        private void AnalyzeFileBufferVariable()
        {
            AnalyzeFileVariable();
            AcceptTerminal(Symbol.Caret);
        }

        private void AnalyzeFileVariable()
        {
            AnalyzeVariable();
        }

        private void AnalyzeProcedureStatement()
        {

        }

        private void AnalyzeGotoStatement()
        {

        }

        private void AnalyzeComplexStatement()
        {

        }

        private void AnalyzeExpression()
        {

        }

        private void AnalyzeSimpleExpression()
        {

        }

        private void AnalyzeComparisonOperators()
        {
            switch (CurrentSymbol)
            {
                case Symbol.Equals:
                case Symbol.NotEqual:
                case Symbol.Less:
                case Symbol.LessOrEqual:
                case Symbol.Greater:
                case Symbol.GreaterOrEqual:
                case Symbol.In:
                    GetNextToken();
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1001);
                    break;
            }
        }

        private void AnalyzeAdditiveOperators()
        {
            switch (CurrentSymbol)
            {
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.Or:
                    GetNextToken();
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1002);
                    break;
            }
        }

        private void AnalyzeMultiplicativeOperators()
        {
            switch (CurrentSymbol)
            {
                case Symbol.Asterisk:
                case Symbol.Slash:
                case Symbol.And:
                case Symbol.Div:
                case Symbol.Mod:
                    GetNextToken();
                    break;
                default:
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1003);
                    break;
            }
        }

        private void AnalyzeAddend()
        {
            AnalyzeMultiplicand();
            while (CurrentSymbol == Symbol.Asterisk
            || CurrentSymbol == Symbol.Slash
            || CurrentSymbol == Symbol.And
            || CurrentSymbol == Symbol.Div
            || CurrentSymbol == Symbol.Mod)
            {
                AnalyzeMultiplicativeOperators();
                AnalyzeMultiplicand();
            }
        }

        private void AnalyzeMultiplicand()
        {

        }

        private void AnalyzeFunctions()
        {
            switch (CurrentSymbol)
            {
                case Symbol.Function:
                case Symbol.Procedure:
                    break;
            }
        }

        private void AnalyzeConstants()
        {
            if (CurrentSymbol == Symbol.Const)
            {
                AcceptTerminal(Symbol.Const);
                AcceptTerminal(Symbol.Identifier);
                AcceptTerminal(Symbol.Equals);
                AnalyzeConstant();
                AcceptTerminal(Symbol.Semicolon);
            }
        }

        private void AnalyzeConstant()
        {
            if (CurrentSymbol == Symbol.Plus || CurrentSymbol == Symbol.Minus)
            {
                switch (CurrentSymbol)
                {
                    case Symbol.FloatConstant:
                        AcceptTerminal(Symbol.FloatConstant);
                        break;
                    case Symbol.IntegerConstant:
                        AcceptTerminal(Symbol.IntegerConstant);
                        break;
                    default:
                        GetNextToken();
                        IoManager.InsertError(CurrentToken.CharacterNumber, 50);
                        break;
                }
            }
            else
            {
                switch (CurrentSymbol)
                {
                    case Symbol.FloatConstant:
                        AcceptTerminal(Symbol.FloatConstant);
                        break;
                    case Symbol.IntegerConstant:
                        AcceptTerminal(Symbol.IntegerConstant);
                        break;
                    case Symbol.CharConstant:
                        AcceptTerminal(Symbol.CharConstant);
                        break;
                    case Symbol.StringConstant:
                        AcceptTerminal(Symbol.StringConstant);
                        break;
                    default:
                        GetNextToken();
                        IoManager.InsertError(CurrentToken.CharacterNumber, 83);
                        break;
                }
            }
        }

        private void AnalyzeVariables()
        {
            if (CurrentSymbol == Symbol.Var)
            {
                AcceptTerminal(Symbol.Var);
                AnalyzeSameTypeVariables();
                AcceptTerminal(Symbol.Semicolon);
            }
        }

        private void AnalyzeSameTypeVariables()
        {
            AcceptTerminal(Symbol.Identifier);
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AcceptTerminal(Symbol.Identifier);
            }
            AcceptTerminal(Symbol.Colon);
            AnalyzeType();
        }

        private void AnalyzeTypes()
        {
            if (CurrentSymbol == Symbol.Type)
            {
                AcceptTerminal(Symbol.Type);
                AcceptTerminal(Symbol.Equals);
                AnalyzeType();
                AcceptTerminal(Symbol.Semicolon);
            }
        }

        private void AnalyzeType()
        {
            switch (CurrentSymbol)
            {
                case Symbol.Identifier:
                case Symbol.LeftRoundBracket:
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.IntegerConstant:
                case Symbol.FloatConstant:
                case Symbol.CharConstant:
                case Symbol.StringConstant:
                    AnalyzeSimpleType();
                    break;
                case Symbol.Record:
                case Symbol.Array:
                case Symbol.Set:
                case Symbol.File:
                case Symbol.Packed:
                    AnalyzeComplexType();
                    break;
                case Symbol.Caret:
                    AnalyzeReferenceType();
                    break;
                default:
                    GetNextToken();
                    IoManager.InsertError(CurrentToken.CharacterNumber, 331);
                    break;
            }
        }

        private void AnalyzeSimpleType()
        {
            switch (CurrentSymbol)
            {
                case Symbol.Identifier:
                    AcceptTerminal(Symbol.Identifier);
                    break;
                case Symbol.LeftRoundBracket:
                    AnalyzeEnumerableType();
                    break;
                case Symbol.Plus:
                case Symbol.Minus:
                case Symbol.IntegerConstant:
                case Symbol.FloatConstant:
                case Symbol.CharConstant:
                case Symbol.StringConstant:
                    AnalyzeLimitedType();
                    break;
                default:
                    GetNextToken();
                    IoManager.InsertError(CurrentToken.CharacterNumber, 1);
                    break;
            }
        }

        private void AnalyzeEnumerableType()
        {
            AcceptTerminal(Symbol.LeftRoundBracket);
            AcceptTerminal(Symbol.Identifier);
            while (CurrentSymbol == Symbol.Comma)
            {
                AcceptTerminal(Symbol.Comma);
                AcceptTerminal(Symbol.Identifier);
            }
            AcceptTerminal(Symbol.RightRoundBracket);
        }

        private void AnalyzeLimitedType()
        {
            AnalyzeConstant();
            AcceptTerminal(Symbol.TwoPoints);
            AnalyzeConstant();
        }

        private void AnalyzeComplexType()
        {
            if (CurrentSymbol == Symbol.Packed)
                AcceptTerminal(Symbol.Packed);
            AnalyzeUnpackedComplexType();
        }

        private void AnalyzeUnpackedComplexType()
        {
            switch (CurrentSymbol)
            {
                case Symbol.Array:
                    AnalyzeRegularType();
                    break;
                case Symbol.Record:
                    AnalyzeCombinedType();
                    break;
                case Symbol.Set:
                    AnalyzeSetType();
                    break;
                case Symbol.File:
                    AnalyzeFileType();
                    break;
                default:
                    //IoManager.InsertError(CurrentToken.CharacterNumber, ???);
                    break;
            }
        }

        private void AnalyzeRegularType()
        {
            AcceptTerminal(Symbol.Array);
            AcceptTerminal(Symbol.LeftSquareBracket);
            AnalyzeSimpleType();
            while (CurrentSymbol == Symbol.Comma) ;
            {
                AcceptTerminal(Symbol.Colon);
                AnalyzeSimpleType();
            }
            AcceptTerminal(Symbol.RightSquareBracket);
            AcceptTerminal(Symbol.Of);
            AcceptTerminal(Symbol.Identifier);
        }

        private void AnalyzeCombinedType()
        {
            AcceptTerminal(Symbol.Record);
            AnalyzeRecordFields();
            AcceptTerminal(Symbol.End);
        }

        private void AnalyzeRecordFields()
        {
            AnalyzeRecordFixedParts();
        }

        private void AnalyzeRecordFixedParts()
        {
            AnalyzeRecordSection();
            while (CurrentSymbol == Symbol.Semicolon)
            {
                AcceptTerminal(Symbol.Semicolon);
                AnalyzeRecordSection();
            }
        }

        private void AnalyzeRecordSection()
        {
            if (CurrentSymbol == Symbol.Identifier)
            {
                AcceptTerminal(Symbol.Identifier);
                while (CurrentSymbol == Symbol.Comma)
                {
                    AcceptTerminal(Symbol.Comma);
                    AcceptTerminal(Symbol.Identifier);
                }
                AcceptTerminal(Symbol.Colon);
                AnalyzeType();
            }
        }

        private void AnalyzeSetType()
        {
            AcceptTerminal(Symbol.Set);
            AcceptTerminal(Symbol.Of);
            AnalyzeType();
        }

        private void AnalyzeFileType()
        {
            AcceptTerminal(Symbol.File);
            AcceptTerminal(Symbol.Of);
            AnalyzeType();
        }

        private void AnalyzeReferenceType()
        {
            AcceptTerminal(Symbol.Caret);
            AcceptTerminal(Symbol.Identifier);
        }

        private void AnalyzeLabels()
        {
            if (CurrentSymbol == Symbol.Label)
            {
                AcceptTerminal(Symbol.Label);
                AcceptTerminal(Symbol.IntegerConstant);
            }
        }

        public void Analyze()
        {
            if (Tokenizer.IsEndOfTokens)
                return;

            GetNextToken();

            AnalyzeProgram();
            AnalyzeLabels();
            AnalyzeConstants();
            AnalyzeTypes();
            AnalyzeVariables();
            AnalyzeFunctions();
            AnalyzeStatements();

            AcceptTerminal(Symbol.Point);
        }

        public void Dispose()
        {
            Tokenizer.Dispose();
        }
    }
}
