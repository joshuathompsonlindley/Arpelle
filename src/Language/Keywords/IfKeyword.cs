using Arpelle.CodeParser;
using Arpelle.Tokens;

namespace Arpelle.Language.Keywords
{

    class IfKeyword : ILanguageConstruct
    {
        public void Parse(Parser parser)
        {
            parser.GetNextToken();
            parser.CodeEmitter.Emit("if(");
            parser.ParseComparison();
            parser.MatchToken(TokenType.Do);
            parser.ParseNewLine();
            parser.CodeEmitter.EmitLine("){");

            while (!parser.IsCurrentToken(TokenType.EndIf))
                parser.ParseStatement();

            parser.MatchToken(TokenType.EndIf);
            parser.CodeEmitter.EmitLine("}");
            parser.GetNextToken();
        }
    }

}