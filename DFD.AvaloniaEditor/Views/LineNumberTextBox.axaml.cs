using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DFD.AvaloniaEditor.ViewModels;

namespace DFD.AvaloniaEditor.Views
{
    public partial class LineNumberTextBox : UserControl
    {
        private TextBlock lineNrTextBlock;
        
        public LineNumberTextBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            lineNrTextBlock = this.GetControl<TextBlock>(nameof(LineNumberTextBlock));
        }
        private void UpdateLineNumber(TextBox textBox)
        {
            int lineNumber = 1;
            int caretIndex = textBox.CaretIndex;
            
            for (int i = 0; i < caretIndex; i++)
            {
                if (textBox.Text[i] == '\n')
                {
                    lineNumber++;
                }
            }

            lineNrTextBlock.Text = string.Empty;
            for (int i = 1; i <= lineNumber; i++)
            {
                lineNrTextBlock.Text += i + "\n";
            }
        }

        private void CodeTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            UpdateLineNumber(sender as TextBox);
        }
    }
}
