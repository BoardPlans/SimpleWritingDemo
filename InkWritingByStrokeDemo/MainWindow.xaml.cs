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

namespace TouchWritingDemo
{
    /// <summary>
    /// 重写底层笔迹绘制demo
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += MainWindow_MouseDown;
            MouseMove += MainWindow_MouseMove;
            MouseUp += MainWindow_MouseUpUp;

            StylusMove += MainWindow_StylusMove;
            StylusUp += MainWindow_StylusUp;

            //待处理 擦除
        }

        #region 鼠标

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseStrokeVisual = new StrokeVisual();
            var visualCanvas = new VisualCanvas(_mouseStrokeVisual);
            InkGrid.Children.Add(visualCanvas);
        }

        private void MainWindow_MouseUpUp(object sender, MouseButtonEventArgs e)
        {
            _mouseStrokeVisual = null;
        }

        private StrokeVisual _mouseStrokeVisual; 
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseStrokeVisual!=null)
            {
                var stylusPoint = e.GetPosition(this);
                _mouseStrokeVisual.Add(new StylusPoint(stylusPoint.X, stylusPoint.Y));
                _mouseStrokeVisual.Redraw();
            }
        }

        #endregion

        #region 触摸

        private void MainWindow_StylusUp(object sender, StylusEventArgs e)
        {
            StrokeVisualList.Remove(e.StylusDevice.Id);
        }

        private void MainWindow_StylusMove(object sender, StylusEventArgs e)
        {
            var strokeVisual = GetStrokeVisual(e.StylusDevice.Id);
            var stylusPointCollection = e.GetStylusPoints(this);
            foreach (var stylusPoint in stylusPointCollection)
            {
                strokeVisual.Add(new StylusPoint(stylusPoint.X, stylusPoint.Y));
            }

            strokeVisual.Redraw();
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

        #endregion
    }
}