using DevExpress.Skins;
using System;
using System.Windows.Forms;

namespace MGStudio
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.Run(new frmMainForm());
        }
    }
#endif
}
