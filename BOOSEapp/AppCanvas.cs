using System;
using System.Drawing;
using BOOSE;

namespace BOOSEapp
{
    /// <summary>
    /// Implementation of BOOSE ICanvas for drawing on a Bitmap.
    /// No restrictions – everything draws normally.
    /// </summary>
    public class AppCanvas : ICanvas, IDisposable
    {
        private Bitmap canvasBitmap;
        private Graphics g;
        private Pen pen = new Pen(Color.Black);
        private int xPos;
        private int yPos;
        private bool disposed = false;

        public AppCanvas(int width, int height)
        {
            canvasBitmap = new Bitmap(width, height);
            g = Graphics.FromImage(canvasBitmap);
            Clear();
        }

        public int Xpos { get => xPos; set => xPos = value; }
        public int Ypos { get => yPos; set => yPos = value; }

        public object PenColour
        {
            get => pen.Color;
            set => pen.Color = (Color)value;
        }

        public void MoveTo(int x, int y)
        {
            xPos = x;
            yPos = y;
        }

        public void DrawTo(int x, int y)
        {
            g.DrawLine(pen, xPos, yPos, x, y);
            xPos = x;
            yPos = y;
        }

        public void Circle(int radius, bool filled)
        {
            Rectangle r = new Rectangle(xPos - radius, yPos - radius, radius * 2, radius * 2);
            if (filled)
                g.FillEllipse(new SolidBrush(pen.Color), r);
            g.DrawEllipse(pen, r);
        }

        public void Rect(int width, int height, bool filled)
        {
            Rectangle r = new Rectangle(xPos, yPos, width, height);
            if (filled)
                g.FillRectangle(new SolidBrush(pen.Color), r);
            g.DrawRectangle(pen, r);
        }

        public void Tri(int width, int height)
        {
            Point p1 = new Point(xPos + width / 2, yPos);
            Point p2 = new Point(xPos, yPos + height);
            Point p3 = new Point(xPos + width, yPos + height);
            g.DrawPolygon(pen, new[] { p1, p2, p3 });
        }

        public void WriteText(string text)
        {
            g.DrawString(text, SystemFonts.DefaultFont, new SolidBrush(pen.Color), xPos, yPos);
        }

        public void SetColour(int r, int g2, int b)
        {
            pen.Color = Color.FromArgb(r, g2, b);
        }

        public void Clear()
        {
            g.Clear(Color.Gray);
        }

        public void Reset()
        {
            xPos = 0;
            yPos = 0;
            pen.Color = Color.Black;
        }

        public void Set(int width, int height)
        {
            // Dispose old graphics and bitmap
            g?.Dispose();
            canvasBitmap?.Dispose();

            canvasBitmap = new Bitmap(width, height);
            g = Graphics.FromImage(canvasBitmap);
            Clear();
        }

        public object getBitmap()
        {
            return canvasBitmap;
        }

        // Implement IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    g?.Dispose();
                    pen?.Dispose();
                    canvasBitmap?.Dispose();
                }
                disposed = true;
            }
        }

        // Destructor
        ~AppCanvas()
        {
            Dispose(false);
        }
    }
}