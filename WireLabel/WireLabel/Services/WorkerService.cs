using WireLabel.BL.Services.Interfaces;

namespace WireLabel.Services;

public class WorkerService
{
    
    private const string Exit = "exit";
    private const string SetPath = "set folder path";
    private const string ShowPath = "show folder path";
    private const string Parse = "parse";
    private readonly IFileService _fileService;

    public WorkerService(IFileService fileService)
    {
        _fileService = fileService;
    }

    public void ReceiveCommand()
    {
        Console.WriteLine("commands:\n{0}\n{1}\n{2}\n{3}", Exit, SetPath, ShowPath, Parse);
        if (_fileService.IsPathFileExist())
        {
            Console.WriteLine("Path: {0}", _fileService.GetPath());
        }
        else
        {
            Console.WriteLine("Enter path:");
            var path = Console.ReadLine();
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path))
                Console.WriteLine(_fileService.SetPath(path));
        }
        bool isWorking = true;

        while (isWorking)
        { 
            Console.WriteLine("Enter command:");
            var userCommand = Console.ReadLine();

            if (string.IsNullOrEmpty(userCommand) || string.IsNullOrWhiteSpace(userCommand))
            {
                Console.WriteLine("Invalid command");
                continue;
            }

            userCommand = userCommand.ToLower();
            switch (userCommand)
            {
                    case Exit:
                        isWorking = false;
                        break;

                    case SetPath:
                        Console.WriteLine("Enter path:");
                        var path = Console.ReadLine();
                        if (!string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path))
                            Console.WriteLine(_fileService.SetPath(path));
                        break;

                    case ShowPath:
                        Console.WriteLine(_fileService.GetPath());
                        break;

                    case Parse:
                    Console.WriteLine(_fileService.Parse());
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
            }    
        }
    }
}