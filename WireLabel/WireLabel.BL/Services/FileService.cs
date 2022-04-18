using Microsoft.Extensions.Configuration;
using WireLabel.BL.Services.Interfaces;

namespace WireLabel.BL.Services;

public class FileService : IFileService
{
    private const string Success = "success";
    private const string Error = "error";
    private const string ParsingPathFile = "ParsingPathFile";

    private readonly IConfiguration _configuration;
    private readonly IPartService _partService;

    public FileService(IConfiguration configuration, IPartService partService)
    {
        _configuration = configuration;
        _partService = partService;
    }

    public string Parse()
    {

        var path = GetPath();
        var allVariants = _partService.GetReadingList(path);
        var partList = _partService.GetPartList(allVariants);
        return Success;

    }

    public string SetPath(string path)
    {
        var savePath = _configuration[ParsingPathFile];
        if (Directory.Exists(path))
        {
            StreamWriter writer = new(savePath, false);
            writer.Write(path);
            writer.Close();
            return Success;
        }
        else
        {
            return Error;
        }
    }

    public string GetPath()
    {
        var savePath = _configuration[ParsingPathFile];
        StreamReader reader = new(savePath);
        string path = reader.ReadToEnd();
        reader.Close();
        return path;
    }
}