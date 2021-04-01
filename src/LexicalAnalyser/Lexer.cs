using System;
using Arpelle.Tokens;
using Arpelle.CompilerExceptions;

namespace Arpelle.LexicalAnalyser
{
    class Lexer
    {
        private readonly string SourceCode = "";
        private char CurrentCharacter = ' ';
        private int CurrentCharacterPosition = -1;

        public Lexer(string SourceCode)
        {
            this.SourceCode = SourceCode;
            MoveNextCharacter();
        }

        private void MoveNextCharacter()
        {
            CurrentCharacterPosition += 1;

            if (CurrentCharacterPosition >= SourceCode.Length)
                CurrentCharacter = '\0';
            else
                CurrentCharacter = SourceCode[CurrentCharacterPosition];
        }

        private char GetNextCharacterAhead()
        {
            if (CurrentCharacterPosition + 1 >= SourceCode.Length)
                return '\0';
            else
                return SourceCode[CurrentCharacterPosition + 1];
        }

        public Token GetNextToken()
        {
            Token NextToken;
            int SubstringStart = 0;
            string Substring = "";

            while (CurrentCharacter == ' ' || CurrentCharacter == '\t' || CurrentCharacter == '\r')
                MoveNextCharacter();

            if (CurrentCharacter == '#')
                while (CurrentCharacter != '\n')
                    MoveNextCharacter();

            switch (CurrentCharacter)
            {
                case '+':
                    NextToken = new Token("+", TokenType.Plus);
                    break;
                case '-':
                    NextToken = new Token("-", TokenType.Minus);
                    break;
                case '*':
                    NextToken = new Token("*", TokenType.Asterisk);
                    break;
                case '/':
                    NextToken = new Token("/", TokenType.Slash);
                    break;
                case '=':
                    NextToken = GetNextCharacterAhead() == '=' ? new Token("==", TokenType.EqualTo) : new Token("=", TokenType.Equals);
                    MoveNextCharacter();
                    break;
                case '>':
                    NextToken = GetNextCharacterAhead() == '=' ? new Token(">=", TokenType.GreaterThanEqualTo) : new Token(">", TokenType.GreaterThan);
                    MoveNextCharacter();
                    break;
                case '<':
                    NextToken = GetNextCharacterAhead() == '=' ? new Token("<=", TokenType.LessThanEqualTo) : new Token("<", TokenType.LessThan);
                    MoveNextCharacter();
                    break;
                case '!':
                    NextToken = GetNextCharacterAhead() == '=' ? new Token("!=", TokenType.NotEqualTo) : throw new CodeLexingException("Expected !=, but got !" + GetNextCharacterAhead());
                    MoveNextCharacter();
                    break;
                case '\"':
                    MoveNextCharacter();
                    SubstringStart = CurrentCharacterPosition;

                    while (CurrentCharacter != '\"')
                    {
                        if (Char.IsControl(CurrentCharacter) || CurrentCharacter == '\\' || CurrentCharacter == '%')
                            throw new CodeLexingException("An illegal character was found in a string.");

                        MoveNextCharacter();
                    }

                    Substring = SourceCode[SubstringStart..CurrentCharacterPosition];
                    NextToken = new Token(Substring, TokenType.StringValue);
                    break;
                case '\n':
                    NextToken = new Token("\n", TokenType.NewLine);
                    break;
                case '\0':
                    NextToken = new Token("\0", TokenType.EndOfFile);
                    break;
                default:
                    if (Char.IsDigit(CurrentCharacter))
                    {
                        SubstringStart = CurrentCharacterPosition;

                        while (Char.IsDigit(GetNextCharacterAhead()))
                            MoveNextCharacter();

                        if (Char.IsDigit(CurrentCharacter))
                            MoveNextCharacter();

                        if (GetNextCharacterAhead() == '.')
                        {
                            MoveNextCharacter();

                            if (!Char.IsDigit(GetNextCharacterAhead()))
                                throw new CodeLexingException("An illegal character was found in a number.");

                            while (Char.IsDigit(GetNextCharacterAhead()))
                                MoveNextCharacter();
                        }

                        if (SubstringStart != CurrentCharacterPosition)
                            Substring = SourceCode[SubstringStart..CurrentCharacterPosition];
                        else
                            Substring = CurrentCharacter.ToString();

                        NextToken = new Token(Substring, TokenType.NumberValue);
                    }
                    else if (Char.IsLetter(CurrentCharacter))
                    {
                        SubstringStart = CurrentCharacterPosition;

                        while (Char.IsLetterOrDigit(GetNextCharacterAhead()))
                            MoveNextCharacter();
                            
                        if(GetNextCharacterAhead() != '\n')
                        {
                            MoveNextCharacter();
                            Substring = SourceCode[SubstringStart..CurrentCharacterPosition];
                        } else {
                            // If the newline is the next character, it'll break the parsers, so we hack it a little bit.
                            int PaddingCharacterPosition = CurrentCharacterPosition + 1;
                            Substring = SourceCode[SubstringStart..PaddingCharacterPosition];
                        }

                        TokenType TokenKeyword = Token.StringToToken(Substring);
                        NextToken = TokenKeyword == TokenType.None ? new Token(Substring, TokenType.VariableIdentifer) : new Token(Substring, TokenKeyword);
                    }
                    else
                    {
                        throw new CodeLexingException("This is an invalid token: " + CurrentCharacter);
                    }
                    break;
            }

            MoveNextCharacter();
            return NextToken;
        }
    }
}