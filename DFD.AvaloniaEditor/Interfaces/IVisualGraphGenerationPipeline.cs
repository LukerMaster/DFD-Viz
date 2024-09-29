using System;
using DFD.DataStructures.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.Interfaces;

internal interface IVisualGraphGenerationPipeline
{
    IVisualGraph RecompiledGraph { get; }
    IVisualGraph RefreshedGraph { get; }
    void ExecuteOnNode(string nodeName, Action<ICollapsibleNodeData> command);
}