using DFD.Model.Interfaces;

namespace DFD.Interpreter;

internal class ParserRunData
{
    public IGraphEntity CurrentScopeNode { get; set; }
    public int CurrentScopeLevel { get; set; }

    public ParserRunData()
    {
        CurrentScopeNode = new Process()
        {
            EntityName = "top",
            DisplayedText = "System"
        };
    }

    public void RaiseScope(IGraphEntity newChild)
    {
        CurrentScopeNode = newChild;
        CurrentScopeLevel++;
    }

    public void LowerScope()
    {
        CurrentScopeNode = CurrentScopeNode.Parent;
        CurrentScopeLevel--;
    }

    public void LowerScopeTo(int level)
    {
        while (CurrentScopeLevel > level)
        {
            LowerScope();
        }
    }
}