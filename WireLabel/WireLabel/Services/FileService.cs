using WireLabel.Services.Interfaces;

namespace WireLabel.Services;

public class FileService : IFileService
{
    public void Parse()
    {
        Console.WriteLine("Parsing");
    }
}