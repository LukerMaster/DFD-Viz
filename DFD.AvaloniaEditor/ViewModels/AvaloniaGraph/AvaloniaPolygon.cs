using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Media;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaPolygon : INotifyPropertyChanged
{
    internal Points Points { get; set; } = new();

    internal IBrush _currentColor;
    internal IBrush CurrentColor
    {
        get { return _currentColor;}
        set
        {
            _currentColor = value;
            OnPropertyChanged();
        }
    }
    internal Color DefaultColor { get; set; } = Colors.Transparent;

    public AvaloniaPolygon()
    {
        CurrentColor = new SolidColorBrush(DefaultColor);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}