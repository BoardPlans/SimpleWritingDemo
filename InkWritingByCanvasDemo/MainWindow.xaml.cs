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
using Framework.Utils;

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

            BoardInkCanvas.UseCustomCursor = true;
            BoardInkCanvas.Cursor = Cursors.Arrow;

            BoardInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            BoardInkCanvas.DefaultDrawingAttributes = new DrawingAttributes()
            {
                Color = Colors.Red,
                FitToCurve = true,
                Height = 6,
                Width = 6,
                IgnorePressure = false,
            };
            BoardInkCanvas.EraserShape = new RectangleStylusShape(60, 80);

            var deviceEventTransformer = new DeviceEventTransformer(BoardInkCanvas);
            deviceEventTransformer.Register();
            deviceEventTransformer.DeviceDown += DeviceEventTransformer_DeviceDown;
            deviceEventTransformer.DeviceUp += DeviceEventTransformer_DeviceUp;
            deviceEventTransformer.DeviceMove += DeviceEventTransformer_DeviceMove;
        }

        #region 收集点集

        private void DeviceEventTransformer_DeviceMove(object sender, DeviceInputArgs e)
        {
            if (!_deviceDown)
            {
                return;
            }
            AddPoint(e.Position);
        }
        private bool _deviceDown = false;
        private void DeviceEventTransformer_DeviceDown(object sender, DeviceInputArgs e)
        {
            _deviceDown = true;
            AddPoint(e.Position);
        }

        private void DeviceEventTransformer_DeviceUp(object sender, DeviceInputArgs e)
        {
            _deviceDown = false;
            AddPoint(e.Position);
        }
        private const double EllipseSize = 20;
        private void AddPoint(Point point)
        {
            var ellipse = new Ellipse();
            ellipse.Width = EllipseSize;
            ellipse.Height = EllipseSize;
            ellipse.Fill = Brushes.ForestGreen;
            Canvas.SetLeft(ellipse, point.X - EllipseSize / 2);
            Canvas.SetTop(ellipse, point.Y - EllipseSize / 2);
            PointsCanvas.Children.Add(ellipse);
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            PointsCanvas.Children.Clear();
            BoardInkCanvas.Strokes.Clear();
        }

        #endregion

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
