using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippSafe.EcoMode
{
    public interface IEcoModeNotificationSource
    {
        void Register<T>(T instance, Func<T, IEnumerable<LockerState>, Task> onEcoModeToggle);
        void Deregister<T>();

        void Activate<T>();
        void Deactivate<T>();
    }
}
