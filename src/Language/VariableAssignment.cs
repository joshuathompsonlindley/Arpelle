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
            VariableMetadata meta;

            parser.GetNextToken();

            if (parser.IsCurrentToken(TokenType.As))
            {
                parser.GetNextToken();

                string VariableDatatype = parser.CurrentToken.Text;

                parser.GetNextToken();
                parser.MatchToken(TokenType.Equals);

                if (parser.DeclaredVariables.ContainsKey(VariableIdentifer))
                    throw new CodeParserException("Variable " + VariableIdentifer + " already exists.");

                switch (VariableDatatype)
                {
                    case "String":
                        if (!parser.IsCurrentToken(TokenType.StringValue))
                            throw new CodeParserException("Expected a string for variable: " + VariableIdentifer + " of type String, but got: " + parser.CurrentToken.Type);

                        parser.CodeEmitter.EmitLine("string " + VariableIdentifer + " = \"" + parser.CurrentToken.Text + "\";");

                        meta = new VariableMetadata(){
                            Datatype = VariableDatatype,
                            Value = parser.CurrentToken.Text
                        };

                        parser.DeclaredVariables.Add(VariableIdentifer, meta);
                        parser.GetNextToken();

                        break;
                    case "Number":
                        if (!parser.IsCurrentToken(TokenType.NumberValue))
                            throw new CodeParserException("Expected a number for variable: " + VariableIdentifer + " of type Number, but got: " + parser.CurrentToken.Type);

                        if (parser.CurrentToken.Text.Contains('.'))
                            parser.CodeEmitter.EmitLine("float " + VariableIdentifer + " = \"" + parser.CurrentToken.Text + "\";");
                        else
                            parser.CodeEmitter.EmitLine("int " + VariableIdentifer + " = " + parser.CurrentToken.Text + ";");

                         meta = new VariableMetadata(){
                            Datatype = VariableDatatype,
                            Value = parser.CurrentToken.Text
                        };

                        parser.DeclaredVariables.Add(VariableIdentifer, meta);
                        parser.GetNextToken();

                        break;
                    case "Boolean":
                        if (!parser.IsCurrentToken(TokenType.True) && !parser.IsCurrentToken(TokenType.False))
                            throw new CodeParserException("Expected a boolean for variable: " + VariableIdentifer + " of type Number, but got: " + parser.CurrentToken.Type);

                        parser.CodeEmitter.EmitLine("boolean " + VariableIdentifer + " = " + parser.CurrentToken.Text.ToLower() + ";");

                        meta = new VariableMetadata(){
                            Datatype = VariableDatatype,
                            Value = parser.CurrentToken.Text
                        };

                        parser.DeclaredVariables.Add(VariableIdentifer, meta);
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
                    System.Console.Write("int");
                }
                else
                {
                    if (parser.DeclaredVariables.ContainsKey(VariableIdentifer) && parser.DeclaredVariables.TryGetValue(VariableIdentifer, out VariableMetadata Metadata))
                    {
                        switch (Metadata.Datatype)
                        {
                            case "String":
                                if (!parser.IsCurrentToken(TokenType.StringValue))
                                    throw new CodeParserException("Expected a string for variable: " + VariableIdentifer + " of type String, but got: " + parser.CurrentToken.Type);

                                parser.CodeEmitter.EmitLine(VariableIdentifer + " = \"" + parser.CurrentToken.Text + "\";");
                                break;
                            case "Number":
                                if (!parser.IsCurrentToken(TokenType.NumberValue))
                                    throw new CodeParserException("Expected a number for variable: " + VariableIdentifer + " of type Number, but got: " + parser.CurrentToken.Type);

                                parser.CodeEmitter.EmitLine(VariableIdentifer + " = " + parser.CurrentToken.Text + ";");
                                break;
                            case "Boolean":
                                if (!parser.IsCurrentToken(TokenType.True) || !parser.IsCurrentToken(TokenType.False))
                                    throw new CodeParserException("Expected a boolean for variable: " + VariableIdentifer + " of type Number, but got: " + parser.CurrentToken.Type);

                                parser.CodeEmitter.EmitLine(VariableIdentifer + " = " + parser.CurrentToken.Text.ToLower() + ";");
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