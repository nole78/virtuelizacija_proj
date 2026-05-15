using Client.Csv;
using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            string csvPath = ConfigurationManager.AppSettings["CsvFilePath"];
            int limitN = int.Parse(ConfigurationManager.AppSettings["RowLimitN"]);
            var meta = new PvMeta { FileName = csvPath,RowLimitN = limitN};
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
            Console.WriteLine("SOLARNI PANELI");
            Console.WriteLine("1 - Normalan prenos");
            Console.WriteLine("2 - Simulacija prekida");
            Console.WriteLine("KRAJ - Izlaz");
            return Console.ReadLine();
        }
    }
}
