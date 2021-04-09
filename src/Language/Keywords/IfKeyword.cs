/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

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
            parser.MatchToken(TokenType.Then);
            parser.ParseNewLine();
            parser.CodeEmitter.EmitLine("){");

            while (!parser.IsCurrentToken(TokenType.End) && !parser.IsCurrentToken(TokenType.Else))
                parser.ParseStatement();

            if (parser.IsCurrentToken(TokenType.Else))
            {
                parser.MatchToken(TokenType.Else);
                parser.CodeEmitter.EmitLine("} else {");

                while (!parser.IsCurrentToken(TokenType.End))
                    parser.ParseStatement();
            }

            parser.MatchToken(TokenType.End);
            parser.CodeEmitter.EmitLine("}");
            parser.GetNextToken();
        }
    }

}