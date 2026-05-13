using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Config
{
    public static class ThresholdConfig
    {
        public static double OverTempThreshold =>
            double.Parse(ConfigurationManager.AppSettings["OverTempThreshold"],
                         System.Globalization.CultureInfo.InvariantCulture);

        public static double VoltageImbalancePct =>
            double.Parse(ConfigurationManager.AppSettings["VoltageImbalancePct"],
                         System.Globalization.CultureInfo.InvariantCulture);

        public static int PowerFlatlineWindow =>
            int.Parse(ConfigurationManager.AppSettings["PowerFlatlineWindow"]);

        public static double AcCur1SpikeThreshold =>
            double.Parse(ConfigurationManager.AppSettings["AcCur1SpikeThreshold"],
                         System.Globalization.CultureInfo.InvariantCulture);

        public static double DcVoltMin =>
            double.Parse(ConfigurationManager.AppSettings["DcVoltMin"],
                         System.Globalization.CultureInfo.InvariantCulture);

        public static double DcVoltMax =>
            double.Parse(ConfigurationManager.AppSettings["DcVoltMax"],
                         System.Globalization.CultureInfo.InvariantCulture);
    }
}
