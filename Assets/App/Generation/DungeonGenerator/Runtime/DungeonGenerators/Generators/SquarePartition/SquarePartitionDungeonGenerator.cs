using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SquarePartition
{
    public class SquarePartitionDungeonGenerator : IDungeonGenerator
    {
        private readonly RoomCreator _roomCreator;

        public SquarePartitionDungeonGenerator(RoomCreator roomCreator)
        {
            _roomCreator = roomCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.DungeonGenerationResult;
            if (!generation.TryGetConfig<SquarePartitionGenerationConfig>(out var config))
            {
                Debug.LogError("Not found config.");
                return Optional<DungeonGeneration>.Fail();
            }

            var area = new SquareArea(new Vector2Int(0, 0), new Vector2Int(config.Size.Width, config.Size.Height));
            var areas = new List<SquareArea>();
            Split(
                areas,
                area,
                config,
                config.Depth
            );

            var areaRooms = CreateRooms(areas, config.MinRoomSize, config.MaxRoomSize, config.AreaPadding);
            
            var cash = new SquareGenerationCash(areas);
            generation.AddCash(cash);
            
            var roomsAmount = areaRooms.Count;
            var rooms = new List<DungeonGenerationRoom>(roomsAmount);
            foreach (var areaRoom in areaRooms)
            {
                var room = _roomCreator.Create(areaRoom.Position, areaRoom.Size);
                rooms.Add(room);
            }
            
            dungeon.GenerationData.GenerationRooms.Rooms = rooms;

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void Split(
            List<SquareArea> areas, 
            SquareArea area, 
            SquarePartitionGenerationConfig config,
            int depth)
        {
            int width = area.Size.X;
            int height = area.Size.Y;

            var minSize = config.MinAreaSize;
            var offset = config.Offset;
            // 🔹 Условие остановки
            if (depth <= 0 || width < minSize * 2 || height < minSize * 2)
            {
                areas.Add(area);
                return;
            }

            // 🔹 Выбор направления с учётом соотношения сторон
            bool splitVertical;

            float ratio = (float)width / height;

            if (ratio > 1.25f)
                splitVertical = true;
            else if (ratio < 0.75f)
                splitVertical = false;
            else
                splitVertical = Random.value > 0.5f;

            // 🔹 Смещение около центра (чтобы было "примерно равное")
            float splitPercent = Random.Range(0.5f - offset, 0.5f + offset);

            if (splitVertical)
            {
                int splitX = Mathf.RoundToInt(width * splitPercent);

                // защита от слишком маленьких частей
                if (splitX < minSize || width - splitX < minSize)
                {
                    areas.Add(area);
                    return;
                }

                var left = new SquareArea(
                    area.Position,
                    new Vector2Int(splitX, height)
                );

                var right = new SquareArea(
                    new Vector2Int(area.Position.X + splitX, area.Position.Y),
                    new Vector2Int(width - splitX, height)
                );

                Split(areas, left, config, depth - 1);
                Split(areas, right, config, depth - 1);
            }
            else
            {
                int splitY = Mathf.RoundToInt(height * splitPercent);

                if (splitY < minSize || height - splitY < minSize)
                {
                    areas.Add(area);
                    return;
                }

                var top = new SquareArea(
                    area.Position,
                    new Vector2Int(width, splitY)
                );

                var bottom = new SquareArea(
                    new Vector2Int(area.Position.X, area.Position.Y + splitY),
                    new Vector2Int(width, height - splitY)
                );

                Split(areas, top, config, depth - 1);
                Split(areas, bottom, config, depth - 1);
            }
        }
        
        private List<SquareArea> CreateRooms(List<SquareArea> areas, int minRoomSize, int maxRoomSize, int padding)
        {
            var rooms = new List<SquareArea>();

            foreach (var area in areas)
            {
                int maxWidth = area.Size.X - padding * 2;
                int maxHeight = area.Size.Y - padding * 2;

                // если область слишком маленькая — пропускаем
                if (maxWidth < minRoomSize || maxHeight < minRoomSize)
                    continue;

                // 🔹 размер комнаты (не больше minRoomSize)
                int roomWidth = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, maxWidth) + 1);
                int roomHeight = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, maxHeight) + 1);

                // 🔹 допустимые границы центра
                int minX = area.Position.X + padding + roomWidth / 2;
                int maxX = area.Position.X + area.Size.X - padding - roomWidth / 2;

                int minY = area.Position.Y + padding + roomHeight / 2;
                int maxY = area.Position.Y + area.Size.Y - padding - roomHeight / 2;

                if (minX > maxX || minY > maxY)
                    continue;

                // 🔹 случайный центр
                int centerX = Random.Range(minX, maxX + 1);
                int centerY = Random.Range(minY, maxY + 1);

                // 🔹 позиция (левый нижний угол)
                int x = centerX - roomWidth / 2;
                int y = centerY - roomHeight / 2;

                var room = new SquareArea(
                    new Vector2Int(x, y),
                    new Vector2Int(roomWidth, roomHeight)
                );

                rooms.Add(room);
            }

            return rooms;
        }

        public string GetName()
        {
            return "Square Partition";
        }
    }
}