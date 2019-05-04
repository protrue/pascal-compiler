using System.Collections.Generic;

namespace PascalCompiler.Analyzer
{
    public class Record
    {
        public HashSet<Variable> Fields { get; set; }

        public Record()
        {
            Fields = new HashSet<Variable>();
        }
    }
}
