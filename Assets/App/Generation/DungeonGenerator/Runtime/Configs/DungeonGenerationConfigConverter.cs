using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased;
using RuntimeDungeonGenerationConfig = App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation.DungeonGenerationConfig;
using DungeonGenerationSettings = App.Generation.DungeonGenerator.Runtime.Configs.Settings.DungeonGenerationConfig;

namespace App.Generation.DungeonGenerator.Runtime.Configs
{
    public class DungeonGenerationConfigConverter
    {
        public bool TryConvert(DungeonGenerationSettings settings, out RuntimeDungeonGenerationConfig config)
        {
            config = null;
            var source = settings?.TileBasedGenerationConfig;
            if (source == null || string.IsNullOrWhiteSpace(settings.Key) || source.Rooms == null || source.Rooms.Length == 0 ||
                source.MaxDepth < 0 || source.MaxIterations < 1)
                return false;

            var rooms = new RoomGenerationConfig[source.Rooms.Length];
            var ids = new HashSet<string>();
            var types = new HashSet<RoomType>();
            for (var i = 0; i < rooms.Length; i++)
            {
                var room = source.Rooms[i];
                if (room == null || !room.TryCreateRuntimeConfig(out rooms[i]) || !ids.Add(room.Id))
                    return false;
                types.Add(room.RoomType);
            }
            if (!types.Contains(RoomType.Start) || !types.Contains(RoomType.Transit) || !types.Contains(RoomType.Content))
                return false;

            config = new RuntimeDungeonGenerationConfig(
                settings.Key,
                new IGenerationConfig[] { new TileBasedGenerationConfig(rooms, source.MaxDepth, source.MaxIterations) });
            return true;
        }
    }
}
