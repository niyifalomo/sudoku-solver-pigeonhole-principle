using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class StatusCode
    {
        public const int Solved = 0;
        public const int NoFurtherProgress = 1;
        public const int Unsolvable = 2;
        public const int EncounteredError = 3;

    }
}
