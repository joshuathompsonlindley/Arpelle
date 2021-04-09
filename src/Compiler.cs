/*
    Arpelle
    Copyright (c) 2021 Joshua Thompson-Lindley. All rights reserved.
    Licensed under the MIT License. See LICENSE file in the project root for full license information.
*/

using System;
using System.IO;
using System.Diagnostics;

using Arpelle.LexicalAnalyser;
using Arpelle.CodeParser;
using Arpelle.CodeEmitter;
using Arpelle.CompilerExceptions;

namespace Arpelle
{

    public class Compiler
    {
        public static string input;
        public static string output;
        public static string executable;

        public static void Main(string[] args)
        {
            if (args.Length >= 1 && args.Length <= 2)
            {
                string inputPath = args[0];
                output = args.Length == 2 ? args[1] : "arplc_output.cpp";

                if (File.Exists(inputPath))
                {
                    try
                    {
                        input = File.ReadAllText(inputPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[ERROR]: A system error occurred when reading file " + inputPath);
                        Console.WriteLine("         " + e.Message);
                    }

                    Console.WriteLine("[INFO]: Arpelle Compiler");
                    Console.WriteLine("[INFO]: by Joshua Thompson-Lindley (https://github.com/joshuathompsonlindley/Arpelle)");

                    Stopwatch watch = new Stopwatch();
                    Lexer lex = new Lexer(input);
                    Emitter emit = new Emitter(output);
                    Parser parse = new Parser(lex, emit);

                    try
                    {
                        watch.Start();
                        parse.StartParsing();
                        emit.WriteFile();
                        watch.Stop();

                        TimeSpan span = watch.Elapsed;
                        string time = span.Minutes + "m " + span.Seconds + "s " + span.Milliseconds + "ms.";

                        Console.WriteLine("[INFO]: Compilation finished in " + time);
                        Console.WriteLine("[INFO]: File saved as " + output);
                    }
                    catch (CodeParserException e)
                    {
                        Console.WriteLine("[ERROR]: An error occurred when parsing code on line " + lex.CurrentLineNumber + ":" + lex.CurrentLinePosition);
                        Console.WriteLine("         " + e.Message);
                    }
                    catch (CodeLexingException e)
                    {
                        Console.WriteLine("[ERROR]: An error occurred when analysing code on line " + lex.CurrentLineNumber + ":" + lex.CurrentLinePosition);
                        Console.WriteLine("         " + e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[ERROR]: A system error occurred when compiling code on line " + lex.CurrentLineNumber + ":" + lex.CurrentLinePosition);
                        Console.WriteLine("         " + e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("[ERROR]: The specified file " + inputPath + " does not exist.");
                }
            }
            else
            {
                Console.WriteLine("[ERROR]: Invalid Arguments");
                Console.WriteLine("Command Line Help:\narplc <input> [output]\nIf no output is given it defaults to: arplc_output");
            }
        }
    }
}