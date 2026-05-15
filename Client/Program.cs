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
      /*
            // Sljanje do N redova serveru u petlji
            string csvPath = ConfigurationManager.AppSettings["CsvFilePath"];
            int limitN = int.Parse(ConfigurationManager.AppSettings["RowLimitN"]);

            var samples = new List<PvSample>();
            var parser = new CsvRowParser();

            using (var reader = new CsvReader(csvPath))
            {
                reader.ReadLine();
                int currentRow = 1;
                while (samples.Count < limitN)
                {
                    string line = reader.ReadLine();
                    if (line == null) break; // Kraj fajla

                    var sample = parser.MapLineToSample(line, currentRow);

                    if (sample != null)
                        samples.Add(sample);
                    else
                        parser.LogRejectedRow(line, currentRow);

                    currentRow++;
                }
            }
        */
    }
}
