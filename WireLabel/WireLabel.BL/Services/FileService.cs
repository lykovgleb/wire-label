using WireLabel.BL.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace WireLabel.BL.Services;

public class FileService : IFileService
{
    private const string ParsingPathFile = "ParsingPathFile";
    private readonly IConfiguration _configuration;

    public FileService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
   
    public void Parse()
    {
        if (File.Exists(_configuration[ParsingPathFile]))
        {
            Console.WriteLine("Parsing");
        }
    }

    public string SetPath(string path)
    {
        var savePath = _configuration[ParsingPathFile];
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

    public string GetPath()
    {
        if (File.Exists(_configuration[ParsingPathFile]))
        {
            var savePath = _configuration[ParsingPathFile];
            using StreamReader reader = new(savePath);
            string text = reader.ReadToEnd();
            return text;
        }
        else
        {
            return "Path not set";
        }
    }
}