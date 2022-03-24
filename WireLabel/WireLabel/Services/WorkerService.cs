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
        var path = _configuration["SavePath"];
        bool isWorking = true;
        while (isWorking)
        {
            Console.WriteLine("Enter command: ");
            var command = Console.ReadLine();

            if (command == "exit")
                isWorking = false;
        }
    }
}