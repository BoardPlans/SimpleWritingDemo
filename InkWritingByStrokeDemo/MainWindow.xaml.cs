using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using InkWritingByStrokeDemo;

namespace InkWritingByStrokeDemo
{
    /// <summary>
    /// 重写底层笔迹绘制demo
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InkGrid.MouseEnter += MainWindow_MouseEnter;

            var deviceEventTransformer = new DeviceEventTransformer(InkGrid);
            deviceEventTransformer.Register();
            deviceEventTransformer.DeviceDown += DeviceEventTransformer_DeviceDown;
            deviceEventTransformer.DeviceUp += DeviceEventTransformer_DeviceUp;
            deviceEventTransformer.DeviceMove += DeviceEventTransformer_DeviceMove;
        }

        private void DeviceEventTransformer_DeviceMove(object sender, DeviceInputArgs e)
        {
            if (!_deviceDown)
            {
                return;
            }
            AddPoint(e.Position);
            var strokeVisual = GetStrokeVisual(e.DeviceId);
            if (e.DeviceType == DeviceType.Mouse)
            {
                var stylusPoint = e.GetPosition(this);
                strokeVisual.Add(new StylusPoint(stylusPoint.X, stylusPoint.Y));
                strokeVisual.Redraw();
            }
            else
            {
                var stylusPointCollection = e.Points;
                foreach (var stylusPoint in stylusPointCollection)
                {
                    strokeVisual.Add(new StylusPoint(stylusPoint.X, stylusPoint.Y));
                }
                strokeVisual.Redraw();
            }
        }

        private void DeviceEventTransformer_DeviceUp(object sender, DeviceInputArgs e)
        {
            _deviceDown = false;
            StrokeVisualList.Remove(e.DeviceId);
        }
        private bool _deviceDown = false;
        private void DeviceEventTransformer_DeviceDown(object sender, DeviceInputArgs e)
        {
            _deviceDown = true;
            AddPoint(e.Position);
            if (e.DeviceType != DeviceType.Mouse)
            {
                //Cursor = Cursors.None;
                //Mouse.UpdateCursor();
            }
        }

        private const double EllipseSize = 10;
        private void AddPoint(Point point)
        {
            var ellipse = new Ellipse();
            ellipse.Width = EllipseSize;
            ellipse.Height = EllipseSize;
            ellipse.Fill = Brushes.ForestGreen;
            Canvas.SetLeft(ellipse, point.X + EllipseSize / 2);
            Canvas.SetTop(ellipse, point.Y + EllipseSize / 2);
            PointsCanvas.Children.Add(ellipse);
        }

        private void MainWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice != null && e.StylusDevice.Id > -1)
            {
                return;
            }
            //Cursor = GetFillCursor();
            //Mouse.UpdateCursor();
        }

        #region 其它
        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            PointsCanvas.Children.Clear();
            StrokeVisualList.Clear();
            InkGrid.Children.Clear();
        }

        private StrokeVisual GetStrokeVisual(int id)
        {
            if (StrokeVisualList.TryGetValue(id, out var visual))
            {
                return visual;
            }

            var strokeVisual = new StrokeVisual();
            StrokeVisualList[id] = strokeVisual;
            var visualCanvas = new VisualCanvas(strokeVisual);
            InkGrid.Children.Add(visualCanvas);

            return strokeVisual;
        }

        private Dictionary<int, StrokeVisual> StrokeVisualList { get; } = new Dictionary<int, StrokeVisual>();
        private Cursor _fillCursor = null;
        private Cursor GetFillCursor()
        {
            return _fillCursor ?? (_fillCursor = CursorHelper.CreateFillCursor());
        }
        #endregion

    }

}