using WireLabel.BL.Models;

namespace WireLabel.BL.Services.Interfaces
{
    public interface IExcelService
    {
        public void MakeExcelFile(List<Label> labelsForOneWorkplace, string path);
    }
}
