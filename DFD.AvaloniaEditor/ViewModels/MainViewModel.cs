using System.IO;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.Services;
using DFD.GraphConverter.Interfaces;
using DFD.Parsing.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    public MainViewModel(IVisualGraphProvider visualGraphProvider, IDfdCodeStringProvider? dfdCodeCode)
    {
        if (dfdCodeCode is not null)
        {
            _dfdCodeProvider = dfdCodeCode;
        }
        GraphViewModel = new DiagramViewModel(visualGraphProvider);
    }

    public MainViewModel()
    {
        // Design only
    }

    IDfdCodeStringProvider _dfdCodeProvider = new DfdCodeStringProvider();
    public DiagramViewModel GraphViewModel { get; }
    public string Greeting => "Welcome to Avalonia!";
    public string DfdCode
    {
        get => _dfdCodeProvider.DfdCode;
        set => _dfdCodeProvider.DfdCode = value;
    }
}
