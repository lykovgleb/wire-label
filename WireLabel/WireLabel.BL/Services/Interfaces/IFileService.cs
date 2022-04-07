namespace WireLabel.BL.Services.Interfaces;

public interface IFileService
{
    void Parse();
    string SetPath(string path);
    string GetPath();
}