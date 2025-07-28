using App.Common.Utility.Runtime;
using App.Game.GameManagers.External;
using App.Game.GameTiles.External.Config.Dto;
using App.Game.GameTiles.External.Config.Model;
using TilesConfig = App.Game.GameTiles.External.Config.Model.TilesConfig;

namespace App.Game.GameTiles.External.Config.Converter
{
    public class TilesDtoToConfigConverter
    {
        public Optional<TilesConfig> Convert(TilesConfigDto dto)
        {
            var tiles = new TileConfig[dto.Tiles.Count];
            for (int i = 0; i < dto.Tiles.Count; ++i)
            {
                var tileDto = dto.Tiles[i];
                tiles[i] = new TileConfig(
                    id: tileDto.ID, 
                    generationID: tileDto.GenerationID, 
                    spriteKey: tileDto.SpriteKey);
            }
            var config = new TilesConfig(tiles: tiles);
            return Optional<TilesConfig>.Success(config);
        }
    }
}