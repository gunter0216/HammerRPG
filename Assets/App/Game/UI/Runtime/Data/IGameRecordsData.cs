using System.Collections.Generic;

namespace App.Game.UI.Runtime.Data
{
    public interface IGameRecordsData
    {
        List<GameRecord> GameRecords { get; set; }
    }
}