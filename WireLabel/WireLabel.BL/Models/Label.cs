

namespace WireLabel.BL.Models
{
    public class Label
    {
        public string PartNumber { get; set; }
        public string Type { get; set; }
        public string DrowingNumber { get; set; }
        public List<string> Names { get; set; }
        public List<string> NumberOfVariants { get; set; }
        public string Module { get; set; }
        public List<string> SpliceComposition { get; set; }
    }
}
