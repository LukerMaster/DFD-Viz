using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
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
        RecompileGraph();
    }

    private void OpenFile(string FilePath)
    {

    }

    private void RecompileGraph()
    {
        try
        {
            ErrorTextBlock.Text = String.Empty;
            ViewModel.GraphViewModel.RecompileGraph();
        }
        catch (DfdInterpreterException interpreterException)
        {
            ErrorTextBlock.Text =
                $"{interpreterException.Inner.Message}\nLine: {interpreterException.Line}\nStatement: {interpreterException.Statement}";
        }
    }

    private void RefreshGraph_Clicked(object? sender, RoutedEventArgs e)
    {
        ViewModel.GraphViewModel.RefreshGraph();
    }

    private void CollapseAllNodes_Clicked(object? sender, RoutedEventArgs e)
    {
        foreach (var avaloniaVisualNode in ViewModel.GraphViewModel.VisualGraph.Nodes)
        {
            avaloniaVisualNode.Node.ChildrenCollapsed = true;
        }

        ViewModel.GraphViewModel.RefreshGraph();
    }

    private async void OpenFile_Clicked(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("DFD Graph Files")
                {
                    Patterns = new [] {"*.dfd"}
                }
            },
            Title = "Open DFD File"
        });

        await using var stream = await files[0].OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        var fileContent = await streamReader.ReadToEndAsync();

        ViewModel.DfdCode = fileContent;
        RecompileGraph();
    }

    private void Save_Clicked(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
