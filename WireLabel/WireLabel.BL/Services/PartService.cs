using WireLabel.BL.Models;
using WireLabel.BL.Services.Interfaces;

namespace WireLabel.BL.Services;

public class PartService : IPartService
{
    public List<List<string>> GetReadingList(string path)
    {
        var allVariants = new List<List<string>>();
        var files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            var variantLines = new List<string>();
            var reader = new StreamReader(file);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                    variantLines.Add(line);
            }
            reader.Close();
            allVariants.Add(variantLines.ToList());
        }
        return allVariants;
    }

    public List<List<Part>> GetPartList(List<List<string>> allVariants)
    {
        var partList = new List<List<Part>>();
        foreach (var variantLines in allVariants)
        {
            var parts = new List<Part>();
            var moduleAndNumberOfVariant = variantLines[4][..44].Trim();
            var numberOfVariant = moduleAndNumberOfVariant[(moduleAndNumberOfVariant.LastIndexOf(' ') + 1)..];
            var module = moduleAndNumberOfVariant[..moduleAndNumberOfVariant.LastIndexOf(' ')];
            for (int i = 0; i < variantLines.Count; i++)
            {
                if (variantLines[i].StartsWith(" .1"))
                {
                    var part = new Part
                    {
                        Module = module,
                        NumberOfVariants = numberOfVariant,
                        PartNumber = variantLines[i].Substring(17, 14).Trim(),
                        Name = variantLines[i].Substring(128, 11).Trim()
                    };
                    if (part.PartNumber.StartsWith("5"))
                    {
                        part.KlipComposition = new List<string>();
                        for (int j = i+1; j < variantLines.Count; j++)
                        {
                            if (variantLines[j].StartsWith(" ..2"))
                            {                             
                                var klipwire = variantLines[j].Substring(128, 11).Trim();
                                if (!string.IsNullOrEmpty(klipwire) && !string.IsNullOrWhiteSpace(klipwire))
                                    part.KlipComposition.Add(klipwire);
                            }
                            if (variantLines[j].StartsWith(" .1"))
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

