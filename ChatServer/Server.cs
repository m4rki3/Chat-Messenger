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
using System.Net.WebSockets;

namespace ChatServer;
public class Server
{
    private readonly IPAddress ip;
    private readonly IPEndPoint endPoint;
    private readonly Logger logger;
    public Server(string hostNameOrAddress, int serverPort, int loggerPort)
    {
        ip = Dns.GetHostAddresses(hostNameOrAddress)[0];
        endPoint = new(ip, serverPort);
        logger = new(loggerPort, hostNameOrAddress);
    }
    public void Start()
    {
        Task loggerExecution = new(
            () => logger.Start(),
            TaskCreationOptions.LongRunning
        );
        loggerExecution.Start();
        using Socket serverSocket
            = new(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(endPoint);
        serverSocket.Listen();
        while (true)
        {
            try
            {
                Console.WriteLine("...Waiting for socket connection...");
                Socket clientSocket = serverSocket.Accept();
                Task clientHandler = new(() =>
                {
                    try
                    {
                        Console.WriteLine("Client socket accepted");
                        do
                        {
                            byte[] message = new byte[256];
                            int bytesReceived = clientSocket.Receive(
                                message, SocketFlags.Partial
                            );
                            Console.WriteLine("Bytes received");
                            logger.Log += Encoding.UTF8.GetString(
                                message, 0, bytesReceived
                            );
                        } while (clientSocket.Connected);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        clientSocket.Dispose();
                    }
                }, TaskCreationOptions.LongRunning);
                clientHandler.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
