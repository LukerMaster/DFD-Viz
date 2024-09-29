using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.Services;

namespace DFD.AvaloniaEditor.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    private IFileStorageService _storageService;

    IDfdCodeStringProvider _dfdCodeProvider = new DfdCodeStringProvider();
    public ILanguageService LanguageService { get; }
    public MainViewModel(IVisualGraphGenerationPipeline visualGraphGenerationPipeline,
        IDfdCodeStringProvider? dfdCode,
        IFileStorageService storageService, ILanguageService languageService)
    {
        if (dfdCode is not null)
        {
            _dfdCodeProvider = dfdCode;
        }

        GraphViewModel = new DiagramViewModel(visualGraphGenerationPipeline);
        _storageService = storageService;
        LanguageService = languageService;

        
    }

    public MainViewModel()
    {
        // Design only
    }


    public DiagramViewModel GraphViewModel { get; }
    public string Greeting => "Welcome to Avalonia!";

    public string DfdCode
    {
        get => _dfdCodeProvider.DfdCode;
        set
        {
            _dfdCodeProvider.DfdCode = value;
            OnPropertyChanged();
        }
    }

    public string? CurrentlyOpenFilePath
    {
        get => _dfdCodeProvider.FilePath;
        set
        {
            if (value == _dfdCodeProvider.FilePath) return;
            _dfdCodeProvider.FilePath = value;
            OnPropertyChanged();
        }
    }

    public async Task OpenGraphFileAsync()
    {
        var file = await _storageService.PickFileAsync();
        if (file is not null)
        {
            var content = await _storageService.OpenFileAsync(file.TryGetLocalPath());
            CurrentlyOpenFilePath = file.Name;
            DfdCode = content;
            GraphViewModel.RecompileGraph();
        }
    }

    public async void SaveGraphFileAsync()
    {
        _storageService.SaveFileAsync(CurrentlyOpenFilePath, DfdCode);
    }

    public async void SaveGraphFileAsAsync()
    {
        CurrentlyOpenFilePath = await _storageService.SaveFileAsync(DfdCode);
    }

    public async void ExportGraphAsAsync(Bitmap bitmap)
    {
        _storageService.ExportFileAsync(bitmap);
    }
}