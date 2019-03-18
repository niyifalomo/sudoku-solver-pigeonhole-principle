using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{

    public class Engine : IPigeonHole
    {

        private readonly int _overallGridColumnCount;
        private readonly int _overallGridRowCount;
        private readonly int _subgridM;
        private readonly int _subgridN;
        private readonly IPuzzle _puzzle;


        public readonly HashSet<PuzzleCell<int>> PuzzleGrid;


        public Engine(IPuzzle puzzle)
        {
            PuzzleGrid = puzzle.GetPuzzleGrid();
            _puzzle = puzzle;
            _overallGridColumnCount = puzzle.GetOverallColumnCount();
            _overallGridRowCount = puzzle.GetOverallRowCount();
            _subgridM = puzzle.GetSubgridRowCount();
            _subgridN = puzzle.GetSubgridColumnCount();

        }

        public HashSet<PuzzleCell<int>> GetPuzzle()
        {
            return PuzzleGrid;
        }


        public int SolvePuzzle()
        {
            int statusCode;

            try
            {
                for (int k = 1; k < 9; k++)
                {
                    CheckRows(k);
                    CheckColumns(k);
                    CheckSubGrids(k);
                }
            }
            catch (Exception ex)
            {
                statusCode = StatusCode.EncounteredError;

                return statusCode;
            }

            statusCode = _puzzle.IsSolved() ? StatusCode.Solved : StatusCode.Unsolvable;

            return statusCode;
        }
        

        public void CheckRows(int k)
        {

            for (int i = 0; i < _overallGridRowCount; i++)
            {

                // check in each row subgrid 
                //build region cache
                //For K=1, Single valued-cells are valid combinations

                var combinations = new HashSet<PuzzleCell<int>>();

                var potentialCombinationCandidates = new HashSet<PuzzleCell<int>>();

                var region = new HashSet<PuzzleCell<int>>();


                for (int j = 0; j < _overallGridColumnCount; j++)
                {


                    var cellValues = PuzzleGrid.Where(x => x.X == i && x.Y == j)
                        .Select(x => x.Items)
                        .FirstOrDefault();

                    // add to subgrid hashset
                    region.Add(new PuzzleCell<int>(i, j) { Items = cellValues });
                    

                    if (k == 1)
                    {
                        if (cellValues != null && cellValues.Length == k)
                        {
                            combinations.Add(new PuzzleCell<int>(i, j) { Items = cellValues });
                        }
                    }
                    if (cellValues != null && k!=1 && cellValues.Length <= k)
                    {
                        potentialCombinationCandidates.Add(new PuzzleCell<int>(i, j) { Items = cellValues });
                    }


                }
                if (k > 1)
                {
                    combinations = GetKCombinations(k, potentialCombinationCandidates);
                }

                SieveRegion(k, region, combinations);


            }
        }

        
        public void CheckColumns(int k)
        {
            
            for (int i = 0; i < _overallGridRowCount; i++)
            {

                // check in each column 
                var combinations = new HashSet<PuzzleCell<int>>();
                var potentialCombinationCandidates = new HashSet<PuzzleCell<int>>();
                var region = new HashSet<PuzzleCell<int>>();
                for (int j = 0; j < _overallGridColumnCount; j++)
                {
                    var cellValues = PuzzleGrid.Where(x => x.X == j && x.Y == i)
                        .Select(x => x.Items)
                        .FirstOrDefault();

                    // add to subgrid hashset
                    region.Add(new PuzzleCell<int>(j, i) { Items = cellValues });
                    //get kcomb
                    if (k == 1)
                    {
                        if (cellValues != null && cellValues.Length == k)
                        {
                            combinations.Add(new PuzzleCell<int>(j, i) { Items = cellValues });
                        }
                    }
                    if (cellValues != null && (k!=1 && cellValues.Length <= k))
                    {
                        potentialCombinationCandidates.Add(new PuzzleCell<int>(j, i) { Items = cellValues });
                    }


                }

                if (k > 1)
                {
                    combinations = GetKCombinations(k, potentialCombinationCandidates);
                }

                 SieveRegion(k, region, combinations);
  

            }
        }

        public void CheckSubGrids(int k)
        {

            for (int i = 0; i < _overallGridRowCount/ _subgridM; i++)
            {
                for (int j = 0; j < _overallGridColumnCount/ _subgridN; j++)
                {
                    int row = i * 3;
                    int col = j * 3;

                    // check in 3 by 3 subgrid 
                    var combinations = new HashSet<PuzzleCell<int>>();
                    var potentialCombinationCandidates = new HashSet<PuzzleCell<int>>();
                    var region = new HashSet<PuzzleCell<int>>();


                    //Get K combinations
                    for (int m = 0; m < _overallGridRowCount / _subgridM; m++)
                    {
                        for (int n = 0; n < _overallGridColumnCount / _subgridN; n++)
                        {

                            var n1 = n;
                            var m1 = m;

                            var cellValues = PuzzleGrid.Where(x => x.X == m1 + row && x.Y == n1 + col).Select(x => x.Items).FirstOrDefault();

                            // add to subgrid hashset
                            region.Add(new PuzzleCell<int>(m1 + row, n1 + col) { Items = cellValues });

                            if (k == 1)
                            {
                                if (cellValues != null && cellValues.Length == k)
                                {
                                    combinations.Add(new PuzzleCell<int>(m1 + row, n1 + col) { Items = cellValues });
                                }
                            }
                            //get kcomb
                            if (cellValues != null && k!=1 && cellValues.Length <= k)
                            {
                                potentialCombinationCandidates.Add(new PuzzleCell<int>(m1 + row, n1 + col) { Items = cellValues });
                            }
                        }
                    }

                    if (k > 1)
                    {
                        combinations = GetKCombinations(k, potentialCombinationCandidates);
                    }

                    SieveRegion(k, region, combinations);
                }
            }
        }


        public void SieveRegion(int k, HashSet<PuzzleCell<int>> region, HashSet<PuzzleCell<int>> combinations)
        {
            //Sieve other cells that do not make up combination cells
           
            foreach (var combinationCell in combinations)
            {
                var otherCells = new HashSet<PuzzleCell<int>>();

                if (k == 1)
                {
                    var otherCellsInRegion = region.Where(x => x.Items.SequenceEqual(combinationCell.Items) != true);
                    if (otherCellsInRegion.Count() > 0)
                    {
                        otherCells = Utility.EnumerableToHashSet(otherCellsInRegion);
                    }
                }
                else
                {

                    foreach (var cell in region)
                    {
                        //if cell is not in combination, add to otherCells

                        var exists = combinations.Any(x => x.X == cell.X && x.Y == cell.Y);
                        if (!exists)
                        {
                            otherCells.Add(new PuzzleCell<int>(cell.X, cell.Y) {Items = cell.Items});
                        }
                    }
                }

                foreach (var otherCell in otherCells.ToList())
                {
                    //Get otherCell's values
                    var items = PuzzleGrid.Where(x => x.X == otherCell.X && x.Y == otherCell.Y).Select(x => x.Items).First();
                    int[] oldItems = items;
                    
                    int[] newItems = oldItems.Except(combinationCell.Items).ToArray();

                    //remove old value in puzzle
                    int removedValues = 0;

                    //If otherCell is sievable...i.e contain combination values
                    //Sieve otherCell's values------Remove combination values from otherCell

                    if (!oldItems.SequenceEqual(newItems))
                    {
                        region.RemoveWhere(x => x.X == otherCell.X && x.Y == otherCell.Y);
                        region.Add(new PuzzleCell<int>(otherCell.X, otherCell.Y) { Items = newItems });

                        removedValues = PuzzleGrid.RemoveWhere(x => x.X == otherCell.X && x.Y == otherCell.Y);
                        PuzzleGrid.Add(new PuzzleCell<int>(otherCell.X, otherCell.Y) { Items = newItems });
                    }

                    //if value is removed, restart
                    if (k > 1 && removedValues > 0)
                    {
                        SolvePuzzle();
                        break;
                    }

                }

            }
            
        }

        public HashSet<PuzzleCell<int>> GetKCombinations(int k, HashSet<PuzzleCell<int>> potentialCandidates)
        {
            var combinations = new HashSet<PuzzleCell<int>>();
           
             //possibilities are cells that contain less than K numbers(items)

            foreach (var potentialCandidate in potentialCandidates)
            {
                
                int count = 1;

                int[] pontentialCanadidatesUnion = potentialCandidate.Items;

                var remainingPotentialCandidates = potentialCandidates.Where(x => x.X != potentialCandidate.X || x.Y != potentialCandidate.Y);
                var candidates = new HashSet<PuzzleCell<int>>
                {
                    new PuzzleCell<int>(potentialCandidate.X, potentialCandidate.Y) {Items = potentialCandidate.Items}
                };


                foreach (var remainingPotentialCandidate in remainingPotentialCandidates)
                {
                    int[] unionValues = pontentialCanadidatesUnion.Union(remainingPotentialCandidate.Items).ToArray();

                    //if union results in not more than k elements/items

                    if (unionValues.Length <= k)
                    {
                        pontentialCanadidatesUnion = pontentialCanadidatesUnion.Union(remainingPotentialCandidate.Items).ToArray();
                        candidates.Add(new PuzzleCell<int>(remainingPotentialCandidate.X, remainingPotentialCandidate.Y) { Items = remainingPotentialCandidate.Items });
                        count++;
                    }
                }


                if (candidates.Count == k)
                {

                    combinations = candidates;
                    break;
                }

            }
            

            return combinations;
        }

        
    }
}
