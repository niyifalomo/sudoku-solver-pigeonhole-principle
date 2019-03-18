using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

/*
 *  Uses CommandLine Parser .NET Package   https://commandline.codeplex.com/
 */

namespace SudokuSolver
{
    public class CommandOption
    {
        [Option('i', "input", Required = false, DefaultValue = "stdin" ,HelpText = "Input file containing sudoku.")]
        public string InputFile { get; set; }


        [Option('e', null, Required = false, HelpText = "Error Message file")]
        public string ErrorFile { get; set; }

        [Option('l', null, Required = false, DefaultValue = "stderr", HelpText = "Log file")]
        public string LogFile { get; set; }

        [Option('o', null, Required = false, DefaultValue = "stdout", HelpText = "Output file")]
        public string OutputFile { get; set; }


        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var usage = new StringBuilder();
            usage.AppendLine("Sudoku Solver 1.0");
            return usage.ToString();
        }
    }
}
