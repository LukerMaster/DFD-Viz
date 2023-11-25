using System.Numerics;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer;

internal class ErrorViewData : IViewDataProvider
{
    private AggregateException _exception;
    private Font _font;
    private Text _text;
    public ErrorViewData(AggregateException e)
    {
        _font = new Font(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf"));

        _text = new Text();
        _text.Font = _font;
        _text.DisplayedString = e.Message;
        _text.FillColor = new Color(255, 40, 40);
        _text.CharacterSize = 60;
        _text.Scale = new Vector2f(1, 1);
        _text.Position = new Vector2f(0, 0);
        //_text.Origin = new Vector2f(_text.GetLocalBounds().Width / 2, _text.GetLocalBounds().Height / 2);
    }

    public Text Text
    {
        get => _text;
    }

    public Vector2 Center
    {
        get => new Vector2((_text.GetLocalBounds().Width / 2) + _text.Position.X, (_text.GetLocalBounds().Height / 2) + _text.Position.Y);
    }
    public Vector2 Size { get => new Vector2(_text.GetLocalBounds().Width, _text.GetLocalBounds().Height); }
}