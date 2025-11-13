namespace BOOSEapp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtProgram;
        private System.Windows.Forms.PictureBox picCanvas;
        private System.Windows.Forms.Button btnRun;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.txtProgram = new System.Windows.Forms.TextBox();
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.btnRun = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.SuspendLayout();

            // txtProgram
            this.txtProgram.AcceptsReturn = true;
            this.txtProgram.Multiline = true;
            this.txtProgram.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProgram.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtProgram.Location = new System.Drawing.Point(12, 12);
            this.txtProgram.Size = new System.Drawing.Size(350, 500);

            // picCanvas
            this.picCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCanvas.Location = new System.Drawing.Point(380, 12);
            this.picCanvas.Size = new System.Drawing.Size(800, 500);

            // btnRun
            this.btnRun.Location = new System.Drawing.Point(12, 520);
            this.btnRun.Size = new System.Drawing.Size(120, 35);
            this.btnRun.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);

            // Form1
            this.ClientSize = new System.Drawing.Size(1200, 570);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.picCanvas);
            this.Controls.Add(this.txtProgram);
            this.Text = "BOOSE App";
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}
