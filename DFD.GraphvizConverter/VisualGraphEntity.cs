using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.ViewModel;

public class VisualGraphEntity : IVisualGraphEntity
{
    public IGraphEntity Entity { get; set; }
    public ITransformableSymbol TransformableSymbol { get; set; }
}