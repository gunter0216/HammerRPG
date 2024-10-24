namespace App.Common.Utility.Runtime.Time
{
    public interface ITimeToStringConverter
    {
        string ReturnTimeToShow(long time);
        string ReturnTimeToShow(float timeLeft);
        string ReturnFullTimeToShow(float timeLeft);
    }
}