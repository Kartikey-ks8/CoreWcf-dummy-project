using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;
using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WebAppWithSignalRAndWCF.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly HubConnection _signalRConnection;
        private readonly IHubProxy _signalRHubProxy;
        private readonly IService _wcfClient;

        public HomeController()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            var endpointAddress = new EndpointAddress("https://localhost:7028/Service.svc");

            var channelFactory = new ChannelFactory<IService>(binding, endpointAddress);
            _wcfClient = channelFactory.CreateChannel();

            _signalRConnection = new HubConnection("https://localhost:7028/");
            _signalRHubProxy = _signalRConnection.CreateHubProxy("myHub");

            _signalRHubProxy.On<string>("ReceiveMessage", (message) =>
            {
                // Handle received messages here
                ViewData["ReceivedMessage"] = message;
                Console.WriteLine("Message received from SignalR hub: " + message);
            });
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                await _signalRConnection.Start();
                ViewData["SignalRStatus"] = "Connected";
                Console.WriteLine("SignalR connection established.");
            }
            catch (Exception ex)
            {
                ViewData["SignalRStatus"] = $"Connection failed: {ex.Message}";
                Console.WriteLine("SignalR connection failed: " + ex.Message);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(string message)
        {
            try
            {
                await _signalRHubProxy.Invoke("SendMessage", message);
                ViewData["SentMessage"] = message;
                Console.WriteLine("Message sent to SignalR hub: " + message);
            }
            catch (Exception ex)
            {
                ViewData["SentMessage"] = $"Failed to send message: {ex.Message}";
                Console.WriteLine("Failed to send message: " + ex.Message);
            }

            return View("Index");
        }
    }
}
