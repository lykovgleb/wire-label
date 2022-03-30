namespace WireLabel.Services.Interfaces;

public interface IFileService
{
    void Parse();
    string SetPath(string path, string savePath);
    string ShowPath(string savePath);
}