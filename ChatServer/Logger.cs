using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatServer;
public class Logger
{
    private readonly IPAddress ipAddress;
    private readonly IPEndPoint endPoint;
    public string Log { get; internal set; }
    public Logger(int port, string hostNameOrAddress)
    {
        ipAddress = Dns.GetHostAddresses(hostNameOrAddress)[0];
        endPoint = new(ipAddress, port);
        Log = string.Empty;
    }
    public void Start()
    {
        using Socket logSocket
            = new(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        logSocket.Bind(endPoint);
        logSocket.Listen();
        while (true)
        {
            try
            {
                Socket clientSocket = logSocket.Accept();
                Console.WriteLine("Log client socket has been accepted.");
                Task clientHandler = new(() =>
                {
                    try
                    {
                        do
                        {
                            Thread.Sleep(1500);
                            clientSocket.Send(
                                Encoding.UTF8.GetBytes(Log),
                                SocketFlags.Partial
                            );
                        }
                        while (clientSocket.Connected);
                        clientSocket.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        clientSocket.Dispose();
                    }
                });
                clientHandler.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
