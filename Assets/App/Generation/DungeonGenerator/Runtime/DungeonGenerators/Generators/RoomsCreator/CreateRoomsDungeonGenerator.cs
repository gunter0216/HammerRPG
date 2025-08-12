using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator.Config;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using Assets.App.Common.Algorithms.Runtime;
using Assets.App.Common.Algorithms.Runtime.Extensions;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator
{
    public class CreateRoomsDungeonGenerator : IDungeonGenerator
    {
        private readonly RoomCreator m_RoomCreator;
        private readonly Random m_Random;
        
        public CreateRoomsDungeonGenerator(RoomCreator roomCreator)
        {
            m_Random = new Random();
            m_RoomCreator = roomCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.DungeonGenerationResult;
            if (!generation.TryGetConfig<CreateRoomsGenerationConfig>(out var config))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            var roomsAmount = config.CountRooms;
            var rooms = new List<DungeonGenerationRoom>(roomsAmount);
            for (int i = 0; i < roomsAmount; ++i)
            {
                var position = GetRandomRoomPosition(config.Radius);
                var size = GetRandomRoomSize(config);
                var room = m_RoomCreator.Create(position, size);
                rooms.Add(room);
            }
            
            dungeon.GenerationData.GenerationRooms.Rooms = rooms;

            return Optional<DungeonGeneration>.Success(generation);
        }

        private Vector2Int GetRandomRoomSize(CreateRoomsGenerationConfig config)
        {
            var width = m_Random.Next(config.MinWidthRoom, config.MaxWidthRoom + 1);
            var height = m_Random.Next(config.MinHeightRoom, config.MaxHeightRoom + 1);
            return new Vector2Int(width, height);
        }

        private Vector2Int GetRandomRoomPosition(float radius = 1)
        {
            var randomPointInCircle = GetRandomPointInCircle(radius);
            return new Vector2Int((int)randomPointInCircle.X, (int)randomPointInCircle.Y);
        }

        private Vector2 GetRandomPointInCircle(float radius)
        {
            return GetRandomPointInCircle() * radius;
        }

        private Vector2 GetRandomPointInCircle()
        {
            return m_Random.RandomInUnitCircle();
        }

        public string GetName()
        {
            return "Create Rooms";
        }
    }
}