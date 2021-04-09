/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using System;
using System.Linq;
using System.Collections.Generic;

namespace Arpelle.Tokens
{
    public class Token
    {
        // Token text (eg: "False", "If" or a variable value.)
        public string Text { get; set; }
        // Token type (TokenType.False, TokenType.If, TokenType.ValueString)
        public TokenType Type { get; set; }

        public Token(string Text, TokenType Type)
        {
            this.Text = Text;
            this.Type = Type;
        }

        // Returns a token based on a string.
        // This is used for figuring out if a token is a keyword, like If, While, Set etc.
        public static TokenType StringToToken(string String)
        {
            // Get all TokenType values from the TokenType enum.
            IEnumerable<TokenType> Tokens = Enum.GetValues(typeof(TokenType)).Cast<TokenType>();

            foreach (TokenType CurrentTokenType in Tokens)
            {
                // Get the current token name and type (as integer)
                string TokenName = Enum.GetName(typeof(TokenType), CurrentTokenType);
                int TokenValue = (int)CurrentTokenType;

                // Keyword tokens exist as a number between 100 and 200 in the TokenType enum.
                if (String == TokenName && TokenValue >= 100 && TokenValue < 200)
                    return CurrentTokenType;
            }

            // None is the same as null.
            return TokenType.None;
        }
    }
}