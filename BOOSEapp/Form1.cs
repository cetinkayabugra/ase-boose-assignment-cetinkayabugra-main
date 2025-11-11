using BOOSE;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        private AppCanvas canvas;
        CommandFactory Factory;
        StoredProgram Program;
        IParser Parser;


        public Form1()
        {
            InitializeComponent();

            // smoother repainting
            this.DoubleBuffered = true;

            // Hook the Paint event
            this.Paint += Form1_Paint;

            // BOOSE about() (ok to keep)
            Debug.WriteLine(AboutBOOSE.about());

            // Create our canvas and draw a circle
            canvas = new AppCanvas(640, 480);

            // Put pen at centre, then draw a filled circle radius 100
            canvas.MoveTo(320, 240);
            canvas.Circle(100, true);

            // Request a repaint
            this.Invalidate();
            Program = new StoredProgram(canvas);
            Parser = new Parser(Factory, Program);
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var b = (Bitmap)canvas.getBitmap();
            g.DrawImageUnscaled(b, 0, 0);
        }
    }
}
