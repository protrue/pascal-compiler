using System.Collections.Generic;
using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public class Scope
    {
        public List<Entity> Entities { get; set; }
        public List< Constant> Constants { get; set; }
        public List< Type> Types { get; set; }
        public List< Variable> Variables { get; set; }

        public Scope OuterScope { get; set; }

        public Scope()
        {
            Entities = new List<Entity>();
            Constants = new List<Constant>();
            Types = new List<Type>();
            Variables = new List<Variable>();
        }
    }
}
