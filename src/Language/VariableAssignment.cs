/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using Arpelle.CodeParser;
using Arpelle.CompilerExceptions;
using Arpelle.Tokens;

namespace Arpelle.Language
{
    class VariableMetadata
    {
        public string Value { get; set; }
        public string Datatype { get; set; }
    }

    class VariableAssignment : ILanguageConstruct
    {
        public void Parse(Parser parser)
        {
            parser.GetNextToken();

            string VariableIdentifer = parser.CurrentToken.Text;
            VariableMetadata metadata;

            parser.GetNextToken();

            if (parser.IsCurrentToken(TokenType.As))
            {
                parser.GetNextToken();

                string VariableDatatype = parser.CurrentToken.Text;

                parser.GetNextToken();
                parser.MatchToken(TokenType.Equals);

                if (parser.DeclaredVariables.ContainsKey(VariableIdentifer))
                    throw new CodeParserException("Variable " + VariableIdentifer + " already exists.");

                if (VariableIdentifer.ToLower() == "int" || VariableIdentifer.ToLower() == "string" || VariableIdentifer.ToLower() == "bool")
                    throw new CodeParserException("A variable cannot be named " + VariableIdentifer + ".");

                switch (VariableDatatype)
                {
                    case "String":
                        if (!parser.IsCurrentToken(TokenType.StringValue))
                            throw new CodeParserException("Expected a string for variable " + VariableIdentifer + " of type String, but got " + parser.CurrentToken.Text);

                        parser.CodeEmitter.EmitLine("string " + VariableIdentifer + " = \"" + parser.CurrentToken.Text + "\";");

                        metadata = new VariableMetadata()
                        {
                            Datatype = VariableDatatype,
                            Value = parser.CurrentToken.Text
                        };

                        parser.DeclaredVariables.Add(VariableIdentifer, metadata);
                        parser.GetNextToken();

                        break;

                    case "Number":
                        if (!parser.IsCurrentToken(TokenType.NumberValue))
                            throw new CodeParserException("Expected a number for variable " + VariableIdentifer + " of type Number, but got " + parser.CurrentToken.Text);

                        if (parser.CurrentToken.Text.Contains('.'))
                            parser.CodeEmitter.EmitLine("float " + VariableIdentifer + " = \"" + parser.CurrentToken.Text + "\";");
                        else
                            parser.CodeEmitter.EmitLine("int " + VariableIdentifer + " = " + parser.CurrentToken.Text + ";");

                        metadata = new VariableMetadata()
                        {
                            Datatype = VariableDatatype,
                            Value = parser.CurrentToken.Text
                        };

                        parser.DeclaredVariables.Add(VariableIdentifer, metadata);
                        parser.GetNextToken();

                        break;

                    case "Boolean":
                        if (!parser.IsCurrentToken(TokenType.True) && !parser.IsCurrentToken(TokenType.False))
                            throw new CodeParserException("Expected a boolean for variable " + VariableIdentifer + " of type Boolean, but got " + parser.CurrentToken.Text);

                        parser.CodeEmitter.EmitLine("bool " + VariableIdentifer + " = " + parser.CurrentToken.Text.ToLower() + ";");

                        metadata = new VariableMetadata()
                        {
                            Datatype = VariableDatatype,
                            Value = parser.CurrentToken.Text
                        };

                        parser.DeclaredVariables.Add(VariableIdentifer, metadata);
                        parser.GetNextToken();

                        break;

                    default:
                        throw new CodeParserException("Unknown data type " + VariableDatatype + ".");
                }
            }
            else if (parser.IsCurrentToken(TokenType.Equals))
            {
                parser.GetNextToken();
                if (parser.IsCurrentToken(TokenType.VariableIdentifer))
                {
                    // Deal with expressions...
                }
                else
                {
                    if (parser.DeclaredVariables.ContainsKey(VariableIdentifer) && parser.DeclaredVariables.TryGetValue(VariableIdentifer, out VariableMetadata Metadata))
                    {
                        switch (Metadata.Datatype)
                        {
                            case "String":
                                if (!parser.IsCurrentToken(TokenType.StringValue))
                                    throw new CodeParserException("Expected a string for variable " + VariableIdentifer + " of type String, but got " + parser.CurrentToken.Text);

                                parser.CodeEmitter.EmitLine(VariableIdentifer + " = \"" + parser.CurrentToken.Text + "\";");
                                Metadata.Value = parser.CurrentToken.Text;
                                parser.DeclaredVariables[VariableIdentifer] = Metadata;
                                parser.GetNextToken();

                                break;

                            case "Number":
                                if (!parser.IsCurrentToken(TokenType.NumberValue))
                                    throw new CodeParserException("Expected a number for variable: " + VariableIdentifer + " of type Number, but got: " + parser.CurrentToken.Type);

                                parser.CodeEmitter.EmitLine(VariableIdentifer + " = " + parser.CurrentToken.Text + ";");
                                Metadata.Value = parser.CurrentToken.Text;
                                parser.DeclaredVariables[VariableIdentifer] = Metadata;
                                parser.GetNextToken();

                                break;

                            case "Boolean":
                                if (!parser.IsCurrentToken(TokenType.True) || !parser.IsCurrentToken(TokenType.False))
                                    throw new CodeParserException("Expected a boolean for variable " + VariableIdentifer + " of type Boolean, but got " + parser.CurrentToken.Text);

                                parser.CodeEmitter.EmitLine(VariableIdentifer + " = " + parser.CurrentToken.Text.ToLower() + ";");
                                Metadata.Value = parser.CurrentToken.Text;
                                parser.DeclaredVariables[VariableIdentifer] = Metadata;
                                parser.GetNextToken();

                                break;

                            default:
                                throw new CodeParserException("Unknown data type " + Metadata.Datatype + ".");
                        }
                    }

                    else
                    {
                        throw new CodeParserException("An error occurred trying to get datatype for " + VariableIdentifer);
                    }
                }
            }
            else
            {
                throw new CodeParserException("An error occurred when trying to assign a value for variable " + VariableIdentifer + ".");
            }
        }
    }
}