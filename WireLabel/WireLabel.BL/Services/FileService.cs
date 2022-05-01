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
    private readonly ILabelService _labelService;

    public FileService(IConfiguration configuration, IPartService partService, ILabelService labelService)
    {
        _configuration = configuration;
        _partService = partService;
        _labelService = labelService;
    }

    public string Parse()
    {
        var path = GetPath();
        var allVariants = GetReadingList(path);
        var partList = _partService.GetPartList(allVariants);
        var labelList = _labelService.GetLabelList(partList);
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
    public bool IsPathFileExist()
    {
        return File.Exists(_configuration[ParsingPathFile]);
    }

    private List<List<string>> GetReadingList(string path)
    {
        var allVariants = new List<List<string>>();
        var files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            var variantLines = new List<string>();
            var reader = new StreamReader(file);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                    variantLines.Add(line);
            }
            reader.Close();
            allVariants.Add(variantLines.ToList());
        }
        return allVariants;
    }
}