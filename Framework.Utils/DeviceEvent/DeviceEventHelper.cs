using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Framework.Utils
{
    /// <summary>
    /// 附加点击事件辅助类
    /// <para>附加事件委托类型：<see cref="DeviceInputHandler"/></para>
    /// <para>附加事件参数类型：<see cref="DeviceInputArgs"/></para>
    /// <para>事件转换来源：<see cref="DeviceEventTransformer"/></para>
    /// </summary>
    public static class DeviceEvents
    {
        // 声明并定义路由事件
        public static readonly RoutedEvent DeviceDownEvent = EventManager.RegisterRoutedEvent
            ("DeviceDown", RoutingStrategy.Bubble, typeof(DeviceInputHandler), typeof(UIElement));

        // 添加侦听
        public static void AddDeviceDownHandler(DependencyObject d, DeviceInputHandler handler)
        {
            if (d is UIElement uiElement)
            {
                //注册路由
                uiElement.AddHandler(DeviceEvents.DeviceDownEvent, handler);
                //todo 临时处理，待添加EventTransformManager
                var deviceEventTransformer = new DeviceEventTransformer(uiElement);
                deviceEventTransformer.DeviceDown += (sender, args) =>
                {
                    //触发对应路由
                    args.RoutedEvent = DeviceEvents.DeviceDownEvent;
                    uiElement.RaiseEvent(args);
                };
                deviceEventTransformer.Register();
            }
        }

        // 移除侦听
        public static void RemoveDeviceDownHandler(DependencyObject d, DeviceInputHandler handler)
        {
            if (d is UIElement uiElement)
            {
                uiElement.RemoveHandler(DeviceEvents.DeviceDownEvent, handler);
            }
        }
        // 声明并定义路由事件
        public static readonly RoutedEvent DeviceMoveEvent = EventManager.RegisterRoutedEvent
            ("DeviceMove", RoutingStrategy.Bubble, typeof(DeviceInputHandler), typeof(UIElement));

        // 添加侦听
        public static void AddDeviceMoveHandler(DependencyObject d, DeviceInputHandler handler)
        {
            if (d is UIElement uiElement)
            {
                //注册路由
                uiElement.AddHandler(DeviceEvents.DeviceMoveEvent, handler);
                //todo 临时处理，待添加EventTransformManager
                var deviceEventTransformer = new DeviceEventTransformer(uiElement);
                deviceEventTransformer.DeviceMove += (sender, args) =>
                {
                    //触发对应路由
                    args.RoutedEvent = DeviceEvents.DeviceMoveEvent;
                    uiElement.RaiseEvent(args);
                };
                deviceEventTransformer.Register();
            }
        }

        // 移除侦听
        public static void RemoveDeviceMoveHandler(DependencyObject d, DeviceInputHandler handler)
        {
            if (d is UIElement uiElement)
            {
                uiElement.RemoveHandler(DeviceEvents.DeviceMoveEvent, handler);
            }
        }
        // 声明并定义路由事件
        public static readonly RoutedEvent DeviceUpEvent = EventManager.RegisterRoutedEvent
            ("DeviceUp", RoutingStrategy.Bubble, typeof(DeviceInputHandler), typeof(UIElement));

        // 添加侦听
        public static void AddDeviceUpHandler(DependencyObject d, DeviceInputHandler handler)
        {
            if (d is UIElement uiElement)
            {
                //注册路由
                uiElement.AddHandler(DeviceEvents.DeviceUpEvent, handler);
                //todo 临时处理，待添加EventTransformManager
                var deviceEventTransformer = new DeviceEventTransformer(uiElement);
                deviceEventTransformer.DeviceUp += (sender, args) =>
                {
                    //触发对应路由
                    args.RoutedEvent = DeviceEvents.DeviceUpEvent;
                    uiElement.RaiseEvent(args);
                };
                deviceEventTransformer.Register();
            }
        }

        // 移除侦听
        public static void RemoveDeviceUpHandler(DependencyObject d, DeviceInputHandler handler)
        {
            if (d is UIElement uiElement)
            {
                uiElement.RemoveHandler(DeviceEvents.DeviceUpEvent, handler);
            }
        }
    }
    //自定义委托类型
    public delegate void DeviceInputHandler(object sender, DeviceInputArgs e);
}
