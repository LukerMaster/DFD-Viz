using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public IVisualGraph GraphViewModel;
    public string Greeting => "Welcome to Avalonia!";
    public string TestString => "";
}
