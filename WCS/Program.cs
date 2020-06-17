using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace WCS
{
    static class Program
    {
        public static bool bOnlyOneInstance = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bOnlyOneInstance = false;
            Mutex mutex = new Mutex(true, Application.UserAppDataPath.Replace(@"\", "_"), out bOnlyOneInstance);

            if (!bOnlyOneInstance)
            {
                MessageBox.Show("系统已经运行!", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Environment.Exit(0);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
