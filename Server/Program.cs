using Common;
using Server.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TransferEventHub hub = new TransferEventHub();
            ConsoleLogger logger = new ConsoleLogger();
            logger.Subscribe(hub);

            PvDataService service = new PvDataService(hub);
            ServiceHost host = new ServiceHost(service);
            host.Open();

            Console.WriteLine("[SERVICE OPEN]");
            Console.ReadKey();

            host.Close();
            Console.WriteLine("[SERVICE CLOSED]");
        }
    }
}
