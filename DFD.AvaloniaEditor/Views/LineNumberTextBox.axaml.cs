using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DFD.AvaloniaEditor.ViewModels;

namespace DFD.AvaloniaEditor.Views
{
    public partial class LineNumberTextBox : UserControl
    {
        private TextBlock lineNrTextBlock;
        private string _Text;
        public static readonly DirectProperty<LineNumberTextBox, string> TextProperty = AvaloniaProperty.RegisterDirect<LineNumberTextBox, string>("Text",
            o => o.Text,
            (o, v) => o.Text = v);

        private string _Watermark;
        public static readonly DirectProperty<LineNumberTextBox, string> WatermarkProperty = AvaloniaProperty.RegisterDirect<LineNumberTextBox, string>("Watermark",
            o => o.Watermark,
            (o, v) => o.Watermark = v);

        public LineNumberTextBox()
        {
            InitializeComponent();
            UpdateLineNumbers();
        }

        public string Text
        {
            get { return _Text; }
            set { 
                SetAndRaise(TextProperty, ref _Text, value);
                CodeTextBox.Text = value;
                UpdateLineNumbers();
            }
        }

        public string Watermark
        {
            get { return _Watermark; }
            set
            {
                SetAndRaise(WatermarkProperty, ref _Watermark, value);
                CodeTextBox.Watermark = value;
            }
        }

        private void UpdateLineNumbers()
        {
            int lineNumber = 1;
            
            for (int i = 0; i < CodeTextBox.Text?.Length; i++)
            {
                if (CodeTextBox.Text[i] == '\n')
                {
                    lineNumber++;
                }
            }

            LineNumberTextBlock.Text = string.Empty;
            for (int i = 1; i <= lineNumber; i++)
            {
                LineNumberTextBlock.Text += i + "\n";
            }
        }

        private void CodeTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateLineNumbers();
        }
    }
}
