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
    class LabelKeyword : ILanguageConstruct
    {
        public void Parse(Parser parser)
        {
            parser.GetNextToken();

            if (parser.DeclaredLabels.Contains(parser.CurrentToken.Text))
                throw new CodeParserException("Attempted to declare a label that already exists " + parser.CurrentToken.Text);

            parser.DeclaredLabels.Add(parser.CurrentToken.Text);
            parser.CodeEmitter.EmitLine(parser.CurrentToken.Text + ":");
            parser.MatchToken(TokenType.VariableIdentifer);
        }
    }
}