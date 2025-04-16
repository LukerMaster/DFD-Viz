using DFD.GraphvizConverter.API;

namespace DFD.GraphvizConverter;

public class GraphvizRunnerFactory
{
    private readonly PlatformID _os;

    public GraphvizRunnerFactory(PlatformID os)
    {
        _os = os;
    }

    public IGraphvizRunner CreateRunner()
    {
        if (_os == PlatformID.Win32NT)
        {
            var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            
            var exeFolder = Path.GetDirectoryName(exePath);

            if (!Directory.Exists(exeFolder))
            {
                throw new DirectoryNotFoundException($"Could not determine folder of executing assembly.");
            }
            
            var dotDirectory = Path.Combine(exeFolder, "graphviz/win64");
            
            var dotFile = Path.Combine(dotDirectory, "dot.exe");
            
            return new GraphvizRunner(new LocalFolderStrategy(dotFile, dotDirectory, null));
        }

        if (_os == PlatformID.Unix)
        {
            return new GraphvizRunner(new SystemwiseStrategy(null));
        }

        throw new NotSupportedException("OS not supported.");
    }
}