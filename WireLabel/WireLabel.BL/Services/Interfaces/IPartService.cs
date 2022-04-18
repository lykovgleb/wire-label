using WireLabel.BL.Models;

namespace WireLabel.BL.Services.Interfaces;

public interface IPartService
{
    List<List<string>> GetReadingList(string path);
    List<List<Part>> GetPartList(List<List<string>> allVariants);

}

