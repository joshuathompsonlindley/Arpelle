using System;

namespace Arpelle.CompilerExceptions
{
    class CodeLexingException : Exception
    {
        public CodeLexingException() { }

        public CodeLexingException(string message) : base(message) { }

        public CodeLexingException(string message, Exception inner) : base(message, inner) { }
    }
}