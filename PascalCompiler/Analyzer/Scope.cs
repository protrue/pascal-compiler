using System.Collections.Generic;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public class Scope
    {
        public Dictionary<string, IdentifierClass> IdentifierClasses { get; set; }
        public Dictionary<string, Constant> Constants { get; set; }
        public Dictionary<string, Type> Types { get; set; }
        public Dictionary<string, Variable> Variables { get; set; }

        public Scope OuterScope { get; set; }

        public Scope()
        {
            IdentifierClasses = new Dictionary<string, IdentifierClass>();
            Constants = new Dictionary<string, Constant>();
            Types = new Dictionary<string, Type>();
            Variables = new Dictionary<string, Variable>();
        }
    }
}
