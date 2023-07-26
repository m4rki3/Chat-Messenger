using ChatServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat;
public class Client : IDisposable
{
    private readonly Socket serverSocket;
    private readonly Socket loggerSocket;
    public bool Connected =>
        serverSocket.Connected && loggerSocket.Connected;
    public Client(string hostNameOrAddress, int serverPort, int loggerPort)
    {
        IPAddress ipAddress = Dns.GetHostAddresses(hostNameOrAddress)[0];
        IPEndPoint serverEndPoint = new(ipAddress, serverPort);
        IPEndPoint loggerEndPoint = new(ipAddress, loggerPort);
        serverSocket = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        loggerSocket = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Connect(serverEndPoint);
        loggerSocket.Connect(loggerEndPoint);
    }
    public void Dispose()
    {
        serverSocket.Dispose();
        loggerSocket.Dispose();
    }
    public void SendMessage(string name, string message)
    {
        serverSocket.Send(
            Encoding.UTF8.GetBytes($"{name}: {message}\n"),
            SocketFlags.Partial
        );
    }
    public string GetChatLog()
    {
        byte[] buffer = new byte[256];
        int bytesReceived = loggerSocket.Receive(buffer, SocketFlags.Partial);
        return Encoding.UTF8.GetString(buffer, index: 0, bytesReceived);
    }
}