using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippSafe.EcoMode
{
    /// <summary>
    /// A dummy implementation of the <see cref="ILockerSystemManager" /> interface
    /// </summary>
    class DummyLockerSystemManager : ILockerSystemManager
    {
        private readonly ILogger logger;

        public DummyLockerSystemManager(ILogger logger)
        {
            this.logger = logger;
        }

        public Task<IEnumerable<LockerState>> SwitchEcoOff() => SwitchEco(on: false);

        public Task<IEnumerable<LockerState>> SwitchEcoOn() => SwitchEco(on: true);

        private Task<IEnumerable<LockerState>> SwitchEco(bool on)
        {
            logger.Info($"Switching Eco mode {(on ? "On" : "Off")}");

            var lockerIds = new[]
            {
                Guid.Parse("808ebce1-be47-4e1b-a876-314bd36ec7dd"),
                Guid.Parse("b896a224-b188-466c-9819-616c54697d7e"),
                Guid.Parse("fec49152-647a-48d9-87fb-bb30e65a14a8"),
                Guid.Parse("8c5160c0-bac0-4e5e-a6c0-61e92a0f592e"),
                Guid.Parse("7e61ee6f-f71d-46dd-9a54-c65c03316803"),
            };

            var result = lockerIds.Select(lockerId => new LockerState
            {
                LockerId = lockerId,
                RunsInEco = on,
            });

            return Task.FromResult(result);
        }
    }
}
