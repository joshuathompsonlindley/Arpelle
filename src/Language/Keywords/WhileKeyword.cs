using Arpelle.CodeParser;
using Arpelle.Tokens;

namespace Arpelle.Language.Keywords
{

    class WhileKeyword : ILanguageConstruct
    {
        public void Parse(Parser parser)
        {
            parser.GetNextToken();
            parser.CodeEmitter.Emit("while(");
            parser.ParseComparison();
            parser.MatchToken(TokenType.Repeat);
            parser.ParseNewLine();
            parser.CodeEmitter.EmitLine("){");

            while (!parser.IsCurrentToken(TokenType.EndWhile))
                parser.ParseStatement();

            parser.MatchToken(TokenType.EndWhile);
            parser.CodeEmitter.EmitLine("}");
            parser.GetNextToken();
        }
    }

}