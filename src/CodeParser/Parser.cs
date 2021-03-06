/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using System;
using System.Collections.Generic;

using Arpelle.CompilerExceptions;
using Arpelle.CodeEmitter;
using Arpelle.LexicalAnalyser;
using Arpelle.Tokens;
using Arpelle.Language.Keywords;
using Arpelle.Language;

namespace Arpelle.CodeParser
{
    class Parser
    {
        public Lexer Lexer;
        public Emitter CodeEmitter;
        public Dictionary<string, VariableMetadata> DeclaredVariables = new Dictionary<string, VariableMetadata>();
        public List<String> DeclaredLabels = new List<string>();
        public List<String> UsedLabels = new List<string>();
        public Token CurrentToken;
        public Token NextTokenAhead;

        public Parser(Lexer Lexer, Emitter CodeEmitter)
        {
            this.Lexer = Lexer;
            this.CodeEmitter = CodeEmitter;

            GetNextToken();
            GetNextToken();
        }

        public bool IsCurrentToken(TokenType Type) => Type == CurrentToken.Type;

        public bool IsAheadToken(TokenType Type) => Type == NextTokenAhead.Type;

        public void MatchToken(TokenType type)
        {
            if (!IsCurrentToken(type))
                throw new CodeParserException("Expected token: " + type + ", but got: " + CurrentToken.Type);

            GetNextToken();
        }

        public void GetNextToken()
        {
            CurrentToken = NextTokenAhead;

            try
            {
                NextTokenAhead = Lexer.GetNextToken();
            }
            catch (CodeLexingException e)
            {
                Console.WriteLine("[Lexer]: " + e.Message);
            }
        }

        public bool IsTokenComparisonOperator()
        {
            return IsCurrentToken(TokenType.GreaterThan) ||
                   IsCurrentToken(TokenType.GreaterThanEqualTo) ||
                   IsCurrentToken(TokenType.LessThan) ||
                   IsCurrentToken(TokenType.LessThanEqualTo) ||
                   IsCurrentToken(TokenType.NotEqualTo) ||
                   IsCurrentToken(TokenType.EqualTo);
        }

        public bool IsTokenArithmeticOperator()
        {
            return IsCurrentToken(TokenType.Plus) ||
                   IsCurrentToken(TokenType.Minus) ||
                   IsCurrentToken(TokenType.Slash) ||
                   IsCurrentToken(TokenType.Asterisk);
        }

        public void StartParsing()
        {
            CodeEmitter.EmitHeader("#include <iostream>");
            CodeEmitter.EmitHeader("#include <string>");
            CodeEmitter.EmitHeader("using namespace std;");
            CodeEmitter.EmitHeader("// Arpelle Compiler Output");
            CodeEmitter.EmitHeader("int main(void){");

            while (IsCurrentToken(TokenType.NewLine))
                GetNextToken();

            while (!IsCurrentToken(TokenType.EndOfFile))
                ParseStatement();

            CodeEmitter.EmitLine("return 0;");
            CodeEmitter.EmitLine("}");

            foreach (string label in UsedLabels)
                if (!DeclaredLabels.Contains(label))
                    throw new CodeParserException("The program is attempting to goto the undeclared label: " + label);
        }

        public void ParseStatement()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Set:
                    new VariableAssignment().Parse(this);
                    break;

                case TokenType.Printout:
                    new PrintoutKeyword().Parse(this);
                    break;

                case TokenType.If:
                    new IfKeyword().Parse(this);
                    break;

                case TokenType.While:
                    new WhileKeyword().Parse(this);
                    break;

                case TokenType.Goto:
                    new GotoKeyword().Parse(this);
                    break;

                case TokenType.Label:
                    new LabelKeyword().Parse(this);
                    break;

                case TokenType.Input:
                    new InputKeyword().Parse(this);
                    break;

                case TokenType.NewLine:
                    ParseNewLine();
                    break;

                case TokenType.EndOfFile:
                    break;

                default:
                    throw new CodeParserException("An unexpected token was being parsed: " + CurrentToken.Text + " of token type " + CurrentToken.Type.ToString());
            }
        }

        public void ParseComparison()
        {
            ParseExpression();

            if (IsTokenComparisonOperator())
            {
                CodeEmitter.Emit(CurrentToken.Text);
                GetNextToken();
                ParseExpression();
            }

            while (IsTokenComparisonOperator())
            {
                CodeEmitter.Emit(CurrentToken.Text);
                GetNextToken();
                ParseExpression();
            }
        }

        public void ParseExpression()
        {
            ParseTerm();

            while (IsCurrentToken(TokenType.Plus) || IsCurrentToken(TokenType.Minus))
            {
                CodeEmitter.Emit(CurrentToken.Text);
                GetNextToken();
                ParseTerm();
            }
        }

        public void ParseTerm()
        {
            ParseUnary();

            while (IsCurrentToken(TokenType.Asterisk) || IsCurrentToken(TokenType.Slash))
            {
                CodeEmitter.Emit(CurrentToken.Text);
                GetNextToken();
                ParseUnary();
            }
        }

        public void ParseUnary()
        {
            if (IsCurrentToken(TokenType.Plus) || IsCurrentToken(TokenType.Minus))
            {
                CodeEmitter.Emit(CurrentToken.Text);
                GetNextToken();
            }

            ParsePrimary();
        }

        public void ParsePrimary()
        {
            if (IsCurrentToken(TokenType.NumberValue) || IsCurrentToken(TokenType.StringValue))
            {
                CodeEmitter.Emit(CurrentToken.Text);
                GetNextToken();
            }
            else if (IsCurrentToken(TokenType.True) || IsCurrentToken(TokenType.False))
            {
                CodeEmitter.Emit(CurrentToken.Text.ToLower());
                GetNextToken();
            }
            else if (IsCurrentToken(TokenType.VariableIdentifer))
            {
                if (!DeclaredVariables.ContainsKey(CurrentToken.Text))
                    throw new CodeParserException("Variable " + CurrentToken.Text + " does not exist.");

                CodeEmitter.Emit(CurrentToken.Text);
                GetNextToken();
            }
            else
            {
                throw new CodeParserException("Unknown token " + CurrentToken.Text + ".");
            }
        }

        public void ParseNewLine()
        {
            if (IsCurrentToken(TokenType.EndOfFile))
                return;

            MatchToken(TokenType.NewLine);

            while (IsCurrentToken(TokenType.NewLine))
                GetNextToken();
        }
    }
}
