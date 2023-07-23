using System.Net.Sockets;
using System.Net;
using System.Text;

public class Program
{
    public static void Main()
    {
        const int port = 8808;
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new(ip, port);
        using Socket socket = new(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(endPoint);
        socket.Listen(5);
        while (true)
        {
            using Socket listener = socket.Accept();
            byte[] bytes = new byte[256];
            int bytesReceived;
            StringBuilder builder = new();
            do
            {
                bytesReceived = listener.Receive(bytes);
                builder.Append(Encoding.UTF8.GetString(bytes, 0, bytesReceived));
            }
            while (listener.Available > 0);
            Console.WriteLine(builder);
            listener.Send(Encoding.UTF8.GetBytes("Success"));
            Console.ReadLine();
        }
    }
}
