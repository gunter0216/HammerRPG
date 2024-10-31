namespace App.Common.Time.Runtime
{
    public interface ITimeToStringConverter
    {
        string ReturnTimeToShow(long time);
        string ReturnTimeToShow(float timeLeft);
        string ReturnFullTimeToShow(float timeLeft);
    }
}