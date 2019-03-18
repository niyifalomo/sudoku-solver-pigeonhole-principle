using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SudokuSolver
{
    [XmlRoot(ElementName = "cell")]
    public class Cell
    {
        [XmlElement(ElementName = "row")]
        public string Row { get; set; }

        [XmlElement(ElementName = "col")]
        public string Col { get; set; }

        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "initial-values")]
    public class Initialvalues
    {
        [XmlElement(ElementName = "cell")]
        public List<Cell> Cell { get; set; }
    }

    [XmlRoot(ElementName = "puzzle")]
    public class PuzzleXml
    {
        [XmlElement(ElementName = "initial-values")]
        public Initialvalues Initialvalues { get; set; }
    }
}
