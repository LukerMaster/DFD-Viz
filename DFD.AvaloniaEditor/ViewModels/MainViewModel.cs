using System.IO;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel(string inputFilePath)
    {
        _dfdFileContents = File.ReadAllText(inputFilePath);
    }

    public MainViewModel()
    {

    }

    string _dfdFileContents = string.Empty;

    public IVisualGraph GraphViewModel;
    public string Greeting => "Welcome to Avalonia!";
    public string DfdCode => _dfdFileContents;
}
