using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace DFD.GraphvizConverter;

internal class UbuntuProcessStarter : IGraphvizProcessStarter
{
    public byte[] LayoutAndRender(string graph, string layoutAlgorithm, string outputFormat,
        params string[] extraCommandLineFlags)
    {
        try
        {
            StandardProcessStarter s = new StandardProcessStarter("dot", null);
            return s.Render(graph, layoutAlgorithm, outputFormat, extraCommandLineFlags);
        }
        catch (Win32Exception e)
        {
            // If file not found - basically - error codes are weird
            if (e.ErrorCode == -2147467259 && e.NativeErrorCode == 2)
                throw new GraphvizNotFoundException("sudo apt install graphviz");
            throw;
        }
    }
}