﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Avalonia;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.Services;
using DFD.GraphConverter.Interfaces;
using DFD.GraphConverter;
using DFD.GraphConverter.ViewModelImplementation;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;
using DFD.Parsing.Interfaces;
using DFD.Parsing;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

internal class DiagramViewModel : ViewModelBase
{
    public DiagramViewModel(IVisualGraphProvider diagramProvider)
    {
        Provider = diagramProvider;
    }

    public IVisualGraphProvider Provider { get; }

}