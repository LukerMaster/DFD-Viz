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
            return new GraphvizRunner(new LocalFolderStrategy("win64", null));
        }

        if (_os == PlatformID.Unix)
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            
            var libEnvVar = new Dictionary<string, string>() { { "LD_LIBRARY_PATH", "./unix" } };
            
            return new GraphvizRunner(new LocalFolderStrategy("unix", libEnvVar));
        }

        throw new NotSupportedException("OS not supported.");
    }
}