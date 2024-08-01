using System;

namespace Runtime.Shared
{
    /// <summary>
    /// A timer used to record periods of time
    /// </summary>
    public sealed class Timer
    {
    #region Fields
        public DateTime StartTime {private set; get;}
        public TimeSpan DeltaTime {private set; get;}
    #endregion

    #region Properties
        public TimeSpan ElapsedTime
        {
            get {
                return DateTime.Now.Subtract(StartTime);
            }
        }
    #endregion Properties

        /// <summary>
        /// Begin the timer
        /// </summary>
        public void Start ()
        {
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        /// <returns>Returns the elapsed time</returns>
        public TimeSpan Stop ()
        {
            if (ElapsedTime.TotalMilliseconds == 0) return new(0);
            DeltaTime = ElapsedTime;
            return DeltaTime;
        }
    }
}