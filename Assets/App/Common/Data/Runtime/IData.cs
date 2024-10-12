namespace App.Common.Data.Runtime
{
    public interface IData
    {
        string Name();
        void WhenCreateNewData();
        void BeforeSerialize();
    }
}