using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippSafe.CommandLine
{
    class DummyLogger : ILogger
    {
        public void Error(string message, Exception exception)
        {
            Console.WriteLine($"Exception! {message}");
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.StackTrace);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
