using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: dodaj konfiguraciju, parser i onda pokreni ovo da radi
            // TODO: dodaj ostala polja u meta i pravu putanju ka fajlu
            var meta = new PvMeta { FileName = "data.csv"};
            var service = new TransferService(meta);

            string izbor = "";
            while(izbor != "KRAJ")
                switch (PrintMenu())
                {
                    case "1":
                        service.Run();
                        break;
                    case "2":
                        service.RunWithSimulatedFailure();
                        break;
                    default:
                        Console.WriteLine("Nepoznat izbor");
                        break;
                }
        }

        private static string PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1 - Normalan prenos");
            Console.WriteLine("2 - Simulacija prekida");
            Console.WriteLine("KRAJ - Izlaz");
            return Console.ReadLine();
        }
    }
}
