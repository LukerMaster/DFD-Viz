using System.Text.Json;
using DataStructure.NamedTree;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter;

public class JsonToGraphParser
{
    public ITreeNode<IVisualGraphNode> CreateGraphFrom(string json)
    {
        JsonDocument document = JsonDocument.Parse(json);
        return null;
    }
}