using Avalonia.Controls;
using Avalonia.Interactivity;
using DFD.AvaloniaEditor.ViewModels;

namespace DFD.AvaloniaEditor.Views;

public partial class MainView : UserControl
{
    private MainViewModel ViewModel => DataContext as MainViewModel;
    public MainView()
    {
        InitializeComponent();
    }

    private void RecompileGraph_Clicked(object? sender, RoutedEventArgs e)
    {
        ViewModel.GraphViewModel.RegenerateGraph();
    }
}
