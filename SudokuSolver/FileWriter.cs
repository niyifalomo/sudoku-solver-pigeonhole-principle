using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class FileWriter
    {

        public bool Write(string filePath, string data)
        {
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (fs.CanWrite)
                {
                    using (var fw = new StreamWriter(fs))
                    {
                        fw.WriteLine(data);
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsReadable(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (fs.CanRead)
                        return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool IsWritable(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {

                    if (fs.CanWrite)
                        return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }


    }
}
