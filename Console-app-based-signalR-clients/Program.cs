/*using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using ServiceReference1;

class Program
{
    static async Task Main(string[] args)
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        var endpointAddress = new EndpointAddress("https://localhost:7028/Service.svc");

        var channelFactory = new ChannelFactory<IService>(binding, endpointAddress);
        IService wcfClient = channelFactory.CreateChannel();

        try
        {
            // string result = wcfClient.GetData(123);
            // Console.WriteLine("GetData result: " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        finally
        {
            ((IClientChannel)wcfClient).Close();
        }

        var signalRConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7028/myHub")
            .Build();

        signalRConnection.On<string>("ReceiveMessage", (message) =>
        {
            Console.WriteLine("Message received from SignalR hub: " + message);
        });

        try
        {
            await signalRConnection.StartAsync();
            Console.WriteLine("SignalR connection started.");

            var inputTask = Task.Run(async () =>
            {
                while (true)
                {
                    var message = Console.ReadLine();


                    if (string.IsNullOrWhiteSpace(message))
                    {
                        break;
                    }

                    await signalRConnection.InvokeAsync("SendMessage", message);
                }
            });

            Console.WriteLine("You can now type messages. Press Enter without typing anything to exit.");


            await inputTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred with SignalR: " + ex.Message);
        }
        finally
        {
            await signalRConnection.StopAsync();
            Console.WriteLine("SignalR connection stopped.");
        }
    }
}
*/