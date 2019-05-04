using System;
using System.Collections.Generic;
using System.IO;
using PascalCompiler.Tokenizer;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public partial class Analyzer : IDisposable
    {
        public IoManager.IoManager IoManager { get; private set; }
        public Tokenizer.Tokenizer Tokenizer { get; private set; }

        public Token CurrentToken { get; private set; }
        public Symbol CurrentSymbol => CurrentToken?.Symbol ?? Symbol.EndOfFile;

        public StreamWriter Output { get; set; }

        public Token AcceptedToken { get; private set; }

        public List<Scope> Scopes { get; set; }

        public Scope CurrentScope => Scopes[Scopes.Count - 1];

        private Type _currentType;
        private List<Variable> _currentVariables;
        private Type _currentRecord;

        public Analyzer(IoManager.IoManager ioManager, Tokenizer.Tokenizer tokenizer)
        {
            IoManager = ioManager;
            Tokenizer = tokenizer;
            Scopes = new List<Scope>();
            var fictitiousScope = new Scope();
            fictitiousScope.Types["integer"] = new Type() { Identifier = "integer", BaseType = BaseType.Scalar, ScalarType = ScalarType.Integer };
            fictitiousScope.Types["real"] = new Type() { Identifier = "real", BaseType = BaseType.Scalar, ScalarType = ScalarType.Real };
            fictitiousScope.Types["char"] = new Type() { Identifier = "char", BaseType = BaseType.Scalar, ScalarType = ScalarType.Char };
            fictitiousScope.Types["string"] = new Type() { Identifier = "string", BaseType = BaseType.Scalar, ScalarType = ScalarType.String };
            fictitiousScope.Types["boolean"] = new Type() { Identifier = "boolean", BaseType = BaseType.Scalar, ScalarType = ScalarType.Boolean };
            Scopes.Add(fictitiousScope);
        }

        private void InsertError(int errorCode)
        {
            IoManager.InsertError(CurrentToken?.CharacterNumber ?? IoManager.CurrentCharacterNumber, errorCode);
        }

        private void GetNextToken()
        {
            CurrentToken = Tokenizer.GetNextToken();
        }

        private void AcceptTerminal(Symbol expectedSymbol)
        {
            if (CurrentToken == null)
            {
                AcceptedToken = null;
                IoManager.InsertError(IoManager.CurrentCharacterNumber, (int)expectedSymbol);
                return;
            }

            if (CurrentSymbol == expectedSymbol)
            {
                AcceptedToken = CurrentToken;
                GetNextToken();
            }
            else
            {
                AcceptedToken = null;
                IoManager.InsertError(CurrentToken.CharacterNumber, (int)expectedSymbol);
            }
        }

        private bool Belongs(HashSet<Symbol> symbols) =>
            symbols.Contains(CurrentSymbol);

        private HashSet<Symbol> Union(HashSet<Symbol> first, HashSet<Symbol> second)
        {
            var result = new HashSet<Symbol>(first);
            result.UnionWith(second);
            return result;
        }

        private HashSet<Symbol> Union(HashSet<Symbol> first, HashSet<Symbol> second, HashSet<Symbol> third)
        {
            var result = new HashSet<Symbol>(first);
            result.UnionWith(second);
            result.UnionWith(third);
            return result;
        }

        private void SkipTo(HashSet<Symbol> symbols)
        {
            while (!symbols.Contains(CurrentSymbol) && CurrentToken != null)
            {
                GetNextToken();
            }
        }

        private void SkipTo(HashSet<Symbol> firstSet, HashSet<Symbol> secondSet)
        {
            while (!firstSet.Contains(CurrentSymbol) && !secondSet.Contains(CurrentSymbol) && CurrentToken != null)
            {
                GetNextToken();
            }
        }

        private Type SearchType(string identifier)
        {
            foreach (var scope in Scopes)
            {
                if (scope.Types.ContainsKey(identifier))
                {
                    return scope.Types[identifier];
                }
            }

            return null;
        }

        private Variable SearchVariable(string identifier)
        {
            foreach (var scope in Scopes)
            {
                if (scope.Variables.ContainsKey(identifier))
                {
                    return scope.Variables[identifier];
                }
            }

            return null;
        }

        private Constant SearchConstant(string identifier)
        {
            foreach (var scope in Scopes)
            {
                if (scope.Constants.ContainsKey(identifier))
                {
                    return scope.Constants[identifier];
                }
            }

            return null;
        }

        public void Analyze()
        {
            GetNextToken();
            AnalyzeBlocks();
        }

        public void Dispose()
        {
            Tokenizer.Dispose();
        }
    }
}
