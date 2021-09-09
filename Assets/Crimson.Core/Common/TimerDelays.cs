using System;

namespace Assets.Crimson.Core.Common
{
    [Serializable]
    public struct TimerDelays
    {
        public float Delay;
        public float StartupDelay;

        public bool IsEmpty => Delay == StartupDelay && Delay == 0;
    }
}