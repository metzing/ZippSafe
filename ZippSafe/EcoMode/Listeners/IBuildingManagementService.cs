using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippSafe.EcoMode.Listeners
{
    interface IBuildingManagementService
    {
        Task ManagerLockerStateChanges(IEnumerable<LockerState> lockerStates);
    }
}
