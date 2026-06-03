using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Events
{
    public class ConsoleLogger
    {
        private const ConsoleColor INFO_COLOR = ConsoleColor.Blue;
        private const ConsoleColor WARNING_COLOR = ConsoleColor.Red;
        private const ConsoleColor WARNING_TYPE_COLOR = ConsoleColor.DarkRed;
        private const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
        public void Subscribe(TransferEventHub hub)
        {
            hub.OnTransferStarted += TransferStarted;
            hub.OnSampleReceived += SampleReceived;
            hub.OnTransferCompleted += TransferCompleted;
            hub.OnWarningRaised += WarningRaised;
        }

        private void TransferStarted(object sender, TransferEventArgs e)
        {
            WriteInfo(e.Message);
        }
        private void SampleReceived(object sender, TransferEventArgs e)
        {
            WriteInfo(e.Message);
        }
        private void TransferCompleted(object sender, TransferEventArgs e)
        {
            WriteInfo(e.Message);
        }
        private void WarningRaised(object sender, TransferEventArgs e)
        {
            WriteWarning(e.WarningType, e.Message);
        }

        private void WriteWarning(WarningType type, string message)
        {
            Console.ForegroundColor = WARNING_COLOR;
            Console.Write("[WARNING]");
            Console.ForegroundColor = WARNING_TYPE_COLOR;
            Console.Write($" [{type}]");
            Console.ForegroundColor = DEFAULT_COLOR;
            Console.Write($" {message}\n");
        }

        private void WriteInfo(string message)
        {
            Console.ForegroundColor = INFO_COLOR;
            Console.Write("[INFO]");
            Console.ForegroundColor = DEFAULT_COLOR;
            Console.Write($" {message}\n");
        }
    }
}
