using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Game.UI.Runtime.Data;

namespace App.Game.UI.External.Data
{
    public class GameRecordsDataLoader : IGameRecordsDataLoader
    {
        private readonly IDataManager m_DataManager;

        public GameRecordsDataLoader(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public IGameRecordsData Load()
        {
            var data = m_DataManager.GetData(nameof(GameRecordsData)).Value as GameRecordsData;
            if (data == default)
            {
                HLogger.LogError("cant load data GameRecordsData");
                return null;
            }

            return data;
        }
    }
}