using DFD.AvaloniaEditor.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

internal class DiagramViewModel : ViewModelBase
{
    public DiagramViewModel(IVisualGraphProvider diagramProvider)
    {
        Provider = diagramProvider;
    }

    public IVisualGraphProvider Provider { get; }
}