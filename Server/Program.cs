using Common;
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
            PvDataService service = new PvDataService();
            ServiceHost host = new ServiceHost(service);
            host.Open();

            Console.WriteLine("[SERVICE OPEN]");
            Console.ReadKey();

            host.Close();
            Console.WriteLine("[SERVICE CLOSED]");
        }
    }
}
