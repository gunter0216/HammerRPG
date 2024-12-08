using System;

namespace App.Common.Timer.Runtime
{
    public class TimeToStringConverter : ITimeToStringConverter
    {
        private readonly string m_Days;
        private readonly string m_Hours;
        private readonly string m_Minutes;
        private readonly string m_Seconds;

        public TimeToStringConverter()
        {
            // todo localize
            m_Days = "d";
            m_Hours = "h";
            m_Minutes = "m";
            m_Seconds = "s";
        }
        
        public string ReturnTimeToShow(long time)
        {
            var date = new DateTime(time);
            return date.ToString();
        }
        
        // public string ReturnTimeToShow(long time)
        // {
        //     var date = new DateTime(time).Subtract(DateTime.MinValue);
        //     
        //     if (date.Days > 0)
        //     {
        //         return $"{date.Days}{m_Days} {date.Hours}{m_Hours}";
        //     } 
        //     
        //     if (date.Hours > 0)
        //     {
        //         return $"{date.Hours}{m_Hours} {date.Minutes}{m_Minutes}";
        //     }
        //     
        //     return $"{date.Minutes}{m_Minutes} {date.Seconds}{m_Seconds}";
        // }

        public string ReturnTimeToShow(float timeLeft)
        {
            string timeToShow;
            
            // if ((int)(timeLeft / Seconds.Day) > 0)
            // {
            //     timeToShow = (int)(timeLeft / Seconds.Day) +
            //                  "days".Localize() +
            //                  " " +
            //                  (int)(timeLeft / Seconds.Hour % 24) +
            //                  "hours".Localize();
            // } 
            // else if ((int)(timeLeft / Seconds.Hour) > 0)
            // {
            //     timeToShow = (int)(timeLeft / Seconds.Hour) +
            //                  "hours".Localize() +
            //                  " " +
            //                  (int)(timeLeft / Seconds.Minute % Seconds.Minute) +
            //                  "minutes".Localize();
            // }
            // else
            // {
            //     timeToShow = (int)(timeLeft / Seconds.Minute % Seconds.Minute) +
            //                  "minutes".Localize() +
            //                  " " +
            //                  (int)(timeLeft % Seconds.Minute) +
            //                  "seconds".Localize();
            // }
            

            return "";
        }

        public string ReturnFullTimeToShow(float timeLeft)
        {
            // int hour = (int)(timeLeft / Seconds.Hour);
            // int minute = (int)(timeLeft / Seconds.Minute % Seconds.Minute);
            // int seconds = (int)(timeLeft - (Seconds.Hour * hour) - (Seconds.Minute * minute));
            // string timeToShow =  hour +
            //                      "hours".Localize() +
            //                      " " +
            //                      minute +
            //                      "minutes".Localize() +
            //                      " " +
            //                      seconds +
            //                      "seconds".Localize();
            //
            // return timeToShow;
            return "";
        }
    }
}