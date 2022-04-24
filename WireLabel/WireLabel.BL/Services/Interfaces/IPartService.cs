using WireLabel.BL.Models;

namespace WireLabel.BL.Services.Interfaces;

public interface IPartService
{
    IList<List<Part>> GetPartList(IList<List<string>> allVariants);

}

