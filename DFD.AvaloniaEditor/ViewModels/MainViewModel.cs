using System.IO;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.Services;
using DFD.GraphConverter.Interfaces;
using DFD.Parsing.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    public MainViewModel(IVisualGraphProvider visualGraphProvider, IDfdCodeStringProvider? dfdCode)
    {
        if (dfdCode is not null)
        {
            _dfdProvider = dfdCode;
        }
        GraphViewModel = new DiagramViewModel(visualGraphProvider);
    }

    public MainViewModel()
    {
        // Design only
    }

    IDfdCodeStringProvider _dfdProvider = new DfdCodeStringProvider();

    public DiagramViewModel GraphViewModel { get; }
    public string Greeting => "Welcome to Avalonia!";
    public string DfdCode => _dfdProvider.DfdCode;
}
