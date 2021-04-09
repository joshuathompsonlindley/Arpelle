/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using Arpelle.CodeParser;
using Arpelle.CompilerExceptions;
using Arpelle.Tokens;

namespace Arpelle.Language.Keywords
{
    class PrintoutKeyword : ILanguageConstruct
    {
        public void Parse(Parser parser)
        {
            parser.GetNextToken();

            if (parser.IsCurrentToken(TokenType.VariableIdentifer))
            {
                if (parser.DeclaredVariables.ContainsKey(parser.CurrentToken.Text) && parser.DeclaredVariables.TryGetValue(parser.CurrentToken.Text, out VariableMetadata Metadata))
                {
                    if (Metadata.Datatype != "String" && Metadata.Datatype != "Number")
                        throw new CodeParserException("Tried to printout an unprintable variable.");

                    parser.CodeEmitter.EmitLine("cout << " + parser.CurrentToken.Text + " << endl;");
                }
                else
                {
                    throw new CodeParserException("Variable " + parser.CurrentToken.Text + " does not exist.");
                }
            }
            else if (parser.IsCurrentToken(TokenType.StringValue))
            {
                parser.CodeEmitter.EmitLine("cout << \"" + parser.CurrentToken.Text + "\" << endl;");
                parser.GetNextToken();
            }
            else if (parser.IsCurrentToken(TokenType.NumberValue))
            {
                parser.CodeEmitter.Emit("cout << ");
                parser.ParseExpression();
                parser.CodeEmitter.EmitLine(" << endl;");
            }
            else
            {
                throw new CodeParserException("Tried to printout an unprintable variable.");
            }

            parser.GetNextToken();
        }
    }
}