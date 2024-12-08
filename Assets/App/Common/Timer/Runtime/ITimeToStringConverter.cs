namespace App.Common.Timer.Runtime
{
    public interface ITimeToStringConverter
    {
        string ReturnTimeToShow(long time);
        string ReturnTimeToShow(float timeLeft);
        string ReturnFullTimeToShow(float timeLeft);
    }
}