using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InkWritingByCanvasDemo
{
    /// <summary>
    /// InkCanvas实现书写的最简单DEMO
    /// </summary>
    public partial class MainWindow : Window
    {
        private InkCanvasEditingMode _editingMode;

        public MainWindow()
        {
            InitializeComponent();
            BoardInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            BoardInkCanvas.DefaultDrawingAttributes = new DrawingAttributes()
            {
                Color = Colors.Red,
                FitToCurve = true,
                Height = 6,
                Width = 6,
                IgnorePressure = false,
            };
            BoardInkCanvas.EraserShape = new RectangleStylusShape(100, 200);
        }

        private void SelectButton_OnChecked(object sender, RoutedEventArgs e)
        {
            BoardInkCanvas.EditingMode = _editingMode = InkCanvasEditingMode.Select;
        }

        private void PenButton_OnChecked(object sender, RoutedEventArgs e)
        {
            BoardInkCanvas.EditingMode = _editingMode = InkCanvasEditingMode.InkAndGesture;
        }

        private void EraserButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _editingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void BoardInkCanvas_OnPreviewStylusDown(object sender, StylusDownEventArgs e)
        {
            if (_editingMode == InkCanvasEditingMode.EraseByPoint)
            {
                BoardInkCanvas.EditingMode = _editingMode;
            }
        }

        private void BoardInkCanvas_OnPreviewStylusUp(object sender, StylusEventArgs e)
        {
            if (_editingMode == InkCanvasEditingMode.EraseByPoint)
            {
                BoardInkCanvas.EditingMode = InkCanvasEditingMode.None;
            }
        }

        private void BoardInkCanvas_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_editingMode == InkCanvasEditingMode.EraseByPoint)
            {
                BoardInkCanvas.EditingMode = _editingMode;
            }
        }
        private void BoardInkCanvas_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_editingMode == InkCanvasEditingMode.EraseByPoint)
            {
                BoardInkCanvas.EditingMode = InkCanvasEditingMode.None;
            }
        }
    }
}
