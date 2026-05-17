using Client.Csv;
using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvPath = ConfigurationManager.AppSettings["CsvFilePath"];
            int limitN = int.Parse(ConfigurationManager.AppSettings["RowLimitN"]);
            int totalRows = 0;
            if (File.Exists(csvPath))
            {
                totalRows = File.ReadLines(csvPath).Count() - 1;
            }
            else
            {
                Console.WriteLine($"[ERROR] Fajl na putanji {csvPath} ne postoji!");
                Console.ReadKey();
                return;
            }
            //Nemam blage veze sta da radim sa SchemaVersion-om
            var meta = new PvMeta { FileName = csvPath, TotalRows = totalRows, SchemaVersion = "1.0", RowLimitN = limitN};

            var service = new TransferService(meta);

            string izbor = "";
            while(!izbor.Equals("KRAJ"))
                switch (PrintMenu())
                {
                    case "1":
                        service.Run();
                        break;
                    case "2":
                        service.RunWithSimulatedFailure();
                        break;
                    case "KRAJ":
                        izbor = "KRAJ";
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
