using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SudokuSolver
{
    class Driver
    {
        static IPigeonHole _sudokuEngine = null;

        #region Initialize Puzzle variables

        static int puzzleRowCount = 9;
        static int puzzleColCount = 9;
        static int subgridM = 3;
        static int subgridN = 3;
        static int[] alphabet = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        static string inputFile = null;
        static string outputFile = null;
        private static string errorFile = null;
        private static int statusCode;
        #endregion

        static void Main(string[] args)
        {

            //Get command line arguments

            var cmdOption = new CommandOption();

            var parser = new Parser();

            #region Get commandline options and set execution parameters

            if (parser.ParseArguments(args, cmdOption))
            {
                // consume CommandOptions type properties

                inputFile = cmdOption.InputFile;
                outputFile = cmdOption.OutputFile;
                errorFile = cmdOption.ErrorFile;
                
                
            }

            #endregion


            //check if input file is supplied
            if (inputFile == null)
            {
                LogErrorMessage("Error: No input file supplied \nProgram Terminated");
            }

            /*
             * Initialize  and solve Puzzle
             */
            try
            {
                IPuzzle puzzle = new Puzzle(puzzleRowCount, puzzleColCount, subgridM, subgridN);

                puzzle.SetAlphabet(alphabet);

                puzzle.InitializeValues(cmdOption.InputFile);

                //Validate Initialization

                if (!puzzle.IsValid())
                {
                    LogErrorMessage("Error:   Invalid parameters. \nProgram Terminated");
                }

                
                //check if puzzle is already solved
                if (puzzle.IsSolved())
                {
                    LogErrorMessage("Error:   Puzzle is already solved");
                }
                else
                {
                    _sudokuEngine = new Engine(puzzle);
                    
                }

                //Attempt to solve puzzle, if puzzle is not yet solved
                if (_sudokuEngine != null)
                {
                    Console.WriteLine("Solving Sudoku. Please wait..\n");

                    statusCode = _sudokuEngine.SolvePuzzle();

                    Console.WriteLine("Process completed..\n");

                    string finalXML = puzzle.GenerateFinalXml();

                    WriteOutputToFile(finalXML);

                    //PrintResultOnScreen(_sudokuPigeonHole);
                }
                
            }
            catch (IOException e)
            {
                LogErrorMessage("Unable to read input file \n" +
                                         "Program Terminated.");

                statusCode = StatusCode.EncounteredError;

            }
            catch (Exception e)
            {
                LogErrorMessage("Unknown Error occured, check parameters \n Program Terminated");

                statusCode = StatusCode.EncounteredError;

            }

            DisplayStatusCode();

        }

        //Logs error to a an error file. Default stderr
        static void LogErrorMessage(string message)
        {
            if (errorFile != null)
            {
                //if file exists
                try
                {
                    var fileWriter = new FileWriter();

                    //check if file is writable
                    if (fileWriter.IsWritable(errorFile))
                    {
                        //Set error stream to error file
                        using (var errStream = new StreamWriter(errorFile))
                        {
                            Console.SetError(errStream);

                            Console.Error.WriteLine(message);
                        }

                    }

                }
                catch (IOException ex)
                {
                    Console.Error.WriteLine("Error: Unable to open Error File..\n");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error: Unable to write error file..\n");
                }

            }

            else
            {
                Console.Error.WriteLine(message);
            }

        }

        static void WriteOutputToFile(string xml)
        {
            var fileWriter = new FileWriter();
            if (outputFile != null && fileWriter.IsWritable(outputFile))
            {
                fileWriter.Write(outputFile, xml);
            }
        }

        static void DisplayStatusCode()
        {
            Console.WriteLine($"Status Code : {statusCode}");
            Console.ReadLine();
        }

        static void PrintResultOnScreen(IPigeonHole sudokuPigeonHole)
        {
            HashSet<PuzzleCell<int>> puzz = sudokuPigeonHole.GetPuzzle();
            for (int i = 0; i < puzzleRowCount; i++)
            {
                for (int j = 0; j < puzzleColCount; j++)
                {

                    var v = puzz.Where(x => x.X == i && x.Y == j).Select(x => x.Items).ToArray();
                    foreach (var val in v)
                    {
                        foreach (var val2 in val)
                        {
                            Console.Write(val2 + "||");
                        }
                    }

                    //Console.Write("/t");
                    //Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();

            }
        }



    }
}
