﻿using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer;
public class Program
{
    public static void Main()
    {
        Server server = new Server();
        server.Start();
    }
}