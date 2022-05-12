using WireLabel.BL.Models;

namespace WireLabel.BL.Services.Interfaces
{
    public interface ILabelService
    {
        void GetLabels(List<List<Part>> partList, string path);
    }
}
