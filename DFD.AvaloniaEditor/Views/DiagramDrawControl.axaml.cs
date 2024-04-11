using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DFD.AvaloniaEditor.ViewModels;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.Views
{
    public partial class DiagramDrawControl : UserControl
    {
        public DiagramDrawControl()
        {
            InitializeComponent();
        }

        private void Node_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var diagramViewModel = DataContext as DiagramViewModel;

            var polygon = sender as Polygon;
            var node = polygon.Tag as IMultilevelGraphNode;


            var point = e.GetCurrentPoint(this);
            if (point.Properties.IsLeftButtonPressed)
            {
                node.Collapsed = !node.Collapsed;
                diagramViewModel.RefreshGraph();
            }
        }

        private void Node_PointerEntered(object? sender, PointerEventArgs e)
        {
            var polygon = sender as Polygon;
            var node = polygon.Tag as IMultilevelGraphNode;


            polygon.Fill = new SolidColorBrush(Color.FromArgb(30, 120, 200, 200));

        }
    }
}
