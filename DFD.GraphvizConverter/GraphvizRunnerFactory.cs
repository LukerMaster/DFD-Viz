namespace DFD.GraphvizConverter;

public class GraphvizRunnerFactory
{
    private readonly PlatformID _os;

    public GraphvizRunnerFactory(PlatformID os)
    {
        _os = os;
    }

    public GraphvizRunner CreateRunner()
    {
        if (_os == PlatformID.Win32NT)
        {
            return new GraphvizRunner(new WindowsProcessStarter());
        }

        if (_os == PlatformID.Unix)
        {
            return new GraphvizRunner(new UbuntuProcessStarter());
        }

        throw new NotSupportedException("OS not supported.");
    }
}