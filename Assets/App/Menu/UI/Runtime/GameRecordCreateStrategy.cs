using App.Common.Timer.Runtime;
using App.Menu.UI.Runtime.Data;

namespace App.Menu.UI.Runtime
{
    public class GameRecordCreateStrategy
    {
        private readonly GameRecordsDataController m_DataController;

        public GameRecordCreateStrategy(GameRecordsDataController dataController)
        {
            m_DataController = dataController;
        }

        public GameRecordCreateStatus Create(string name)
        {
            if (m_DataController.IsRecordExists(name))
            {
                return GameRecordCreateStatus.NameIsExists;
            }
            
            var record = new GameRecord()
            {
                Name = name,
                DateOfCreation = TimeHelper.Now.Ticks,
                LastLogin = TimeHelper.Now.Ticks
            };
            
            m_DataController.AddRecord(record);
            
            return GameRecordCreateStatus.Successful;
        }
    }
}