using DFD.ViewModel.Interfaces;
using SFML.Graphics;

namespace DFD.Vizualizer.Model;

public interface IDiagramModel
{
    IVisualGraph NodeGraph { get; set; }
    Sprite BgSprite { get; }
}