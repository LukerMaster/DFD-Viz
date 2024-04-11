using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DFD.AvaloniaEditor.ViewModels;
using DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;
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
            var visualNode = polygon.Tag as AvaloniaVisualNode;


            var point = e.GetCurrentPoint(this);
            if (point.Properties.IsLeftButtonPressed)
            {
                visualNode.Node.Collapsed = !visualNode.Node.Collapsed;
                diagramViewModel.RefreshGraph();
            }
        }

        private void Node_PointerEntered(object? sender, PointerEventArgs e)
        {
            var polygon = sender as Polygon;
            var visualNode = polygon.Tag as AvaloniaVisualNode;

            var diagramViewModel = DataContext as DiagramViewModel;


            if (visualNode.Node.Collapsable)
                visualNode.Polygon.CurrentColor = new SolidColorBrush(Color.FromArgb(90, 120, 200, 200));

            diagramViewModel.RefreshGraph();
        }

        private void Node_PointerExited(object? sender, PointerEventArgs e)
        {
            var polygon = sender as Polygon;
            var visualNode = polygon.Tag as AvaloniaVisualNode;

            var diagramViewModel = DataContext as DiagramViewModel;

            diagramViewModel.RefreshGraph();
            visualNode.Polygon.CurrentColor = new SolidColorBrush(visualNode.Polygon.DefaultColor);
        }
    }
}
