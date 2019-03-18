using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public interface IPigeonHole
    {
        int SolvePuzzle();
        void CheckRows(int k);
        void CheckColumns(int k);
        void CheckSubGrids(int k);
        void SieveRegion(int k, HashSet<PuzzleCell<int>> region, HashSet<PuzzleCell<int>> combinations);
        HashSet<PuzzleCell<int>> GetPuzzle();
    }
}
