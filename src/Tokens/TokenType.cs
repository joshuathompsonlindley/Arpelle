namespace Arpelle.Tokens
{
    // List of tokens in Arpelle.
    public enum TokenType
    {
        EndOfFile = -1,             // End of File
        NewLine = 0,                // New Line
        NumberValue = 1,            // Number Value (Not Datatype)
        StringValue = 2,            // String Value (Not Datatype)
        VariableIdentifer = 3,      // Variable Identifier/Name
        Label = 101,                // Branching Label
        Goto = 102,                 // Branching Goto
        Printout = 103,             // Printout to Console
        Input = 104,                // Input from Console
        Set = 105,                  // Set variable keyword
        As = 106,                   // Datatype assignment keyword
        If = 107,                   // Begin If keyword
        Do = 108,                   // Do keyword
        EndIf = 109,                // End If keyword   
        While = 110,                // While loop keyword
        Repeat = 111,               // Repeat keyword
        EndWhile = 112,             // End While loop keyword
        True = 113,                 // True boolean type
        False = 114,                // False boolean type
        Boolean = 115,              // Boolean datatype
        String = 116,               // String datatype
        Number = 117,               // Number datatype
        Equals = 201,               // Equals Arithmetic symbol (=)
        Plus = 202,                 // Plus symbol (+)
        Minus = 203,                // Minus symbol (-)
        Asterisk = 204,             // Times/Asterisk symbol (*)
        Slash = 205,                // Divide/Slash symbol (/)
        EqualTo = 206,              // Boolean Equals to symbol (==)
        NotEqualTo = 207,           // Not Equals to symbol (!=)
        LessThan = 208,             // Less than symbol (<)
        LessThanEqualTo = 209,      // Less than equals to symbol (<=)
        GreaterThan = 210,          // Greater than symbol (>)
        GreaterThanEqualTo = 211,   // Greather than equals to symbol (>=)
        None = int.MaxValue         // Token is none/null, is MaxValue (int32 limit)
    }
}