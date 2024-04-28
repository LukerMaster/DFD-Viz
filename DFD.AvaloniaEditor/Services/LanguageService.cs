using System;
using System.Globalization;
using System.IO;
using Avalonia;
using DFD.AvaloniaEditor.Assets;

namespace DFD.AvaloniaEditor.Services;

public class LanguageService : ILanguageService
{
    private readonly string _fileName = "LangSettings.txt";
    private readonly string _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private string FilePath => Path.Combine(FolderPath, _fileName);
    private string FolderPath => Path.Combine(_appData, "DFD-Viz");

    public LanguageService(string fileName)
    {
        _fileName = fileName;
    }

    public CultureInfo CultureInfo
    {
        get
        {
            if (File.Exists(FilePath))
            {
                return new CultureInfo(File.ReadAllText(FilePath));
            }
            return CultureInfo.CurrentCulture;
        }
        set
        {
            Directory.CreateDirectory(FolderPath);
            File.WriteAllText(FilePath, value.Name);
            Lang.Culture = value;
        }
    }
}