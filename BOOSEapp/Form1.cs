using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using BOOSE;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        private AppCanvas canvas = null!;
        private AppCommandFactory factory = null!;
        private StoredProgram program = null!;
        private Parser parser = null!;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            canvas = new AppCanvas(picCanvas.Width, picCanvas.Height);
            factory = new AppCommandFactory();
            program = new StoredProgram(canvas);
            parser = new Parser(factory, program);
            Debug.WriteLine(AboutBOOSE.about());

            try
            {
                string cleaned = string.Join("\n",
                    txtProgram.Text
                        .Replace("\r", "")
                        .Split('\n')
                        .Where(l => l.Trim().Length > 0)
                );

                parser.ParseProgram(cleaned);
                program.Run();

                picCanvas.Image = (Bitmap)canvas.getBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Runtime error");
            }
        }
    }
}
