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

            if (command == "set folder path")
            {
                Console.WriteLine("Enter path");
                path = Console.ReadLine();

                using (StreamWriter writer = new StreamWriter("MyDir\\path.txt", false))
                {
                    writer.WriteLine(path);
                }
            }

            if (command == "Show folder path")
            {
                using (StreamReader reader = new StreamReader("MyDir\\path.txt"))
                {
                    string text = reader.ReadToEnd();
                    Console.WriteLine(text);
                }
            }

            if (command == "Parse")
            {
                FileService fileService = new FileService();
                fileService.Parse();
            }
        }
    }
}