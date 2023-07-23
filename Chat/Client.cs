using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat;
public class Client : IDisposable
{
    private readonly Socket socket;
    private readonly IPEndPoint ip;
    private readonly int port;
    private EndPoint serverEndPoint;
    public Client(IPAddress serverIP, int serverPort)
    {
        serverEndPoint = new IPEndPoint(serverIP, serverPort);
        port = serverPort;
        string hostName = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(hostName);
        IPAddress ipAddress = ipHost.AddressList[0];
        ip = new(ipAddress, port);
        socket = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ip);
    }
    public void Dispose()
    {
        socket.Close();
    }
    public void SendMessage(string name, string message)
    {
        socket.Send(Encoding.UTF8.GetBytes($"{name}: {message}\n"));
    }
}