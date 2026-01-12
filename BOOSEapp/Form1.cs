using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace BOOSEapp
{
    public partial class Form1 : Form
    {
        private AppCanvas canvas = null!;
        private VariableStore variables = null!;
        private SimpleParser parser = null!;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            canvas?.Dispose();

            // Create fresh instances
            canvas = new AppCanvas(picCanvas.Width, picCanvas.Height);
            variables = new VariableStore();
            parser = new SimpleParser(canvas, variables);

            Debug.WriteLine("=== BOOSE Program Starting ===");

            try
            {
                // Clean program text
                string cleaned = string.Join("\n",
                    txtProgram.Text
                        .Replace("\r", "")
                        .Split('\n')
                        .Where(l => l.Trim().Length > 0)
                );

                // Parse program
                Debug.WriteLine("Parsing program...");
                var commands = parser.Parse(cleaned);
                Debug.WriteLine($"Parsed {commands.Count} commands");

                // Create executor
                var evaluator = new ExpressionEvaluator(variables);
                var executor = new ProgramExecutor(commands, variables, evaluator);

                // Link control flow commands to executor
                foreach (var cmd in commands)
                {
                    if (cmd is IControlFlowCommand controlCmd)
                    {
                        controlCmd.SetExecutor(executor);
                    }
                }

                // Execute program
                Debug.WriteLine("Executing commands...");
                executor.Execute();

                // Display result
                picCanvas.Image = (System.Drawing.Bitmap)canvas.getBitmap();
                Debug.WriteLine("Program completed successfully!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                MessageBox.Show(ex.Message, "Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}