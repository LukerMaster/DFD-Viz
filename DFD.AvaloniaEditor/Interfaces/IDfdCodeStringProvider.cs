namespace DFD.AvaloniaEditor.Interfaces;

public interface IDfdCodeStringProvider
{
    string? FilePath { get; set; }
    string DfdCode { get; set; }
}