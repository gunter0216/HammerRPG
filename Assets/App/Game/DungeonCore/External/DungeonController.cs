using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DungeonCore.External.Controllers;
using App.Game.DungeonCore.External.Services;
using App.Game.DungeonCreator.Runtime.Rooms;
using App.Generation.DungeonCreator.Runtime;

namespace App.Game.DungeonCore.External
{
    public class DungeonController : IDungeonController, IInitSystem
    {
        private readonly IDungeonCreator _dungeonCreator;
        private readonly IItemSpriteLoader _spriteLoader;

        private List<RoomController> _rooms;
        private DungeonService _service;
        
        public DungeonController(IDungeonCreator dungeonCreator, IItemSpriteLoader spriteLoader)
        {
            _dungeonCreator = dungeonCreator;
            _spriteLoader = spriteLoader;
        }

        public void Init()
        {
            if (!CreateDungeon())
            {
                return;
            }
        }

        private bool CreateDungeon()
        {
            if (!CreateService())
            {
                return false;
            }

            if (!CreateControllers())
            {
                return false;
            }
            
            return true;
        }

        private bool CreateControllers()
        {
            _rooms = new List<RoomController>(_service.Rooms.Count);
            foreach (var room in _service.Rooms)
            {
                var controller = new RoomController(room, _spriteLoader);
                _rooms.Add(controller);
            }

            foreach (var room in _rooms)
            {
                room.Initialize();
            }

            return true;
        }

        private bool CreateService()
        {
            var dungeon = _dungeonCreator.Create();
            if (!dungeon.HasValue)
            {
                HLogger.LogError("Cant create dungeon");
                return false;
            }
            
            var rooms = new List<RoomService>(16);
            _service = new DungeonService(dungeon.Value, rooms);

            foreach (var room in dungeon.Value.Rooms)
            {
                var roomService = CreateRoom(room);
                if (!roomService.HasValue)
                {
                    HLogger.LogError("Cant create room");
                    return false;
                }
                
                rooms.Add(roomService.Value);
            }

            _service.Initialize();

            return true;
        }

        private Optional<RoomService> CreateRoom(Room room)
        {
            var roomService = new RoomService(room);
            
            return Optional<RoomService>.Success(roomService);
        }
    }
}