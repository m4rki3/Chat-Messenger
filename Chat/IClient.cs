using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient;
public interface IClient : IDisposable
{
    public bool Connected { get; }
    public void SendMessageToServer(string name, string message);
    public string ReceiveChatLog();
}
