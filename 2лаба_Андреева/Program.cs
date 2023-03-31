using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2лаба_Андреева
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string RemoveCharString(string str)
        {
            const string nums = "1234567890 ,.";

            for (int i = 0; i < str.Length; i++)
            {
                if (!nums.Contains(str[i]))
                {
                    str = str.Remove(i, 1);
                    break;
                }
            }

            return str;
        }
    }
}
