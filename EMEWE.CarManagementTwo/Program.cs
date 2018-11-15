using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using EMEWE.CarManagement.SystemAdmin;

namespace EMEWE.CarManagement
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool noLocalUser = true;
            Mutex mutex = new Mutex(true, Application.UserAppDataPath.Replace(@"\", "_"), out noLocalUser);
            if (noLocalUser)
            {
                try
                {
                    UdpClient client = new UdpClient(5555);
                    client.Close();
                    noLocalUser = true;
                }
                catch (SocketException)
                {
                    noLocalUser = false;
                }
            }
            if (noLocalUser)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LoginForm());
            }
            else
                MessageBox.Show("系统已启动，请关闭后在操作！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }
    }
}