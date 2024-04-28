using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace DFD.AvaloniaEditor.Services;

public interface IFileStorageService
{
    Task<string> OpenFileAsync(string filePath);
    Task<IStorageFile?> PickFileAsync();
    void SaveFileAsync(string filePath, string content);
    Task<string?> SaveFileAsync(string content);
    void ExportFileAsync(Bitmap image);
}