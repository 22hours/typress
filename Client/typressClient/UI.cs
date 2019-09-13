﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientSideSocket;
using TypressPacket;
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
        public static string Id { get; set; }
        public static string Pw { get; set; }
        public LoginUI()
        {
            DataPacket packet = new DataPacket();
            Console.WriteLine("---------Login UI---------");
            Console.WriteLine("> 로그인화면입니다. (0, 0 종료)");
            Console.Write("ID : "); string id = Console.ReadLine();
            Console.Write("PW : "); string pw = Console.ReadLine();
            Id = id;
            Pw = pw;
        }
    }
    class TypressUI
    {
        public TypressUI(DataPacket packet)
        {
            
            Console.WriteLine(packet.Group + "의 " + packet.Name + "님 안녕하세요~!");
            Console.WriteLine("Typress UI . . ."); Console.ReadLine();
           
        }
    }
}
