using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using ServiceReference1;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;


namespace WebAppWithSignalRAndWCF.Controllers
{
    public class HomeController : Controller
    {
        private readonly HubConnection _signalRConnection;
        private readonly IService _wcfClient;

        public HomeController()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            var endpointAddress = new EndpointAddress("https://localhost:7028/Service.svc");

            var channelFactory = new ChannelFactory<IService>(binding, endpointAddress);
            _wcfClient = channelFactory.CreateChannel();

            // Initialize SignalR client
            _signalRConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7028/myHub")
                .Build();

            _signalRConnection.On<string>("ReceiveMessage", (message) =>
            {
                // Handle received messages here
                ViewData["ReceivedMessage"] = message;
                Console.WriteLine("Message received from SignalR hub: " + message);
            });
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await _signalRConnection.StartAsync();
                ViewData["SignalRStatus"] = "Connected";
                Console.WriteLine("SignalR connection established.");
            }
            catch (Exception ex)
            {
                ViewData["SignalRStatus"] = $"Connection failed: {ex.Message}";
                Console.WriteLine("SignalR connection failed: " + ex.Message);
            }

            return View("Index");
        }

  
        [HttpPost("sendmessage")]
        public IActionResult SendMessage(string message)
        {
            try
            {
                _signalRConnection.InvokeAsync("SendMessage", message);
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
