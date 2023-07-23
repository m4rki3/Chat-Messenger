using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text.Unicode;

namespace ChatServer;
public class Server
{
    private readonly IPHostEntry ipHost;
    private readonly IPAddress ip;
    private readonly IPEndPoint endPoint;
    public Server()
    {
        ipHost = Dns.GetHostEntry("127.0.0.1");
        ip = ipHost.AddressList[0];
        endPoint = new(ip, 11000);
    }
    public void Start()
    {
        using Socket serverSocket = new(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(2);
        while (true)
        {
            Console.WriteLine("...Waiting for socket connection...");
            Socket client = serverSocket.Accept();
            Task task = new(() =>
            {
                try
                {
                    Console.WriteLine("Client socket accepted");
                    byte[] message = new byte[256];
                    do
                    {
                        int bytesReceived = client.Receive(message);
                        Console.WriteLine("Bytes received");
                        //chatLog += Encoding.UTF8.GetString(message);
                        message = Array.Empty<byte>();
                    } while (client.Connected);
                    client.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex); 
                }
            });
            task.Start();
        }
    }
}
