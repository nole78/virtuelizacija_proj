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
        }
    }
}
