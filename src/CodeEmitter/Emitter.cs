using System;
using System.IO;
using System.Text;

namespace Arpelle.CodeEmitter
{
    class Emitter
    {
        public string @OutputPath { get; set; }
        public string HeaderCode { get; set; }
        public string Code { get; set; }

        public Emitter(string OutputPath = @"arplc_output.cpp")
        {
            this.OutputPath = OutputPath;
        }

        public void Emit(string code)
        {
            Code += code;
        }

        public void EmitLine(string code)
        {
            Code += code + '\n';
        }

        public void EmitHeader(string code)
        {
            HeaderCode += code + '\n';
        }

        public void WriteFile()
        {
            using (FileStream fs = File.Open(OutputPath, FileMode.Create))
            {
                byte[] writableStream = new UTF8Encoding(true).GetBytes(HeaderCode + Code);
                fs.Write(writableStream, 0, writableStream.Length);
            }
        }
    }
}