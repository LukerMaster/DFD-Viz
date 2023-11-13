﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFD.Model.Interfaces;

namespace DFD
{
    public class Process : ISymbolicEntity
    {
        public string EntityName { get; init; }
        public string DisplayedText { get; init; }
        public DisplayType DisplayType
        {
            get => DisplayType.Rectangle;
        }
        public Color Color { get => Color.LightCyan; }
    }
}
