using System;

namespace Runtime.Shared
{
    [Serializable]
    public struct TimeStamp
    {
    
    #region Fields
        public int date;
        public int month;
        public int year;
    #endregion Fields

        public TimeStamp (int date, int month, int year)
        {
            this.date = date;
            this.month = month;
            this.year = year;
        }

        public override readonly string ToString ()
        {
            return Convert.ToString(date) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
        }
    }
}