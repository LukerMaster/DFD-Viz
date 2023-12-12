using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.Interfaces;

internal interface IVisualGraphProvider
{
    IVisualGraph? VisualGraph { get; }
}