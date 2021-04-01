using Arpelle.CodeParser;
using Arpelle.CompilerExceptions;
using Arpelle.Tokens;

namespace Arpelle.Language.Keywords
{
    class InputKeyword : ILanguageConstruct
    {
        public void Parse(Parser parser)
        {
            parser.GetNextToken();
            
            if (parser.IsCurrentToken(TokenType.VariableIdentifer))
            {
                if (parser.DeclaredVariables.ContainsKey(parser.CurrentToken.Text) && parser.DeclaredVariables.TryGetValue(parser.CurrentToken.Text, out VariableMetadata Metadata))
                {
                    parser.CodeEmitter.EmitLine("cin >> " + parser.CurrentToken.Text);
                }
                else
                {
                    throw new CodeParserException("Variable " + parser.CurrentToken.Text + " does not exist.");
                }
            }
            else
            {
                throw new CodeParserException("Expected a variable identifier but instead got " + parser.CurrentToken.Text);
            }

            parser.GetNextToken();
        }
    }
}