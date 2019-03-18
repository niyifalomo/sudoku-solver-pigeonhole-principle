using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Puzzle : IPuzzle
    {

        public Dimension OverallDimension;
        public Dimension SubgridDimension;
        public Alphabet PuzzleAlphabet;
        public PuzzleXml puzzle;
        //public int[,] PuzzleArray;
        public HashSet<PuzzleCell<int>> PuzzleGrid;

        //initialize puzzle
        public Puzzle(int maxRow, int maxColumn, int subgridX, int subgridY)
        {
            OverallDimension = new Dimension(maxRow, maxColumn);
            SubgridDimension = new Dimension(subgridX, subgridY);
            //PuzzleAlphabet = alphabet;
        }


        public Alphabet GetAlphabet()
        {
            return PuzzleAlphabet;
        }

        public HashSet<PuzzleCell<int>> GetPuzzleGrid()
        {
            return PuzzleGrid;
        }

        public void SetAlphabet(int[] alphabet)
        {
            PuzzleAlphabet = new Alphabet(alphabet);
        }


        public int GetOverallColumnCount()
        {
            return OverallDimension.ColSize;
        }

        public int GetOverallRowCount()
        {
            return OverallDimension.RowSize;
        }
        public int GetSubgridColumnCount()
        {
            return SubgridDimension.ColSize;
        }

        public int GetSubgridRowCount()
        {
            return SubgridDimension.RowSize;
        }

        //works for 9*9
        //Validates the alphabet, overall-dimension and subgrid-dimension
        public bool IsValid()
        {
            var alphabet = PuzzleAlphabet.GetAllElements();

            //if alphabet is not set
            if (alphabet.Length == 0)
                return false;

            //chek overall dimension
            if (OverallDimension.RowSize != alphabet.Length || OverallDimension.ColSize != alphabet.Length)
                return false;

            //check subgrid..implement later

            if ((OverallDimension.RowSize % SubgridDimension.RowSize != 0) ||
                (OverallDimension.ColSize % SubgridDimension.ColSize != 0))
                return false;

            return true;
        }

        public void InitializeValues(string filePath)
        {

            Serializer ser = new Serializer();

            filePath = Directory.GetCurrentDirectory() + $@"\{filePath}";
            var xmlInputData = File.ReadAllText(filePath);

            string path = string.Empty;

            //Deserialize XMLInput into xml object

            puzzle = ser.Deserialize<PuzzleXml>(xmlInputData);

            //convert to Hashset

            ConvertPuzzleXmlToHashSet();


        }

        /*
         * Converts the xmlobject to hashset
         */
        public void ConvertPuzzleXmlToHashSet()
        {

            var items = new HashSet<PuzzleCell<int>>();

            var alphabet = PuzzleAlphabet.GetAllElements();

            //PuzzleArray = new int[OverallDimension.RowSize, OverallDimension.ColSize];

            int row;

            for (row = 0; row < OverallDimension.RowSize; row++)
            {
                for (int col = 0; col < OverallDimension.ColSize; col++)
                {
                    var cell = puzzle.Initialvalues.Cell.Find(
                        x => x.Row == Convert.ToString(row) && x.Col == Convert.ToString(col));

                    //if present in xml
                    if (cell != null)
                    {
                        int[] arr = { Convert.ToInt32(cell.Value) };
                        items.Add(new PuzzleCell<int>(row, col) { Items = arr });



                    }
                    // PuzzleArray[row, col] = Convert.ToInt32(cell.Value);
                    else
                    {
                        //int[] arr = alphabet;
                        items.Add(new PuzzleCell<int>(row, col) { Items = alphabet });
                    }
                }
            }

            PuzzleGrid = items;


        }


        public string GenerateFinalXml()
        {
            Serializer ser = new Serializer();

            var puzzleGrid = GetPuzzleGrid();
            var puzzleXml = new PuzzleXml();
            var initialValues = new Initialvalues();
            var cells = new List<Cell>();

            //Loop through hashset to build xmlobject data.

            for (int row = 0; row < OverallDimension.RowSize; row++)
            {
                for (int col = 0; col < OverallDimension.ColSize; col++)
                {
                    var cellValues = puzzleGrid.Where(x => x.X == row && x.Y == col).Select(x => x.Items).ToArray();
                    foreach (var cellValue in cellValues)
                    {
                        foreach (var value in cellValue)
                        {
                            string rowNumber = Convert.ToString(row);
                            string colNumber = Convert.ToString(col);
                            string cellvalue = Convert.ToString(value);
                            cells.Add(new Cell { Row = rowNumber, Col = colNumber, Value = cellvalue });
                        }
                    }

                }
            }

            initialValues.Cell = cells;
            puzzleXml.Initialvalues = initialValues;

            var xmlOutputData = ser.Serialize(puzzleXml);

            return xmlOutputData;
        }


        /*
         * Checks if region contains alphabet elements only
         */
        public bool IsValidRegion(HashSet<PuzzleCell<int>> region)
        {

            var regionValues = region.SelectMany(x => x.Items).ToArray();
            var alphabetValues = PuzzleAlphabet.GetAllElements();

            //sort array before comparing them
            Array.Sort(alphabetValues);
            Array.Sort(regionValues);

            if (!regionValues.SequenceEqual(alphabetValues))
            {
                return false;
            }

            return true;
        }

        public bool AreRowsSolved()
        {
            //validate rows

            for (int i = 0; i < OverallDimension.RowSize; i++)
            {
                //holds region cells
                var region = new HashSet<PuzzleCell<int>>();

                for (int j = 0; j < OverallDimension.ColSize; j++)
                {
                    var cellValues = PuzzleGrid.Where(x => x.X == i && x.Y == j)
                        .Select(x => x.Items)
                        .FirstOrDefault();

                    region.Add(new PuzzleCell<int>(i, j) { Items = cellValues });
                }

                if (!IsValidRegion(region))
                {
                    return false;
                }

            }
            return true;
        }

        public bool AreColumnsSolved()
        {
            //validate columns
            for (int i = 0; i < OverallDimension.RowSize; i++)
            {
                //holds region cells
                var region = new HashSet<PuzzleCell<int>>();

                for (int j = 0; j < OverallDimension.ColSize; j++)
                {
                    var cellValues = PuzzleGrid.Where(x => x.X == j && x.Y == i)
                        .Select(x => x.Items)
                        .FirstOrDefault();

                    region.Add(new PuzzleCell<int>(j, i) { Items = cellValues });
                }

                if (!IsValidRegion(region))
                {
                    return false;
                }

            }
            return true;
        }

        public bool AreSubgridsSolved()
        {
            //validate subgrid


            for (int i = 0; i < OverallDimension.RowSize / SubgridDimension.RowSize; i++)
            {
                for (int j = 0; j < OverallDimension.ColSize / SubgridDimension.ColSize; j++)
                {
                    int row = i * 3;
                    int col = j * 3;

                    // check in 3 by 3 subgrid 
                    var region = new HashSet<PuzzleCell<int>>();


                    //Get K combinations
                    for (int m = 0; m < SubgridDimension.RowSize; m++)
                    {
                        for (int n = 0; n < SubgridDimension.ColSize; n++)
                        {

                            var n1 = n;
                            var m1 = m;

                            var cellValues = PuzzleGrid.Where(x => x.X == m1 + row && x.Y == n1 + col)
                                .Select(x => x.Items)
                                .FirstOrDefault();

                            // add to subgrid hashset
                            region.Add(new PuzzleCell<int>(m1 + row, n1 + col) { Items = cellValues });

                        }
                    }

                    if (!IsValidRegion(region))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsSolved()
        {

            //check if all region cells contain single values and total 81 values(row*col)
            int totalCells = OverallDimension.ColSize * OverallDimension.RowSize;

            //validate individual cells
            int totalSingleValuedCells = PuzzleGrid.Count(x => x.Items.Length == 1);

            if (totalSingleValuedCells != totalCells)
                return false;

            //validate regions

            if (!AreRowsSolved())
                return false;

            if (!AreColumnsSolved())
                return false;

            if (!AreSubgridsSolved())
                return false;


            return true;
        }




    }
}
