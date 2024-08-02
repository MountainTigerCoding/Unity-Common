using System;

namespace Runtime.Shared
{
    [Serializable, Obsolete("Use new class called 'TimeStamp")]
    public class SavedTime
    {

    #region Fields
        public string time;
        public string date;

        public int savedMillisecond;

        public int savedSecond;
        public int savedMinute;
        public int savedHour;
        public int savedDate;
        public int savedMonth;
        public int savedYear;
    #endregion Field


        public void SetToCurrentTime ()
        {
            savedMillisecond = DateTime.Now.Millisecond;
            savedSecond = DateTime.Now.Second;
            savedMinute = DateTime.Now.Minute;
            savedHour = DateTime.Now.Hour;
            savedDate = DateTime.Now.Day;
            savedMonth = DateTime.Now.Month;
            savedYear = DateTime.Now.Year;

            string savedMinute_;
            if(savedMinute < 9) savedMinute_ = "0"+ savedMinute;
            else savedMinute_ = ""+ savedMinute;

            time = savedHour % 12 +":"+ savedMinute_ +":"+ savedSecond;
            date = savedDate +"/"+ savedMonth +"/"+ savedYear;
        }
    }
}