using WireLabel.BL.Models;
using WireLabel.BL.Services.Interfaces;

namespace WireLabel.BL.Services;

public class PartService : IPartService
{
    private const string FirstLayer = " .1";
    private const string SecondLayer = " ..2";
    private const char SpliceNumberStartWith = '5';
    private const int PartNumberIndex = 17;
    private const int PartNumberLength = 14;
    private const int TypeIndex = 31;
    private const int TypeLength = 29;
    private const int NameIndex = 128;
    private const int NameLength = 11;
    private const int DrowingNumberIndex = 31;
    private const int DrowingNumberLength = 29;
    private const int NumberOfLineWithModule = 4;
    private const int ModuleLength = 44;

    public List<List<Part>> GetPartList(List<List<string>> allVariants)
    {
        var partList = new List<List<Part>>();
        foreach (var variantLines in allVariants)
        {
            var parts = new List<Part>();
            var moduleAndNumberOfVariant = variantLines[NumberOfLineWithModule][..ModuleLength].Trim();
            var numberOfVariant = moduleAndNumberOfVariant[(moduleAndNumberOfVariant.LastIndexOf(' ') + 1)..];
            var module = moduleAndNumberOfVariant[..moduleAndNumberOfVariant.LastIndexOf(' ')];
            for (int i = 0; i < variantLines.Count; i++)
            {
                if (variantLines[i].StartsWith(FirstLayer))
                {
                    var part = new Part
                    {
                        Module = module,
                        NumberOfVariant = numberOfVariant,
                        PartNumber = variantLines[i].Substring(PartNumberIndex, PartNumberLength).Trim(),
                        Type = variantLines[i].Substring(TypeIndex, TypeLength).Trim(),
                        DrowingNumber = variantLines[i+1].Trim(),
                        Name = variantLines[i].Substring(NameIndex, NameLength).Trim()
                    };
                    if (part.PartNumber.StartsWith(SpliceNumberStartWith))
                    {
                        part.SpliceComposition = new List<string>();
                        for (int j = i + 1; j < variantLines.Count; j++)
                        {
                            if (variantLines[j].StartsWith(SecondLayer))
                            {
                                var spliceWire = variantLines[j].Substring(NameIndex, NameLength).Trim();
                                if (!string.IsNullOrEmpty(spliceWire) && !string.IsNullOrWhiteSpace(spliceWire))
                                    part.SpliceComposition.Add(spliceWire);
                            }
                            if (variantLines[j].StartsWith(FirstLayer))
                            {
                                i = j - 1;
                                break;
                            }
                        }
                    }
                    parts.Add(part);
                }
            }
            partList.Add(parts.ToList());
        }
        return partList;
    }
}

