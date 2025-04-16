namespace DFD.GraphvizConverter.API;

public interface IGraphvizRunner
{
    public string GetGraphAsJson(string dotCode);
}