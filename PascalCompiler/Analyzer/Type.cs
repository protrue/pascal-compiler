using static PascalCompiler.Constants.Constants;

namespace PascalCompiler.Analyzer
{
    public class Type
    {
        public string Identifier { get; set; }
        public BaseType BaseType { get; set; }
        public ScalarType? ScalarType { get; set; }
        public Record Record { get; set; }
    }
}
