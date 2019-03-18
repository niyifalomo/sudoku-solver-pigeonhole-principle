using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Alphabet
    {
        public int[] Elements;

        public Alphabet(int[] elements)
        {
            Elements = elements;
        }


        public int[] GetAllElements()
        {
            return Elements;
        }

        
    }
}
