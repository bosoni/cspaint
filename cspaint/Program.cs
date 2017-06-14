using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace cspaint
{
    static class Program
    {
    	public static Tools form;
    	
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form=new Tools();
            Application.Run(form);
        }
    }
}
