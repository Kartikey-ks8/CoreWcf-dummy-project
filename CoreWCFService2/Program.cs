using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(CoreWCFService2.Program))]
namespace CoreWCFService2
{
    public class Program
    {

        public void Configuration(IAppBuilder app)
        {
      
            app.MapSignalR();
        }

        public static void Main(string[] args)
        {
        

            var builder = WebApplication.CreateBuilder(args);

         
            builder.Services.AddServiceModelServices();
            builder.Services.AddServiceModelMetadata();
            builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

     
            builder.Services.AddSignalR();

            // Register CORS policy 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Use CORS policy
            app.UseCors("AllowAll");

           

            app.UseServiceModel(serviceBuilder =>
            {
                serviceBuilder.AddService<Service>();
                serviceBuilder.AddServiceEndpoint<Service, IService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service.svc");
                var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpsGetEnabled = true;
            });

           
            app.MapHub<MyHub>("/myHub");

            app.UseStaticFiles(); 

            app.Run();
        }
    }
}
