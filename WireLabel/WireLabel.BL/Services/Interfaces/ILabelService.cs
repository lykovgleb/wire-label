using WireLabel.BL.Models;

namespace WireLabel.BL.Services.Interfaces
{
    public interface ILabelService
    {
        List<List<Label>> GetLabelList(List<List<Part>> partList);
    }
}
