using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DFD.AvaloniaEditor.Assets;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.Services;
using DFD.AvaloniaEditor.ViewModels;
using DFD.AvaloniaEditor.Views;
using DFD.GraphConverter;
using DFD.GraphConverter.Interfaces;
using DFD.GraphvizConverter;
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

        ILanguageService languageService = new LanguageService("LangSettings.txt");
        Lang.Culture = languageService.CultureInfo;

        IDfdCodeStringProvider codeProvider = new DfdCodeStringProvider();

        IInterpreter interpreter = new Interpreter();

        IMultilevelGraphConverter converter = new MultilevelGraphConverter();

        IVisualGraphCreator creator = new VisualGraphCreator(new GraphvizRunnerFactory(Environment.OSVersion.Platform).CreateRunner());

        IVisualGraphGenerationPipeline generationPipeline = new VisualGraphGenerationPipeline(interpreter, converter, creator, codeProvider);
        
        // Classic way of doing IoC results in circular dependency so creation of this object needs to be broken up
        GraphFileStorageService storageService = new GraphFileStorageService();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.Args.Length == 1)
            {
                codeProvider.FilePath = desktop.Args[0];
                codeProvider.DfdCode = File.ReadAllText(desktop.Args[0]);
            }

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(generationPipeline, codeProvider, storageService, languageService)
            };

            storageService.SetStorageProvider(desktop.MainWindow.StorageProvider);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(null, null, null, null)
            };
        }

        

        base.OnFrameworkInitializationCompleted();
    }
}
