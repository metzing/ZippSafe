using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZippSafe.EcoMode;
using ZippSafe.EcoMode.Listeners;

namespace ZippSafe.CommandLine
{
    class DummyEmailService : IEmailService
    {
        private readonly ILogger logger;

        public DummyEmailService(ILogger logger)
        {
            this.logger = logger;
        }

        public Task SendEmail(IEnumerable<LockerState> lockerStates)
        {
            logger.Info("Sending email:");

            foreach (var state in lockerStates)
            {
                logger.Info($"Eco mode for locker {state.LockerId} has been turned {(state.RunsInEco ? "On" : "Off")}");
            }

            return Task.CompletedTask;
        }
    }
}
