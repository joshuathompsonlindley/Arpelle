using Arpelle.CodeParser;

namespace Arpelle.Language
{
    interface ILanguageConstruct
    {
        void Parse(Parser parser);
    }
}