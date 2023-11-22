using DFD.GraphConverter;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;
using DFD.Interpreter;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer.Model;

class DiagramModel : IDiagramModel
{
    public IVisualGraph NodeGraph { get; set; }
    public Sprite BgSprite { get; set; }
}