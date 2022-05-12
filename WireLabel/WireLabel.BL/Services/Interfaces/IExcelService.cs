using WireLabel.BL.Models;

namespace WireLabel.BL.Services.Interfaces
{
    public interface IExcelService
    {
        public void MadeExcelFile(List<Label> labelsForOneWorkplace, string path);
    }
}
