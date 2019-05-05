using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private Token _storedToken;
        private Token _previousToken;

        public Analyzer(IoManager.IoManager ioManager, Tokenizer.Tokenizer tokenizer)
        {
            IoManager = ioManager;
            Tokenizer = tokenizer;
            Scopes = new List<Scope>();
            var fictitiousScope = new Scope();
            fictitiousScope.Types.Add(new Type() { Identifier = "integer", BaseType = BaseType.Scalar, ScalarType = ScalarType.Integer });
            fictitiousScope.Types.Add(new Type() { Identifier = "real", BaseType = BaseType.Scalar, ScalarType = ScalarType.Real });
            fictitiousScope.Types.Add(new Type() { Identifier = "char", BaseType = BaseType.Scalar, ScalarType = ScalarType.Char });
            fictitiousScope.Types.Add(new Type() { Identifier = "string", BaseType = BaseType.Scalar, ScalarType = ScalarType.String });
            fictitiousScope.Types.Add(new Type() { Identifier = "boolean", BaseType = BaseType.Scalar, ScalarType = ScalarType.Boolean });
            Scopes.Add(fictitiousScope);
        }

        private void InsertError(int errorCode)
        {
            IoManager.InsertError(CurrentToken?.CharacterNumber ?? IoManager.CurrentCharacterNumber, errorCode);
        }

        private void StepBack()
        {
            _storedToken = CurrentToken;
            CurrentToken = _previousToken;
        }

        private void GetNextToken()
        {
            _previousToken = CurrentToken;

            if (_storedToken != null)
            {
                CurrentToken = _storedToken;
                return;
            }
            
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
                var type = scope.Types.FirstOrDefault(t => t.Identifier == identifier);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        private Variable SearchVariable(string identifier)
        {
            foreach (var scope in Scopes)
            {
                var variable = scope.Variables.FirstOrDefault(v => v.Identifier == identifier);
                if (variable != null)
                {
                    return variable;
                }
            }

            return null;
        }

        private Constant SearchConstant(string identifier)
        {
            foreach (var scope in Scopes)
            {
                var constant = scope.Constants.FirstOrDefault(c => c.Identifier == identifier);
                if (constant != null)
                {
                    return constant;
                }
            }

            return null;
        }

        private Entity Search(string identifier)
        {
            foreach (var scope in Scopes)
            {
                var entity = scope.Entities.FirstOrDefault(e => e.Identifier == identifier);
                if (entity != null)
                {
                    return entity;
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
