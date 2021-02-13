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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BoardInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            BoardInkCanvas.DefaultDrawingAttributes=new DrawingAttributes()
            {
                Color = Colors.Red,
                FitToCurve = true,
                Height = 6,
                Width = 6,
                IgnorePressure = true,
                IsHighlighter = true,
            };
        }

        private void SelectButton_OnChecked(object sender, RoutedEventArgs e)
        {
            BoardInkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        private void PenButton_OnChecked(object sender, RoutedEventArgs e)
        {
            BoardInkCanvas.EditingMode = InkCanvasEditingMode.InkAndGesture;
        }

        private void EraserButton_OnChecked(object sender, RoutedEventArgs e)
        {
            BoardInkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            BoardInkCanvas.EraserShape=new RectangleStylusShape(100,200);
        }
    }
}
