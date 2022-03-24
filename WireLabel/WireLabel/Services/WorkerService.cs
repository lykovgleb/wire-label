using Microsoft.Extensions.Configuration;

namespace WireLabel.Services;

public class WorkerService
{
    private readonly IConfiguration _configuration;

    public WorkerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Start()
    {
        var path = _configuration["SavePath"];

        Console.WriteLine(path);
    }
}