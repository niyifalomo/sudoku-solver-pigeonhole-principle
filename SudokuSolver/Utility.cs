using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class Utility
    {
        public static HashSet<T> EnumerableToHashSet<T>(IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }
    }
}
