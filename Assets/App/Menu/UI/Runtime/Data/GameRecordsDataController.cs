﻿using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;

namespace App.Menu.UI.Runtime.Data
{
    public class GameRecordsDataController
    {
        private readonly IGameRecordsData m_Data;
        
        public GameRecordsDataController(IGameRecordsDataLoader dataLoader)
        {
            m_Data = dataLoader.Load();
        }

        public List<GameRecord> GetRecords()
        {
            return m_Data.GameRecords;
        }

        public bool IsRecordExists(string name)
        {
            return m_Data.GameRecords.Any(x => x.Name == name);
        }

        public void AddRecord(GameRecord record)
        {
            m_Data.GameRecords.Add(record);
        }
        
        public void RemoveRecord(string name)
        {
            for (int i = 0; i < m_Data.GameRecords.Count; ++i)
            {
                var record = m_Data.GameRecords[i];
                if (record.Name == name)
                {
                    m_Data.GameRecords.RemoveAt(i);
                    break;
                }
            }
            
            HLogger.LogError($"not found record {name}");
        }
    }
}