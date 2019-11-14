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
using System.Diagnostics;
using System.Collections;
using Nektra.Deviare2;

namespace TypressServer
{
    public static class Program
    {
        //public static ViewHandler ViewManager = null;
        //public static ThreadHandler ThreadManager = null;
        [STAThread]
        public static void Main()
        {
            ThreadHandler ThreadManager = null;
            try
            {
                ViewHandler.TypressOpen();

                ThreadManager = new ThreadHandler();

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
            }
        }
    }
}