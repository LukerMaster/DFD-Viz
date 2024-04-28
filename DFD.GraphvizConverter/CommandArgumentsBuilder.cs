using System.Text;

namespace DFD.GraphvizConverter;

internal class CommandArgumentsBuilder
{
    public static string BuildCommandLineArguments(string layout, string format,
        params string[] extraCommandLineFlags)
    {
        var argumentsBuilder = new StringBuilder();

        if (layout != null)
        {
            argumentsBuilder.Append(" -K").Append(layout);
        }

        if (format != null)
        {
            argumentsBuilder.Append(" -T").Append(format);
        }

        foreach (string extraFlag in extraCommandLineFlags)
        {
            argumentsBuilder.Append(' ').Append(extraFlag);
        }
        

        return argumentsBuilder.ToString();
    }
}