using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ZippSafe.EcoMode;
using ZippSafe.EcoMode.Listeners;

namespace ZippSafe.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new DummyLogger();

            var notifier = new LockerSystemNotifier(new DummyLockerSystemManager(logger), logger);

            ILockerSystemManager manager = notifier;
            IEcoModeNotificationSource notificationSource = notifier;

            notificationSource.Register<IEmailService>(new DummyEmailService(logger), (service, result) => service.SendEmail(result));

            manager.SwitchEcoOn();
        }
    }
}
