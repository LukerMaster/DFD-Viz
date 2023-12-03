using DFD.Vizualizer.Interfaces;
using SFML.Graphics;

namespace DFD.Vizualizer;

internal class ErrorUI : IProgramUI
{
    private RenderWindow _window;
    private ErrorViewData _viewData;
    private WindowViewManipulator _viewManipulator;
    public void Process()
    {
        _window.SetView(_viewManipulator.CurrentView);
        _window.Clear(Color.Black);
        _window.Draw(_viewData.Text);
        _window.Display();
    }

    public ErrorUI(RenderWindow w, ErrorViewData errorData, WindowViewManipulator viewManipulator)
    {
        _window = w;
        _viewData = errorData;
        _viewManipulator = viewManipulator;

        _viewManipulator.ResetView();
    }
}