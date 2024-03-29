﻿using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DFD.AvaloniaEditor.Services;
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
        await ViewModel.OpenGraphFileAsync();
    }

    private void Save_Clicked(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(ViewModel.CurrentlyOpenFilePath))
        {
            SaveAs_Clicked(this, e);
        }
        else
        {
            ViewModel.SaveGraphFileAsync();
        }
    }

    private void SaveAs_Clicked(object? sender, RoutedEventArgs e)
    {
        ViewModel.SaveGraphFileAsAsync();
    }

    private void ToggleTopNodeVisibility_Clicked(object? sender, RoutedEventArgs e)
    {
        ViewModel.GraphViewModel.ToggleVisibility("top");
    }

    private void ExportAs_Clicked(object? sender, RoutedEventArgs e)
    {
        var filePath = "C:\\Users\\lacho\\Documents\\diagramxd.png";
        var panel = this.Find<DiagramDrawControl>("DrawControl").Find<Panel>("MainPanel");
        var bitmap = new RenderTargetBitmap(new PixelSize((int)panel.Bounds.Width, (int)panel.Bounds.Height));
        //ViewModel.ExportGraphAsAsync();
        using (var context = bitmap.CreateDrawingContext())
        {
 
            bitmap.Render(panel);
        }

        // Save the RenderTargetBitmap to a file.
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            // Encode the RenderTargetBitmap to a PNG image.
             bitmap.Save(fileStream);
        }
    }
}
