using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EquipmentReservations.AzureFunctions
{
  public class Program
  {
    public static void Main()
    {
      var host = new HostBuilder()
          .ConfigureFunctionsWorkerDefaults()
          .ConfigureServices((context, services) =>
          {
            var conn = context.Configuration["SERVICEBUS_CONNECTION"];
            if (string.IsNullOrEmpty(conn))
              throw new InvalidOperationException("SERVICEBUS_CONNECTION nie jest ustawione!");

            services.AddSingleton(new ServiceBusClient(conn));
          })
          .Build();

      host.Run();
    }
  }
}
