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

                DateTime? dateValue = ParseJulianDate(parts[1], culture);
                if (dateValue == null)
                {
                    return null;
                }
                if (!TimeSpan.TryParseExact(parts[2], @"hh\:mm\:ss", culture, out TimeSpan timeValue))
                {
                    return null;
                }

                return new PvSample
                {
                    RowIndex = int.Parse(parts[0]),
                    Day = (DateTime)dateValue,
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

        //Pomocna metoda, ni ispravka nije radila :(
        private DateTime? ParseJulianDate(string value, System.Globalization.CultureInfo culture)
        {
            //Cak i ako je dan, recimo, 12. u godini, format ce svejedno imati 7 cifara -> "2026012" 
            if (value.Length != 7) return null;

            if (!int.TryParse(value.Substring(0, 4), out int year)) return null;
            if (!int.TryParse(value.Substring(4, 3), out int dayOfYear)) return null;

            if (dayOfYear < 1 || dayOfYear > 366) return null;

            try
            {
                return new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
            }
            catch
            {
                return null;
            }
        }
    }
}
