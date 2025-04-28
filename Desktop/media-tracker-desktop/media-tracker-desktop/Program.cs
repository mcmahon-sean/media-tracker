using media_tracker_desktop.Forms;

namespace media_tracker_desktop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // Launch DashboardForm instead of Form1
            Application.Run(new MainForm());
        }
    }
}
