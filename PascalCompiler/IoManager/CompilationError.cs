namespace PascalCompiler.IoManager
{
    public class CompilationError
    {
        public int LineNumber { get; }
        public int CharacterNumber { get; }
        public int ErrorCode { get; }

        public CompilationError(int lineNumber, int characterNumber, int errorCode)
        {
            LineNumber = lineNumber;
            CharacterNumber = characterNumber;
            ErrorCode = errorCode;
        }
    }
}
