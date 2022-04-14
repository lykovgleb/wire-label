using Microsoft.Extensions.Configuration;
using WireLabel.BL.Services.Interfaces;
using WireLabel.BL.Models;

namespace WireLabel.BL.Services;

public class FileService : IFileService
{
    private const string success = "success";
    private const string error = "error";
    private const string ParsingPathFile = "ParsingPathFile";

    private readonly IConfiguration _configuration;

    public FileService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Parse()
    {
        if (File.Exists(_configuration[ParsingPathFile]))
        {
            StreamReader pathReader = new(_configuration[ParsingPathFile]);
            string path = pathReader.ReadToEnd();
            pathReader.Close();

            var allVariants = new List<List<string>>();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                var variantLines = new List<string>();
                StreamReader reader = new StreamReader(file);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                        variantLines.Add(line);
                }
                reader.Close();
                allVariants.Add(variantLines.ToList());
                variantLines.Clear();
            }
            GetObjectList(allVariants);
            return success;
        }
        else
        {
            return error;
        }
    }

    private void GetObjectList(List<List<string>> allVariants)
    {
        var objectList = new List<List<Part>>();
        foreach (var variantLines in allVariants)
        {
            var parts = new List<Part>();
            var modulAndNumberOfVariant = variantLines[4][..44].Trim();
            var numberOfVariant = modulAndNumberOfVariant.Substring(modulAndNumberOfVariant.LastIndexOf(' ') + 1);
            var modul = modulAndNumberOfVariant.Substring( 0 , modulAndNumberOfVariant.LastIndexOf(' '));
            foreach (var line in variantLines)
            {
                
                if (line.StartsWith(" .1"))
                {
                    var part = new Part();
                    part.Modul = modul;
                    part.NumberOfVariants = numberOfVariant;
                    part.PartNumber = line.Substring(17, 14).Trim();
                    part.Name = line.Substring(128, 11).Trim();
                    parts.Add(part);
                }
            }
            objectList.Add(parts.ToList());
            parts.Clear();
        }
    }

    public string SetPath(string path)
    {
        var savePath = _configuration[ParsingPathFile];
        if (Directory.Exists(path))
        {
            StreamWriter writer = new(savePath, false);
            writer.Write(path);
            writer.Close();
            return success;
        }
        else
        {
            return error;
        }
    }

    public string GetPath()
    {
        if (File.Exists(_configuration[ParsingPathFile]))
        {
            var savePath = _configuration[ParsingPathFile];
            StreamReader reader = new(savePath);
            string text = reader.ReadToEnd();
            reader.Close();
            return text;
        }
        else
        {
            return error;
        }
    }
}