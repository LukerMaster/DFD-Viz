using System.Diagnostics;

namespace DFD.GraphvizConverter;

public class UnixExecutablePreparer
{
    public void PrepareEnvironment(string workingDirectory, string exeName)
    {
        var exePath = System.IO.Path.Combine(workingDirectory, exeName);
        var chmod = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/chmod",
                Arguments = $"+x \"{exePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        chmod.Start();
        chmod.WaitForExit();
        if (chmod.ExitCode != 0)
        {
            throw new Exception($"Failed to set execute permission on dot executable: {chmod.StandardError.ReadToEnd()}");
        }
    }
}