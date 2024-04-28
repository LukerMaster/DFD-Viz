using System.Globalization;

namespace DFD.AvaloniaEditor.Services;

public interface ILanguageService
{
    public CultureInfo CultureInfo { get; set; }
}