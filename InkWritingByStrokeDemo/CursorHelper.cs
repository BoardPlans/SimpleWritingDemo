using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Pen = System.Drawing.Pen;
using Rectangle = System.Drawing.Rectangle;

namespace InkWritingByStrokeDemo
{
    public static class CursorHelper
    {
        public static Cursor CreateFillCursor(int size = 24, Brush fillBrush = null)
        {
            int unitSize = size / 4;
            var bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clip = new Region(new Rectangle(0, 0, size, size));
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                using (var pen = new Pen(fillBrush ?? Brushes.Red, unitSize))
                {

                    g.DrawEllipse(pen, new Rectangle(unitSize, unitSize, unitSize, unitSize));
                }
            }
            return BitmapCursor.CreateBmpCursor(bmp);
        }

        public static Cursor CreateFromBitmapSource(BitmapSource source)
        {
            var bitmap = BitmapSourceToBitmap(source);
            return BitmapCursor.CreateBmpCursor(bitmap);
        }
        private static Bitmap BitmapSourceToBitmap(BitmapSource source)
        {
            using (var stream = new MemoryStream())
            {
                var e = new BmpBitmapEncoder();
                e.Frames.Add(BitmapFrame.Create(source));
                e.Save(stream);

                var bmp = new Bitmap(stream);

                return bmp;
            }
        }
    }
    /// <summary>
    /// This class allow you create a Cursor form a Bitmap
    /// </summary>
    internal class BitmapCursor : SafeHandle
    {
        public override bool IsInvalid => handle == (IntPtr)(-1);

        public static Cursor CreateBmpCursor(Bitmap cursorBitmap)
        {

            var c = new BitmapCursor(cursorBitmap);

            return CursorInteropHelper.Create(c);
        }
        protected BitmapCursor(Bitmap cursorBitmap)
            : base((IntPtr)(-1), true)
        {
            handle = cursorBitmap.GetHicon();
        }
        protected override bool ReleaseHandle()
        {
            bool result = DestroyIcon(handle);

            handle = (IntPtr)(-1);

            return result;
        }
        [DllImport("user32")]
        private static extern bool DestroyIcon(IntPtr hIcon);
    }
}
