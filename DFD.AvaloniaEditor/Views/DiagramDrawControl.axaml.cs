using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
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
            var node = polygon.Tag as IEditableGraphNode;


            var point = e.GetCurrentPoint(this);
            if (point.Properties.IsLeftButtonPressed)
            {
                node.ChildrenCollapsed = !node.ChildrenCollapsed;
                diagramViewModel.RefreshGraph();
            }
        }

        private void SaveAsPng()
        {
            var filePath = "?";
            var panel = this.Find<Panel>("panel");
            var bitmap = new RenderTargetBitmap(new PixelSize((int)panel.Bounds.Width, (int)panel.Bounds.Height));
            using (var context = bitmap.CreateDrawingContext())
            {
               panel.Render(context);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                bitmap.Save(fileStream);
            }
        }
    }
}
