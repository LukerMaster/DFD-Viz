using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.Services;
using DFD.AvaloniaEditor.ViewModels;
using DFD.AvaloniaEditor.Views;
using DFD.GraphConverter;
using DFD.GraphConverter.Interfaces;
using DFD.Parsing;
using DFD.Parsing.Interfaces;

namespace DFD.AvaloniaEditor;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);


        IDfdCodeStringProvider codeProvider = new DfdCodeStringProvider();

        IInterpreter interpreter = new Interpreter();

        IMultilevelGraphConverter converter = new MultilevelGraphConverter();

        IVisualGraphCreator creator = new VisualGraphCreator();

        IVisualGraphGenerationPipeline generationPipeline = new VisualGraphGenerationPipeline(interpreter, converter, creator, codeProvider);


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.Args[0] is not null)
            {
                codeProvider.FilePath = desktop.Args[0];
                codeProvider.DfdCode = File.ReadAllText(desktop.Args[0]);
            }

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(generationPipeline, codeProvider)
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(null, null)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
