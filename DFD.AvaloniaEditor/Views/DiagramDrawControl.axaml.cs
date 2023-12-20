using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
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
            var node = polygon.Tag as ICollapsableGraphNode;

            if (e.Pointer.IsPrimary)
            {
                node.ChildrenCollapsed = !node.ChildrenCollapsed;
                diagramViewModel.RegenerateGraph();
                Debug.WriteLine(node.ChildrenCollapsed + " on node " + node.Data.Name);
            }
        }
    }
}
