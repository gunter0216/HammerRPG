using System.Collections.Generic;
using System.Linq;
using App.Common.Algorithms.Runtime;
using App.Common.Algorithms.Runtime.Extensions;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.Utility.Runtime.Extensions;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Door;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using App.Generation.DungeonGenerator.Runtime.Utility;
using UnityEngine;
using RectInt = App.Common.Algorithms.Runtime.RectInt;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased
{
    public class TileBasedDungeonGenerator : IDungeonGenerator
    {
        private static readonly float[] Unrotated = { 0f };
        private static readonly float[] QuarterTurns = { 0f, 90f, 180f, 270f };

        private readonly RoomCreator _roomCreator;

        private Dictionary<RoomType, List<RoomGenerationConfig>> _typeToRooms;
        private DungeonGenerationRooms _result;
        private TileBasedGenerationConfig _config;
        private List<DungeonGenerationRoom> _rooms;
        private int _iterations;

        public TileBasedDungeonGenerator(RoomCreator roomCreator)
        {
            _roomCreator = roomCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            _iterations = 0;
            _result =
                generation
                    .DungeonGenerationResult
                    .GenerationData
                    .GenerationRooms;

            _rooms = new List<DungeonGenerationRoom>();
            _result.Rooms = _rooms;

            if (!generation.TryGetConfig(out _config))
                return Optional<DungeonGeneration>.Fail();

            _typeToRooms = new Dictionary<RoomType, List<RoomGenerationConfig>>();

            foreach (var roomPresetConfig in _config.Rooms)
            {
                if (!_typeToRooms.TryGetValue(roomPresetConfig.RoomType, out var list))
                {
                    list = new List<RoomGenerationConfig>();
                    _typeToRooms.Add(roomPresetConfig.RoomType, list);
                }

                list.Add(roomPresetConfig);
            }

            if (!Generate())
                return Optional<DungeonGeneration>.Fail();

            _result.StartGenerationRoom = _rooms.First();
            _result.EndGenerationRoom = _rooms.Last();

            return Optional<DungeonGeneration>.Success(generation);
        }

        private bool Generate()
        {
            var startConfig = _typeToRooms[RoomType.Start].Random();
            var startVariant = startConfig.Variant;

            var startRoom = _roomCreator.Create(Vector2Int.Zero, startConfig, startVariant);
            startRoom.ConfigVariant = startVariant;
            startRoom.RotateEuler = 0;
            startRoom.Depth = 0;

            _rooms.Add(startRoom);

            foreach (var outputDoor in startVariant.OutputDoors)
            {
                if (!GenerateBranch(
                    startRoom,
                    new Vector2Int(outputDoor.X, outputDoor.Y),
                    0))
                    return false;
            }

            return true;
        }

        private bool GenerateBranch(
            DungeonGenerationRoom prevRoom,
            Vector2Int exitDoor,
            int depth)
        {
            if (_iterations >= _config.MaxIterations)
            {
                return false;
            }

            _iterations += 1;
            
            bool isMaxDepth = depth >= _config.MaxDepth;

            var prevDoorWorldPos = GetDoorWorldPosition(prevRoom, exitDoor);
            var prevDoorSide     = GetDoorSide(prevRoom.Size, exitDoor, prevRoom.RotateEuler);
            var sideVec          = SideToVector(prevDoorSide);
            // Door coordinates identify empty cells just outside the floor lip.
            // Join the existing floor lips directly instead of leaving two empty rows.
            var targetDoorWorldPos = prevDoorWorldPos - sideVec;

            var configs = new List<RoomGenerationConfig>();
            if (!isMaxDepth)
            {
                configs.AddRange(_typeToRooms[RoomType.Transit]);
                configs.Shuffle();
            }

            // A one-door content room can close a branch when transit rooms do not fit.
            var contentConfigs = _typeToRooms[RoomType.Content].ToList();
            contentConfigs.Shuffle();
            configs.AddRange(contentConfigs);

            foreach (var config in configs)
            {
                var variant = config.Variant;
                foreach (var rotation in _config.DisableRoomRotation ? Unrotated : QuarterTurns)
                {
                    var inputDoorSide = GetDoorSide(config.Size, config.InputDoor, rotation);

                    if (!IsOpposite(prevDoorSide, inputDoorSide))
                        continue;

                    var rotatedInputDoor = RotatePoint(config.InputDoor, config.Size, rotation);
                    var rotationOffset   = GetRotationOffset(config.Size, rotation);
                    var roomPosition     = targetDoorWorldPos + rotationOffset - rotatedInputDoor;

                    var room = _roomCreator.Create(roomPosition, config, variant);
                    room.ConfigVariant = variant;
                    room.RotateEuler   = rotation;
                    room.Depth         = depth + 1;

                    if (Intersects(room, prevRoom, prevDoorSide))
                        continue;

                    _rooms.Add(room);

                    if (config.RoomType == RoomType.Content)
                        return true;

                    // Пробуем построить все ветки от этой комнаты.
                    // Запоминаем точку отката — если что-то не получится,
                    // обрезаем _rooms до этой точки и пробуем следующую комнату.
                    if (TryBuildBranches(room, depth))
                        return true;

                    // Откат: убираем все комнаты, добавленные в этой попытке
                    RollbackTo(room);
                }
            }

            Debug.LogWarning(
                "FAILED GENERATE BRANCH\n" +
                $"Depth: {depth}\n" +
                $"ContentOnly: {isMaxDepth}\n" +
                $"PrevRoom Config: {prevRoom.GenerationConfig}\n" +
                $"PrevRoom Variant: {prevRoom.ConfigVariant.AssetReference}\n" +
                $"PrevRoom Position: {prevRoom.Position}\n" +
                $"PrevRoom Rotation: {prevRoom.RotateEuler}\n" +
                $"ExitDoor: {exitDoor}\n" +
                $"PrevDoorWorldPos: {prevDoorWorldPos}\n" +
                $"PrevDoorSide: {prevDoorSide}\n" +
                $"TargetDoorWorldPos: {targetDoorWorldPos}");

            return false;
        }

        private bool TryBuildBranches(DungeonGenerationRoom room, int depth)
        {
            foreach (var door in room.ConfigVariant.OutputDoors)
                if (!GenerateBranch(room, door, depth + 1))
                    return false;

            return true;
        }

        // Убирает из _rooms всё начиная с room включительно
        private void RollbackTo(DungeonGenerationRoom room)
        {
            int idx = _rooms.IndexOf(room);
            if (idx >= 0)
                _rooms.RemoveRange(idx, _rooms.Count - idx);
        }

        private bool Intersects(DungeonGenerationRoom room, DungeonGenerationRoom previous, Side connectionSide)
        {
            var rect1 = GetRoomRect(room);

            foreach (var other in _rooms)
            {
                var rect2 = GetRoomRect(other);

                // Bounds include the empty opening row and the wall caps. Only the
                // connected parent may share that two-cell border band.
                if (other == previous && IsConnectionBorder(rect1, rect2, connectionSide))
                    continue;

                if (rect1.Overlaps(rect2))
                    return true;
            }

            return false;
        }

        private static bool IsConnectionBorder(RectInt candidate, RectInt previous, Side side)
        {
            const int border = 2;
            switch (side)
            {
                case Side.Right: return candidate.Position.X >= previous.Position.X + previous.Width - border;
                case Side.Left: return candidate.Position.X + candidate.Width <= previous.Position.X + border;
                case Side.Top: return candidate.Position.Y >= previous.Position.Y + previous.Height - border;
                case Side.Bottom: return candidate.Position.Y + candidate.Height <= previous.Position.Y + border;
                default: return false;
            }
        }

        private RectInt GetRoomRect(DungeonGenerationRoom room)
        {
            var size   = GetRotatedSize(room.Size, room.RotateEuler);
            var offset = GetRotationOffset(room.Size, room.RotateEuler);

            return new RectInt(
                room.Position.X - offset.X,
                room.Position.Y - offset.Y,
                size.X,
                size.Y);
        }

        private Vector2Int RotatePoint(
            Vector2Int point,
            Vector2Int size,
            float rotation)
        {
            var second = point + (point.X == 0 || point.X == size.X - 1 ? Vector2Int.Up : Vector2Int.Right);
            var firstRotated = RotateCell(point, size, rotation);
            var secondRotated = RotateCell(second, size, rotation);
            return new Vector2Int(Mathf.Min(firstRotated.X, secondRotated.X), Mathf.Min(firstRotated.Y, secondRotated.Y));
        }

        private Vector2Int RotateCell(
            Vector2Int point,
            Vector2Int size,
            float rotation)
        {
            int x = point.X, y = point.Y;
            int w = size.X,  h = size.Y;

            if (Mathf.Approximately(rotation, 0))
                return new Vector2Int(x, y);

            if (Mathf.Approximately(rotation, 90))
                return new Vector2Int(y, w - 1 - x);

            if (Mathf.Approximately(rotation, 180))
                return new Vector2Int(w - 1 - x, h - 1 - y);

            // 270
            return new Vector2Int(h - 1 - y, x);
        }

        private Vector2Int GetRotationOffset(
            Vector2Int size,
            float rotation)
        {
            int w = size.X, h = size.Y;

            if (Mathf.Approximately(rotation, 0))
                return Vector2Int.Zero;

            if (Mathf.Approximately(rotation, 90))
                return new Vector2Int(0, w - 1);

            if (Mathf.Approximately(rotation, 180))
                return new Vector2Int(w - 1, h - 1);

            // 270
            return new Vector2Int(h - 1, 0);
        }

        private Vector2Int GetRotatedSize(Vector2Int size, float rotation)
        {
            if (Mathf.Approximately(rotation, 90) ||
                Mathf.Approximately(rotation, 270))
            {
                return new Vector2Int(size.Y, size.X);
            }

            return size;
        }

        private Vector2Int GetDoorWorldPosition(
            DungeonGenerationRoom room,
            Vector2Int localDoor)
        {
            var rotatedDoor = RotatePoint(localDoor, room.Size, room.RotateEuler);
            var offset      = GetRotationOffset(room.Size, room.RotateEuler);
            return room.Position - offset + rotatedDoor;
        }

        private Side GetDoorSide(
            Vector2Int size,
            Vector2Int door,
            float rotation)
        {
            var rotatedDoor = RotatePoint(door, size, rotation);
            var rotatedSize = GetRotatedSize(size, rotation);

            if (rotatedDoor.X == 0)
                return Side.Left;

            if (rotatedDoor.X == rotatedSize.X - 1)
                return Side.Right;

            if (rotatedDoor.Y == 0)
                return Side.Bottom;

            return Side.Top;
        }

        private Vector2Int SideToVector(Side side)
        {
            switch (side)
            {
                case Side.Left:   return Vector2Int.Left;
                case Side.Right:  return Vector2Int.Right;
                case Side.Top:    return Vector2Int.Up;
                case Side.Bottom: return Vector2Int.Down;
            }

            return Vector2Int.Zero;
        }

        private bool IsOpposite(Side a, Side b)
        {
            return (a == Side.Left   && b == Side.Right)
                || (a == Side.Right  && b == Side.Left)
                || (a == Side.Top    && b == Side.Bottom)
                || (a == Side.Bottom && b == Side.Top);
        }

        private bool IsSameDoor(Vector2Int a, Vector2Int b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public string GetName()
        {
            return "Tile Based";
        }
    }
}
