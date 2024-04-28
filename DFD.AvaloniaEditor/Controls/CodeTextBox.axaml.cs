using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;

namespace DFD.AvaloniaEditor.Controls
{
    public class CodeTextBox : TemplatedControl
    {
        private string _Code;
        public static readonly DirectProperty<CodeTextBox, string> CodeProperty = AvaloniaProperty.RegisterDirect<CodeTextBox, string>("Code",
            o => o.Code,
            (o, v) => o.Code = v);

        private string _Watermark;
        public static readonly DirectProperty<CodeTextBox, string> WatermarkProperty = AvaloniaProperty.RegisterDirect<CodeTextBox, string>("Watermark",
            o => o.Watermark,
            (o, v) => o.Watermark = v);

        private string _LineNumbersString;
        public static readonly DirectProperty<CodeTextBox, string> LineNumbersTextProperty = AvaloniaProperty.RegisterDirect<CodeTextBox, string>("LineNumbersText",
            o => o.LineNumbersText,
            (o, v) => o.LineNumbersText = v);

        public string Code
        {
            get => _Code;
            set
            {
                SetAndRaise(CodeProperty, ref _Code, value);
                SetAndRaise(LineNumbersTextProperty, ref _LineNumbersString, GetLineNumbersText());
            }
        }

        public string Watermark
        {
            get => _Watermark;
            set => SetAndRaise(WatermarkProperty, ref _Watermark, value);
        }

        protected string LineNumbersText
        {
            get => _LineNumbersString;
            set => SetAndRaise(LineNumbersTextProperty, ref _LineNumbersString, value);
        }

        private string GetLineNumbersText()
        {
            int lineNumber = 1;


            for (int i = 0; i < Code?.Length; i++)
            {
                if (Code[i] == '\n')
                {
                    lineNumber++;
                }
            }

            string LineNumbersText = string.Empty;
            for (int i = 1; i <= lineNumber; i++)
            {
                LineNumbersText += i + "\n";
            }
            return LineNumbersText;
        }
    }
}
