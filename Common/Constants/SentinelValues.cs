using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public static class SentinelValues
    {
        public const double NoData = 32767.0;

        public static double? MapToNull(double value) // Koristi se prilikom parsiranja da se sentinel vrednost mapira na null  (citanje u klijentu)
            => value == NoData ? (double?)null : value;
    }
}
