/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using Arpelle.CodeParser;

namespace Arpelle.Language
{
    interface ILanguageConstruct
    {
        void Parse(Parser parser);
    }
}