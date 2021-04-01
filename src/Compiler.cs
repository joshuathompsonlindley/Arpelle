using System;
using System.IO;

namespace Arpelle
{

    public class Compiler
    {

        public static string cInputFile;
        public static string cOutputFile;
        public static string cExecutable;


        public static void Main(string[] args)
        {
            // arplc.exe <input> <executable>

            // if (!File.Exists(args[0]))
            //     Console.Write("Input file doesn't exist.");

            // else
            System.Console.WriteLine("ARPL Compiler for Arpelle.");
            
            cInputFile = File.ReadAllText("test.arpl");

            cOutputFile = "test.cpp";

            LexicalAnalyser.Lexer a = new LexicalAnalyser.Lexer(cInputFile);
            CodeEmitter.Emitter b = new CodeEmitter.Emitter(cOutputFile);
            CodeParser.Parser c = new CodeParser.Parser(a, b);

            c.StartParsing();
            b.WriteFile();
        }

    }

}