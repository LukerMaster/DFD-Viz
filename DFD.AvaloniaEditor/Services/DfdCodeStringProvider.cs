using DFD.AvaloniaEditor.Interfaces;

namespace DFD.AvaloniaEditor.Services;

public class DfdCodeStringProvider : IDfdCodeStringProvider
{
    public string? FilePath { get; set; }
    public string DfdCode { get; set; }
}