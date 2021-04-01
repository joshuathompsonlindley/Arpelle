using Arpelle.CodeParser;
using Arpelle.CompilerExceptions;
using Arpelle.Tokens;

namespace Arpelle.Language.Keywords
{
    class GotoKeyword : ILanguageConstruct
    {
        public void Parse(Parser parser)
        {
            parser.GetNextToken();
            parser.UsedLabels.Add(parser.CurrentToken.Text);
            parser.CodeEmitter.EmitLine("goto " + parser.CurrentToken.Text);
            parser.MatchToken(TokenType.VariableIdentifer);
            parser.GetNextToken();
        }
    }
}