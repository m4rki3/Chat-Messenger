using System.Net.Sockets;
using System.Net;
using System.Text;

public class Program
{
    public static void Main()
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        const int port = 8808;
        IPEndPoint endPoint = new(ip, port);
        using Socket socket = new(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        Console.WriteLine("Enter the message");
        string message = Console.ReadLine();
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        socket.Connect(endPoint);
        socket.Send(bytes);
        byte[] buffer = new byte[256];
        int bytesReceived;
        StringBuilder answer = new();
        do
        {
            bytesReceived = socket.Receive(buffer);
            answer.Append(Encoding.UTF8.GetString(buffer, 0, bytesReceived));
        }
        while (socket.Available > 0);
        Console.WriteLine(answer);
        socket.Shutdown(SocketShutdown.Both);
    }
}