namespace BOOSEapp
{
    internal static class Program
    {
        /// <summary>ee
        ///  The main entry point for the appelication.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            System.Windows.Forms.MessageBox.Show("BOOSEapp started successfully!");
            Application.Run(new Form1());
        }
    }
}