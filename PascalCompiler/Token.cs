namespace PascalCompiler
{
    public class Token
    {
        public int LineNumber { get; set; }
        public int CharacterNumber { get; set; }
        public int? Length { get; set; }
        public Constants.Symbol? Symbol { get; set; }
        public string TextValue { get; set; }
        public double? NumericValue { get; set; }

        public override string ToString() =>
            string.Join(' ',
                LineNumber.ToString(),
                CharacterNumber.ToString(),
                Symbol.ToString(),
                TextValue,
                NumericValue.ToString());

    }
}
