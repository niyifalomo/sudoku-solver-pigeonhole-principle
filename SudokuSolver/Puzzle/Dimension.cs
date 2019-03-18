using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Dimension
    {
        public int RowSize;
        public int ColSize;

        public Dimension(int x, int y)
        {
            RowSize = x;
            ColSize = y;
        }
    }
}
