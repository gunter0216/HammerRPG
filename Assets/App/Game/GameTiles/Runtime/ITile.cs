using App.Game.GameTiles.External.Config.Model;
using App.Game.GameTiles.Runtime.Data;

namespace App.Game.GameTiles.Runtime
{
    public interface ITile
    {
        TileData Data { get; }
        TileConfig Config { get; }
    }
}