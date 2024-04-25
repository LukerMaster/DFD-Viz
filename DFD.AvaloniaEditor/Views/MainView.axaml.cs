using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using DFD.AvaloniaEditor.Assets;
using DFD.AvaloniaEditor.Services;
using DFD.AvaloniaEditor.ViewModels;
using DFD.Parsing;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

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
        ViewModel.GraphViewModel.ReconstructGraph();
    }

    private void CollapseAllNodes_Clicked(object? sender, RoutedEventArgs e)
    {
        foreach (var avaloniaVisualNode in ViewModel.GraphViewModel.VisualGraph.Nodes)
        {
            avaloniaVisualNode.Node.Collapsed = true;
        }

        ViewModel.GraphViewModel.ReconstructGraph();
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
        if (ViewModel.GraphViewModel.VisualGraph.Size.SquaredLength > 0)
        {
            var imageScale = 14;
            var imageDPI = 1200;

            float maxSizeCapPx = 96000;

            var panel = this.Find<DiagramDrawControl>("DrawControl").Find<Panel>("MainPanel");

            var finalSize = new PixelSize((int)panel.Bounds.Width * imageScale, (int)panel.Bounds.Height * imageScale);
            var finalDPI = Vector.One * imageDPI;

            if (finalSize.Width > maxSizeCapPx)
            {
                finalDPI *= maxSizeCapPx / finalSize.Width;
                finalSize = new PixelSize((int)maxSizeCapPx, (int)(finalSize.Height * (maxSizeCapPx / finalSize.Width)));
            }
            if (finalSize.Height > maxSizeCapPx)
            {
                finalDPI *= maxSizeCapPx / finalSize.Height;
                finalSize = new PixelSize((int)(finalSize.Width * (maxSizeCapPx / finalSize.Height)), (int)maxSizeCapPx);
            }

            var bitmap = new RenderTargetBitmap(finalSize, finalDPI);
            using (bitmap.CreateDrawingContext())
            {
                bitmap.Render(panel);
            }

            ViewModel.ExportGraphAsAsync(bitmap);
        }
    }

    private void ToLightTheme_Clicked(object? sender, RoutedEventArgs e)
    {
        TopLevel.GetTopLevel(this).RequestedThemeVariant = ThemeVariant.Light;
    }

    private void ToDarkTheme_Clicked(object? sender, RoutedEventArgs e)
    {
        TopLevel.GetTopLevel(this).RequestedThemeVariant = ThemeVariant.Dark;
    }

    private void ToSystemTheme_Clicked(object? sender, RoutedEventArgs e)
    {
        TopLevel.GetTopLevel(this).RequestedThemeVariant = ThemeVariant.Default;
    }

    private async void Click_English(object? sender, RoutedEventArgs e)
    {
        ViewModel.LanguageService.CultureInfo = CultureInfo.GetCultureInfo("en-US");
        var box = MessageBoxManager.GetMessageBoxStandard(Lang.Info, Lang.Application_Needs_Restart, ButtonEnum.Ok);
        await box.ShowAsync();
    }

    private async void Click_Polish(object? sender, RoutedEventArgs e)
    {
        ViewModel.LanguageService.CultureInfo = CultureInfo.GetCultureInfo("pl-PL");
        var box = MessageBoxManager.GetMessageBoxStandard(Lang.Info, Lang.Application_Needs_Restart, ButtonEnum.Ok);
        await box.ShowAsync();
    }

    private async void Click_ShowLocalDocs(object? sender, RoutedEventArgs e)
    {
        var box = MessageBoxManager.GetMessageBoxCustom(
            new MessageBoxCustomParams()
            {
                ButtonDefinitions = new List<ButtonDefinition>()
                {
                    new ButtonDefinition() { Name = Lang.OK }
                },
                ContentTitle = Lang.Documentation,
                ContentMessage = Lang.Entire_Docs_File,
                CanResize = true,
                SizeToContent = SizeToContent.WidthAndHeight
            });
        await box.ShowAsync();
    }
}
