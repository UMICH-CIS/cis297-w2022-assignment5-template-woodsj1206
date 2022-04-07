using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayTable
{
    /// <summary>
    /// This application allows the user to search for authors and books in a database.
    /// </summary>
    /// <Student>Jonathan Woods</Student>
    /// <Class>CIS297</Class>
    /// <Semester>Winter 2022</Semester>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DisplayAuthorsTable());
        }
    }
}
