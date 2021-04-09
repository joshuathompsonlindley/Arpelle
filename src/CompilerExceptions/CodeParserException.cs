/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using System;

namespace Arpelle.CompilerExceptions
{
    class CodeParserException : Exception
    {
        public CodeParserException() { }

        public CodeParserException(string message) : base(message) { }

        public CodeParserException(string message, Exception inner) : base(message, inner) { }
    }
}