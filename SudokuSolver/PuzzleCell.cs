using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class PuzzleCell<T>
    {
        public PuzzleCell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public T[] Items { get; set; }


        // override Equals and GetHashcode...
    }
}
