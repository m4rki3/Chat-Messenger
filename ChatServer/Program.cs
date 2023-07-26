using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer;
public class Program
{
    public static void Main()
    {
        Server server = new Server("127.0.0.1", 11000, 11001);
        server.Start();
    }
}