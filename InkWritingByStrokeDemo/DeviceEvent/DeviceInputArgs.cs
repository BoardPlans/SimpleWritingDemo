using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Framework.Utils
{
    public class DeviceInputArgs : RoutedEventArgs
    {
        public int DeviceId { get; set; } = -1;
        public Point Position { get; set; }
        public StylusPointCollection Points { get; set; }
        public DeviceType DeviceType { get; set; }
        public Func<UIElement, Point> GetPosition { get; set; }
        public InputEventArgs SourceArgs { get; set; }
    }
}
