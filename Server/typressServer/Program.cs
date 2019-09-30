// NameSpace 선언
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using TypressPacket;
using MySql.Data.MySqlClient;
using System.Threading;
using Nektra.Deviare2;
using System.Diagnostics;
using System.Collections;

namespace TypressServer
{
    public static class Program
    {
        //public static ViewHandler ViewManager = null;
        //public static ThreadHandler ThreadManager = null;
        [STAThread]
        public static void Main()
        {
            ViewHandler ViewManager = null;
            ThreadHandler ThreadManager = null;
            try
            {
                ViewManager = new ViewHandler();
                ThreadManager = new ThreadHandler();

                ViewHandler.TypressOpen();

            }
            catch (SocketException socketEx)
            {
                ViewHandler.SocketExMessage(socketEx);
            }
            catch (Exception commonEx)
            {
                ViewHandler.ExMessage(commonEx);
            }
            finally
            {
                ViewHandler.TypressClose();
                ViewManager = null;
            }
        }
    }
}