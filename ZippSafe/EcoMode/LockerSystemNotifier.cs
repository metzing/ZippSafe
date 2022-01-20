using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZippSafe.EcoMode
{
    /// <summary>
    /// Handles the [de]registering and [de]activating of listeners and their notification
    /// </summary>
    public class LockerSystemNotifier : ILockerSystemManager, IEcoModeNotificationSource
    {
        private readonly Dictionary<Type, EcoModeSubscription> listeners = new Dictionary<Type, EcoModeSubscription>();
        private readonly ILockerSystemManager decorated;
        private readonly ILogger logger;

        public LockerSystemNotifier(ILockerSystemManager decorated, ILogger logger)
        {
            this.decorated = decorated;
            this.logger = logger;
        }

        #region implementing IEcoModeNotificationSource

        public void Activate<T>()
        {
            listeners[typeof(T)].IsActive = true;
        }

        public void Deactivate<T>()
        {
            listeners[typeof(T)].IsActive = false;
        }

        public void Deregister<T>()
        {
            listeners.Remove(typeof(T));
        }

        public void Register<T>(T instance, Func<T, IEnumerable<LockerState>, Task> onEcoModeToggle)
        {
            listeners[typeof(T)] = new EcoModeSubscription(
                lockerStates => onEcoModeToggle(instance, lockerStates));
        }

        #endregion

        #region implementing ILockerSystemManager

        public Task<IEnumerable<LockerState>> SwitchEcoOff() => NotifyListeners(decorated.SwitchEcoOff);

        public Task<IEnumerable<LockerState>> SwitchEcoOn() => NotifyListeners(decorated.SwitchEcoOn);

        private async Task<IEnumerable<LockerState>> NotifyListeners(Func<Task<IEnumerable<LockerState>>> withDecorated)
        {
            var decoratedResult = await withDecorated();

            var activeListeners = listeners.Where(listener => listener.Value.IsActive);

            // consider running these in parallel
            foreach (var (type, listener) in activeListeners) 
            {
                logger.Info($"Notifying listener {type.Name}");

                try
                {
                    await listener.Notify(decoratedResult);
                }
                catch (Exception e)
                {
                    logger.Error($"Error during notification of listener {type.Name}", e);
                }
            }

            return decoratedResult;
        }

        #endregion
    }
}
