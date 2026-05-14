using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.PvDataContracts;

namespace Client
{
    public class CsvRowParser
    {

        //Pomocna metoda, proverava polja
        public PvSample MapLineToSample(string line, int index)
        {
            try
            {
                string[] parts = line.Split(',');
                if (parts.Length < 10) return null;

                //Ovo je samo da obezbedi da ce se vrednosti ispravno citati na ma kojem racunaru (lupam, za decimalne vrednosti . umesto ,)
                var culture = CultureInfo.InvariantCulture;

                if (!DateTime.TryParse(parts[0], culture, DateTimeStyles.None, out DateTime dateValue) || !TimeSpan.TryParse(parts[1], culture, out TimeSpan timeValue))
                {
                    return null;
                }

                return new PvSample
                {
                    RowIndex = index,
                    Day = dateValue,
                    Hour = timeValue,
                    AcPwrt = Common.Constants.SentinelValues.CheckSentinel(parts[2]),
                    DcVolt = Common.Constants.SentinelValues.CheckSentinel(parts[3]),
                    Temper = Common.Constants.SentinelValues.CheckSentinel(parts[4]),
                    Vl1to2 = Common.Constants.SentinelValues.CheckSentinel(parts[5]),
                    Vl2to3 = Common.Constants.SentinelValues.CheckSentinel(parts[6]),
                    Vl3to1 = Common.Constants.SentinelValues.CheckSentinel(parts[7]),
                    AcCur1 = Common.Constants.SentinelValues.CheckSentinel(parts[8]),
                    AcVlt1 = Common.Constants.SentinelValues.CheckSentinel(parts[9]),

                };
            }
            catch
            {
                return null;
            }
        }

        public void LogRejectedRow(string line, int index)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | Row {index} | Data: {line}{Environment.NewLine}";
            File.AppendAllText("rejected_client.csv", logEntry);
        }
    }
}
