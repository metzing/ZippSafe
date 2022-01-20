using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippSafe.EcoMode
{
    class EcoModeSubscription
    {
        private readonly Func<IEnumerable<LockerState>, Task> onEcoModeToggle;

        public bool IsActive { get; set; } = true;

        public EcoModeSubscription(Func<IEnumerable<LockerState>, Task> onEcoModeToggle)
        {
            this.onEcoModeToggle = onEcoModeToggle;
        }

        public Task Notify(IEnumerable<LockerState> lockerStates)
        {
            return onEcoModeToggle(lockerStates);
        }
    }
}
