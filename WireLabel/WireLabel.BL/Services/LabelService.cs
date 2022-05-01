using WireLabel.BL.Models;
using WireLabel.BL.Services.Interfaces;

namespace WireLabel.BL.Services;

public class LabelService : ILabelService
{
    public List<List<Label>> GetLabelList(List<List<Part>> partList)
    {
        var labelsList = new List<List<Label>>();
        while (partList.Count() > 0)
        {
            var allVariantsInModuleList = partList
                .Where(p => p.Any(x => x.Module.Equals(partList.First().First().Module)))
                .SelectMany(x => x)
                .ToList();
            var LabelsForOneWorkplace = new List<Label>();
            for (int i = 0; i < allVariantsInModuleList.Count; i++)
            {
                var label = new Label
                {
                    PartNumber = allVariantsInModuleList[i].PartNumber,
                    Module = allVariantsInModuleList[i].Module,
                    SpliceComposition = allVariantsInModuleList[i].SpliceComposition,
                    NumberOfVariants = new List<string>(),
                    Names = new List<string>()
                };
                label.Names.Add(allVariantsInModuleList[i].Name);
                label.NumberOfVariants.Add(allVariantsInModuleList[i].NumberOfVariant);
                for (int j = i + 1; j < allVariantsInModuleList.Count; j++)
                {
                    if (label.PartNumber != allVariantsInModuleList[j].PartNumber)
                        continue;
                    if (!label.NumberOfVariants.Contains(allVariantsInModuleList[j].NumberOfVariant))
                        label.NumberOfVariants.Add(allVariantsInModuleList[j].NumberOfVariant);
                    if (!label.Names.Contains(allVariantsInModuleList[j].Name))
                        label.Names.Add(allVariantsInModuleList[j].Name);
                    allVariantsInModuleList.RemoveAt(j);
                    j--;
                }
                label.NumberOfVariants.Sort();
                LabelsForOneWorkplace.Add(label);
            }
            //ToDo: Replace to excel service
            labelsList.Add(LabelsForOneWorkplace.ToList());
            partList.RemoveAll(p => p.Any(x => x.Module.Equals(partList.First().First().Module)));
        }
        return labelsList;
    }
}
