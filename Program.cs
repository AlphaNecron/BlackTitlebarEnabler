using System;
using System.Security.AccessControl;
using System.Windows.Forms;
using static Microsoft.Win32.Registry;

namespace BlackTitlebar
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            if (!IsWin10)
            {
                MessageBox.Show(@"Windows 10 is required, quitting.", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                Application.Exit();
            }
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static bool IsWin10 =>
            LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion",
                    RegistryRights.ReadKey)
                .GetValue("ProductName").ToString().Contains("Windows 10");
    }
}