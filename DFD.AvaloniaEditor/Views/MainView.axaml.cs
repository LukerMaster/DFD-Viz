using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DFD.AvaloniaEditor.ViewModels;
using DFD.Parsing;

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
        try
        {
            ErrorTextBlock.Text = String.Empty;
            ViewModel.GraphViewModel.RecompileGraph();
        }
        catch (DfdInterpreterException interpreterException)
        {
            ErrorTextBlock.Text = $"{interpreterException.Inner.Message}\nLine: {interpreterException.Line}\nStatement: {interpreterException.Statement}";
        }
    }
}
