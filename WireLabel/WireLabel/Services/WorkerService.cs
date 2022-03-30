using Microsoft.Extensions.Configuration;
using WireLabel.Services.Interfaces;

namespace WireLabel.Services;

public class WorkerService
{
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;

    public WorkerService(IConfiguration configuration, IFileService fileService)
    {
        _configuration = configuration;
        _fileService = fileService;
    }

    public void Start()
    {
        var savePath = _configuration["SavePath"];
        bool isWorking = true;
        while (isWorking)
        { 
            Console.WriteLine("Enter command:");
            var command = Console.ReadLine();
            
            if (command is not null) //Почему выполняется при пустой строке?
            {
                command = command.ToLower();
                switch (command)
                {
                    case "exit":
                        isWorking = false;
                        break;

                    case "set folder path":
                        Console.WriteLine("Enter path:");
                        var path = Console.ReadLine();
                        if (path is not null)
                            Console.WriteLine(_fileService.SetPath(path, savePath));
                        break;

                    case "show folder path":
                        Console.WriteLine(_fileService.ShowPath(savePath));
                        break;

                    case "parse":
                        _fileService.Parse();
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }
    }
}