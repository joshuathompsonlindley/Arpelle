/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

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