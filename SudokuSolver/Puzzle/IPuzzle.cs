using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public interface IPuzzle
    {
        int GetOverallRowCount();
        int GetOverallColumnCount();
        int GetSubgridColumnCount();
        int GetSubgridRowCount();
        Alphabet GetAlphabet();
        void SetAlphabet(int[] alphabet);
        bool IsValid();
        void InitializeValues(string filePath);
        void ConvertPuzzleXmlToHashSet();
        HashSet<PuzzleCell<int>> GetPuzzleGrid();
        bool IsSolved();

        string GenerateFinalXml();

    }

}
