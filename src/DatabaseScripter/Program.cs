#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace DatabaseScripter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            if (args != null)
            {
                List<string> argList = args.ToList();
                if (argList.Count == 3)
                {
                    Worker worker = new Worker(argList[0], argList[1], argList[2]);
                    worker.ScriptDatabase();
                }
                else
                {
                    Console.WriteLine("ERROR: Arguments are missing.");
                    OuputInstructions();
                }
            }
            else
            {
                OuputInstructions();
            }
        }

        static void OuputInstructions()
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine("The following example demonstrates execution.");
            Console.WriteLine("DatabaseScripter.exe \"ServerName\" \"DatabaseName\" \"outputPath\" ");
            Console.WriteLine(" or");
            Console.WriteLine("DatabaseScripter.exe \".\\SQLExpress\" \"VsDbProject\" \"c:\\temp\\dbscripter\\\" ");
            Console.WriteLine("*******************************************");
        }
    }
}
