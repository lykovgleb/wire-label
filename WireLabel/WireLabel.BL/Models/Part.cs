

namespace WireLabel.BL.Models
{
    public class Part
    {
        public string PartNumber { get; set; }
        public string Name { get; set; }
        public string NumberOfVariant { get; set; }
        public string Module { get; set; }
        public IList<string> SpliceComposition { get; set; }
    }
}
