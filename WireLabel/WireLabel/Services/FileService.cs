using WireLabel.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace WireLabel.Services;

public class FileService : IFileService
{
    public void Parse()
    {
        Console.WriteLine("Parsing");
    }

    public string SetPath(string path, string savePath)
    {
        if (Directory.Exists(path))
        {
            using StreamWriter writer = new(savePath, false);
            writer.Write(path);
            return "Path set";
        }
        else
        {
            return "Folder not found";
        }
    }

    public string ShowPath(string savePath)
    {
        using StreamReader reader = new(savePath);
        string text = reader.ReadToEnd();
        return text;
    }        
}