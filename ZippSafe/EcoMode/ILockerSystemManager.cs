using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippSafe.EcoMode
{
    public interface ILockerSystemManager
    {
        Task<IEnumerable<LockerState>> SwitchEcoOn();
        Task<IEnumerable<LockerState>> SwitchEcoOff();
    }
}
