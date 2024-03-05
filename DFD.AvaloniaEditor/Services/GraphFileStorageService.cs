using Avalonia.Controls;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using System;
using DFD.AvaloniaEditor.Assets;

namespace DFD.AvaloniaEditor.Services;

public class GraphFileStorageService : IFileStorageService
{
    private IStorageProvider _storageProvider;

    public GraphFileStorageService(IStorageProvider storageProvider)
    {
        _storageProvider = storageProvider;
    }

    public GraphFileStorageService()
    {
    }

    public void SetStorageProvider(IStorageProvider provider)
    {
        _storageProvider = provider;
    }

    public async Task<string> OpenFileAsync(string filePath)
    {
        var file = await _storageProvider.TryGetFileFromPathAsync(filePath);
        await using var stream = await file.OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        var fileContent = await streamReader.ReadToEndAsync();

        return fileContent;
    }

    public async Task<IStorageFile?> PickFileAsync()
    {
        var files = await _storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType(Lang.DFD_Graph_Files)
                {
                    Patterns = new [] {"*.dfd"}
                }
            },
            Title = Lang.Open_DFD_File
        });
        if (files.Count > 0)
        {
            return files[0];
        }
        return null;
    }

    public async void SaveFileAsync(string filePath, string content)
    {
        var file = await _storageProvider.TryGetFileFromPathAsync(filePath);
        await using var stream = await file.OpenWriteAsync();
        await using var streamReader = new StreamWriter(stream);
        await streamReader.WriteAsync(content);
    }

    public async Task<string?> SaveFileAsync(string content)
    {
        var file = await _storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            DefaultExtension = "dfd",
            ShowOverwritePrompt = true,
            SuggestedFileName = "Graph-" + DateTime.Now.ToFileTimeUtc(),
            Title = Lang.Save_Graph_As
        });

        if (file is not null)
        {
            await using var stream = await file.OpenWriteAsync();
            await using var streamReader = new StreamWriter(stream);
            await streamReader.WriteAsync(content);
            return file.TryGetLocalPath();
        }

        return null;
    }
}