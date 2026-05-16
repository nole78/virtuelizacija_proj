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

        public PvSample MapLineToSample(string line, int index)
        {
            try
            {
                string[] parts = line.Split(',');
                if (parts.Length < 11) return null;

                //Ovo je samo da obezbedi da ce se vrednosti ispravno citati na ma kojem racunaru (lupam, za decimalne vrednosti . umesto ,)
                var culture = CultureInfo.InvariantCulture;

                if (!DateTime.TryParseExact(parts[1], "yyyyddd", culture, DateTimeStyles.None, out DateTime dateValue) || !TimeSpan.TryParseExact(parts[2], @"hh\:mm\:ss", culture, out TimeSpan timeValue))
                {
                    return null;
                }

                return new PvSample
                {
                    RowIndex = int.Parse(parts[0]),
                    Day = dateValue.Date,       //<= ovde prosledjujem samo date, vreme je vec u hour polju
                    Hour = timeValue,
                    AcPwrt = Common.Constants.SentinelValues.CheckSentinel(parts[3]),
                    DcVolt = Common.Constants.SentinelValues.CheckSentinel(parts[4]),
                    Temper = Common.Constants.SentinelValues.CheckSentinel(parts[5]),
                    Vl1to2 = Common.Constants.SentinelValues.CheckSentinel(parts[6]),
                    Vl2to3 = Common.Constants.SentinelValues.CheckSentinel(parts[7]),
                    Vl3to1 = Common.Constants.SentinelValues.CheckSentinel(parts[8]),
                    AcCur1 = Common.Constants.SentinelValues.CheckSentinel(parts[9]),
                    AcVlt1 = Common.Constants.SentinelValues.CheckSentinel(parts[10]),

                };
            }
            catch
            {
                return null;
            }
        }
    }
}
