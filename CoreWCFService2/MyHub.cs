using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CoreWCFService2
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
