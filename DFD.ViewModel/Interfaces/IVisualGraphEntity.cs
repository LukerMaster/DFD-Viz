using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces
{
    public interface IVisualGraphEntity
    {
        public IGraphEntity Entity { get; set; }
        public ITransformableSymbol TransformableSymbol { get; set; }
    }
}