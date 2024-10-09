namespace App.Common.Utility.Runtime.Time
{
    public class TimeToStringConverter : ITimeToStringConverter
    {
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
    }
}