namespace WireLabel.BL.Services.Interfaces;

public interface IFileService
{
    string Parse();
    string SetPath(string path);
    string GetPath();
    bool IsPathFileExist();
}