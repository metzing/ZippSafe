using System;

namespace ZippSafe.EcoMode
{
    public class LockerState
    {
        public Guid LockerId { get; init; }
        public bool RunsInEco { get; init; }
    }
}