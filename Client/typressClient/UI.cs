using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientSideSocket;
using Login;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace typressClient
{
    class LoginUI
    {
        public static TcpClient client = new TcpClient("127.0.0.1", 13000);
        public static NetworkStream stream = client.GetStream();

        public LoginUI()
        {
            LoginPacket packet = new LoginPacket();
            Console.WriteLine("---------Login UI---------");

            while (true) {
                Console.WriteLine("> 로그인화면입니다. (0, 0 종료)");
                Console.Write("ID : "); string id = Console.ReadLine();
                Console.Write("PW : "); string pw = Console.ReadLine();
                if (id == "0" && pw == "0") break;
                packet

                IFormatter formatter = new BinaryFormatter();
                formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();

                formatter.Serialize(stream, packet);


                if (id == "22Hours" && pw == "1") //Controller로 id, pw Socket으로 전송.. 
                {
                    LoginPacket mem = new LoginPacket(true, "이종원", "F.A.N");
                    //id, pw 일치여부 확인 후, Controller에서 Typress를 띄운다.
                    TypressUI tpUI = new TypressUI(mem);
                }
                else
                {
                    Console.WriteLine("로그인 실패 : 다시 시도하세요.");
                }
            }
        }
    }
    class TypressUI
    {
        public TypressUI(LoginPacket packet)
        {
            while (true)
            {
                Console.WriteLine(packet.Group + "의 " + packet.Name + "님 안녕하세요~!");
            }
        }
    }
}
