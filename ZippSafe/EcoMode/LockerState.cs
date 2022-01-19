using System;

namespace ZippSafe.EcoMode
{
    internal class LockerState
    {
        public Guid LockerId { get; init; }
        public bool RunsInEco { get; init; }
    }
}