using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Framework.Utils
{
    /// <summary>
    /// 屏幕点击事件转换类
    /// <para>统一转换成事件的按压、移动、抬起三种类型</para>
    /// </summary>
    public class DeviceEventTransformer
    {
        private readonly UIElement _uiElement;

        public DeviceEventTransformer(UIElement uiElement)
        {
            _uiElement = uiElement;
        }
        /// <summary>
        /// 订阅屏幕点击事件
        /// </summary>
        public void Register()
        {
            //鼠标
            if (_uiElement is Button button)
            {
                //Button类型的冒泡事件，被内部消化处理了，所以需要重新添加路由事件订阅
                button.AddHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler(Button_MouseUp), true);
                button.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(Button_MouseDown), true);
                //button.AddHandler(Button.MouseMoveEvent, new MouseEventHandler(Button_MouseMove), true);
            }
            else
            {
                _uiElement.MouseDown += UiElement_MouseDown;
                _uiElement.MouseUp += UiElement_MouseUp;
            }
            _uiElement.MouseMove += UiElement_MouseMove;
            //触笔
            _uiElement.StylusDown += UiElement_StylusDown;
            _uiElement.StylusMove += UiElement_StylusMove;
            _uiElement.StylusUp += UiElement_StylusUp;
        }
        /// <summary>
        /// 注销屏幕点击事件
        /// </summary>
        public void UnRegister()
        {
            //鼠标
            if (_uiElement is Button button)
            {
                button.RemoveHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler(Button_MouseUp));
                button.RemoveHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(Button_MouseDown));
            }
            else
            {
                _uiElement.MouseDown -= UiElement_MouseDown;
                _uiElement.MouseUp -= UiElement_MouseUp;
            }
            _uiElement.MouseMove -= UiElement_MouseMove;
            //触笔
            _uiElement.StylusDown -= UiElement_StylusDown;
            _uiElement.StylusMove -= UiElement_StylusMove;
            _uiElement.StylusUp -= UiElement_StylusUp;
        }

        #region 触笔事件

        private void UiElement_StylusUp(object sender, StylusEventArgs e)
        {
            var point = e.GetPosition(_uiElement);
            DeviceUp?.Invoke(_uiElement, new DeviceInputArgs()
            {
                DeviceId = e.StylusDevice.Id,
                DeviceType = GetTypeFromDevice(e.StylusDevice),
                Position = point,
                Points = GetStylusPoints(e, _uiElement),
                GetPosition = e.GetPosition,
                SourceArgs = e
            });
        }

        private StylusPointCollection GetStylusPoints(StylusEventArgs stylusEventArgs, UIElement uiElement)
        {
            // 临时去除description
            var pointCollection = new StylusPointCollection();
            var stylusPointCollection = stylusEventArgs.GetStylusPoints(uiElement);
            foreach (var stylusPoint in stylusPointCollection)
            {
                pointCollection.Add(new StylusPoint(stylusPoint.X, stylusPoint.Y, stylusPoint.PressureFactor));
            }

            return pointCollection;
        }

        private void UiElement_StylusMove(object sender, StylusEventArgs e)
        {
            var point = e.GetPosition(_uiElement);
            DeviceMove?.Invoke(_uiElement, new DeviceInputArgs()
            {
                DeviceId = e.StylusDevice.Id,
                DeviceType = GetTypeFromDevice(e.StylusDevice),
                Position = point,
                Points = GetStylusPoints(e, _uiElement),
                GetPosition = e.GetPosition,
                SourceArgs = e
            });
        }
        /// <summary>
        /// 获取当前设备接触类型
        /// </summary>
        /// <param name="stylusDevice"></param>
        /// <returns></returns>
        private DeviceType GetTypeFromDevice(StylusDevice stylusDevice)
        {
            //注：通过DeviceId不准确。这里用Type来判断
            if (stylusDevice.TabletDevice.Type == TabletDeviceType.Stylus)
            {
                return DeviceType.Pen;
            }
            return DeviceType.Touch;
        }

        private void UiElement_StylusDown(object sender, StylusDownEventArgs e)
        {
            var point = e.GetPosition(_uiElement);
            DeviceDown?.Invoke(_uiElement, new DeviceInputArgs()
            {
                DeviceId = e.StylusDevice.Id,
                DeviceType = GetTypeFromDevice(e.StylusDevice),
                Position = point,
                Points = GetStylusPoints(e, _uiElement),
                GetPosition = e.GetPosition,
                SourceArgs = e
            });
        }

        #endregion

        #region 鼠标事件

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {

            UiElement_MouseDown(sender, e);
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UiElement_MouseUp(sender, e);
        }
        private void UiElement_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice == null)
            {
                var point = e.GetPosition(_uiElement);
                DeviceDown?.Invoke(_uiElement, new DeviceInputArgs()
                {
                    DeviceType = DeviceType.Mouse,
                    Position = point,
                    Points = new StylusPointCollection(new List<StylusPoint>() { new StylusPoint(point.X, point.Y) }),
                    GetPosition = e.GetPosition,
                    SourceArgs = e
                });
            }
        }
        private void UiElement_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice == null)
            {
                var point = e.GetPosition(_uiElement);
                DeviceUp?.Invoke(_uiElement, new DeviceInputArgs()
                {
                    DeviceType = DeviceType.Mouse,
                    Position = point,
                    Points = new StylusPointCollection(new List<StylusPoint>() { new StylusPoint(point.X, point.Y) }),
                    GetPosition = e.GetPosition,
                    SourceArgs = e
                });
            }
        }

        private void UiElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.StylusDevice == null)
            {
                var point = e.GetPosition(_uiElement);
                DeviceMove?.Invoke(_uiElement, new DeviceInputArgs()
                {
                    DeviceType = DeviceType.Mouse,
                    Position = point,
                    Points = new StylusPointCollection(new List<StylusPoint>() { new StylusPoint(point.X, point.Y) }),
                    GetPosition = e.GetPosition,
                    SourceArgs = e
                });
            }
        }

        #endregion

        public event EventHandler<DeviceInputArgs> DeviceDown;
        public event EventHandler<DeviceInputArgs> DeviceMove;
        public event EventHandler<DeviceInputArgs> DeviceUp;
    }
}
