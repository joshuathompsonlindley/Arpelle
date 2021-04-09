/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using Arpelle.CodeParser;
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