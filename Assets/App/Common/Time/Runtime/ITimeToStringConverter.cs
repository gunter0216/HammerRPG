namespace App.Common.Utility.Runtime.Time
{
    public interface ITimeToStringConverter
    {
        string ReturnTimeToShow(float timeLeft);
        string ReturnFullTimeToShow(float timeLeft);
    }
}