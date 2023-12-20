using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.Interfaces;

internal interface IVisualGraphGenerationPipeline
{
    IVisualGraph RecompiledGraph { get; }
    IVisualGraph RefreshedGraph { get; }
}