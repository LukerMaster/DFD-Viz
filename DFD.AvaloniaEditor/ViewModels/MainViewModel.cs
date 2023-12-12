using System.IO;
using DFD.AvaloniaEditor.Interfaces;
using DFD.GraphConverter.Interfaces;
using DFD.Parsing.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    private IVisualGraphProvider _graphProvider;
    public MainViewModel(IVisualGraphProvider visualGraphProvider, string? inputFilePath = null)
    {
        _graphProvider = visualGraphProvider;

        if (inputFilePath is not null)
        {
            _dfdFileContents = File.ReadAllText(inputFilePath);
        }
            
    }

    public MainViewModel()
    {

    }

    string _dfdFileContents = string.Empty;

    public DiagramViewModel GraphViewModel;
    public string Greeting => "Welcome to Avalonia!";
    public string DfdCode => _dfdFileContents;
}
