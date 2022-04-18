using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireLabel.BL.Models
{
    public class Part
    {
        public string PartNumber { get; set; }
        public string Name { get; set; }
        public string NumberOfVariants { get; set; }
        public string Module { get; set; }
        public string Layer { get; set; }
        public IList<string> KlipComposition { get; set; }
    }
}
