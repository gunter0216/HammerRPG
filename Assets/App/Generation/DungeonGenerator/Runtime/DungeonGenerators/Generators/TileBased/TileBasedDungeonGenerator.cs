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
        private readonly RoomCreator _roomCreator;

        private Dictionary<RoomType, List<RoomConfigAsset>> _typeToRooms;
        private DungeonGenerationRooms _result;
        private TileBasedGenerationConfig _config;
        private List<DungeonGenerationRoom> _rooms;

        public TileBasedDungeonGenerator(RoomCreator roomCreator)
        {
            _roomCreator = roomCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            _result = generation.DungeonGenerationResult.GenerationData.GenerationRooms;

            _rooms = new List<DungeonGenerationRoom>();
            _result.Rooms = _rooms;

            var configValue = generation.GetConfig<TileBasedGenerationConfig>();
            _config = configValue.Value;

            _typeToRooms = new Dictionary<RoomType, List<RoomConfigAsset>>();

            foreach (var roomPresetConfig in _config.Rooms)
            {
                if (!_typeToRooms.TryGetValue(roomPresetConfig.RoomType, out var list))
                {
                    list = new List<RoomConfigAsset>();
                    _typeToRooms.Add(roomPresetConfig.RoomType, list);
                }

                list.Add(roomPresetConfig);
            }

            Generate();

            _result.StartGenerationRoom = _rooms.First();
            _result.EndGenerationRoom = _rooms.Last();

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void Generate()
        {
            var startConfig =
                _typeToRooms[RoomType.Start]
                    .Random();

            var startRoom =
                _roomCreator.Create(
                    Vector2Int.Zero,
                    startConfig);

            startRoom.RotateEuler = 0;
            startRoom.Depth = 0;

            _rooms.Add(startRoom);

            foreach (var outputDoor in startConfig.OutputDoors)
            {
                GenerateBranch(
                    startRoom,
                    new Vector2Int(outputDoor.X, outputDoor.Y),
                    0);
            }
        }

        private void GenerateBranch(
            DungeonGenerationRoom prevRoom,
            Vector2Int exitDoor,
            int depth)
        {
            bool createEndRoom =
                depth >= _config.MaxDepth;

            var roomType =
                createEndRoom
                    ? RoomType.End
                    : RoomType.Transit;

            var prevDoorWorldPos =
                GetDoorWorldPosition(
                    prevRoom,
                    exitDoor);

            var prevDoorSide =
                GetDoorSide(
                    prevRoom.Size,
                    exitDoor,
                    prevRoom.RotateEuler);

            var sideVec =
                SideToVector(prevDoorSide);

            var targetDoorWorldPos =
                prevDoorWorldPos + sideVec;

            var configs =
                _typeToRooms[roomType]
                    .ToList();

            configs.Shuffle();

            bool branchCreated = false;

            foreach (var config in configs)
            {
                if (branchCreated)
                    break;

                foreach (var rotation in new[] { 0f, 90f, 180f, 270f })
                {
                    var inputDoorSide =
                        GetDoorSide(
                            config.Size,
                            config.InputDoor,
                            rotation);

                    if (!IsOpposite(prevDoorSide, inputDoorSide))
                        continue;

                    var rotatedInputDoor =
                        RotatePoint(
                            config.InputDoor,
                            config.Size,
                            rotation);

                    var rotationOffset =
                        GetRotationOffset(
                            config.Size,
                            rotation);

                    var roomPosition =
                        targetDoorWorldPos
                        + rotationOffset
                        - rotatedInputDoor;

                    var room =
                        _roomCreator.Create(
                            roomPosition,
                            config);

                    room.RotateEuler = rotation;
                    room.Depth = depth + 1;

                    if (Intersects(room))
                        continue;

                    _rooms.Add(room);

                    branchCreated = true;

                    if (!createEndRoom)
                    {
                        foreach (var outputDoor in config.OutputDoors)
                        {
                            var outputDoorPos =
                                new Vector2Int(
                                    outputDoor.X,
                                    outputDoor.Y);

                            if (IsSameDoor(
                                    outputDoorPos,
                                    config.InputDoor))
                            {
                                continue;
                            }

                            GenerateBranch(
                                room,
                                outputDoorPos,
                                depth + 1);
                        }
                    }

                    break;
                }
            }
        }

        private bool Intersects(DungeonGenerationRoom room)
        {
            var rect1 =
                GetRoomRect(room);

            foreach (var other in _rooms)
            {
                var rect2 =
                    GetRoomRect(other);

                if (rect1.Overlaps(rect2))
                    return true;
            }

            return false;
        }

        private RectInt GetRoomRect(DungeonGenerationRoom room)
        {
            var size =
                GetRotatedSize(
                    room.Size,
                    room.RotateEuler);

            var offset =
                GetRotationOffset(
                    room.Size,
                    room.RotateEuler);

            return new RectInt(
                room.Position.X - offset.X,
                room.Position.Y - offset.Y,
                size.X,
                size.Y);
        }

        // Unity rotation:
        // positive Z rotation = counter-clockwise
        //
        // 90°  -> LEFT
        // 180° -> DOWN
        // 270° -> RIGHT
        //
        // We rotate around bottom-left pivot.
        private Vector2Int RotatePoint(
            Vector2Int point,
            Vector2Int size,
            float rotation)
        {
            int x = point.X;
            int y = point.Y;

            int w = size.X;
            int h = size.Y;

            if (Mathf.Approximately(rotation, 0))
            {
                return new Vector2Int(
                    x,
                    y);
            }

            // 90° CCW
            if (Mathf.Approximately(rotation, 90))
            {
                return new Vector2Int(
                    y,
                    w - 1 - x);
            }

            // 180°
            if (Mathf.Approximately(rotation, 180))
            {
                return new Vector2Int(
                    w - 1 - x,
                    h - 1 - y);
            }

            // 270° CCW
            return new Vector2Int(
                h - 1 - y,
                x);
        }

        // Offset from rotated AABB min
        private Vector2Int GetRotationOffset(
            Vector2Int size,
            float rotation)
        {
            int w = size.X;
            int h = size.Y;

            if (Mathf.Approximately(rotation, 0))
            {
                return Vector2Int.Zero;
            }

            // 90° CCW
            if (Mathf.Approximately(rotation, 90))
            {
                return new Vector2Int(
                    0,
                    w - 1);
            }

            // 180°
            if (Mathf.Approximately(rotation, 180))
            {
                return new Vector2Int(
                    w - 1,
                    h - 1);
            }

            // 270° CCW
            return new Vector2Int(
                h - 1,
                0);
        }

        private Vector2Int GetRotatedSize(
            Vector2Int size,
            float rotation)
        {
            if (Mathf.Approximately(rotation, 90) ||
                Mathf.Approximately(rotation, 270))
            {
                return new Vector2Int(
                    size.Y,
                    size.X);
            }

            return size;
        }

        private Vector2Int GetDoorWorldPosition(
            DungeonGenerationRoom room,
            Vector2Int localDoor)
        {
            var rotatedDoor =
                RotatePoint(
                    localDoor,
                    room.Size,
                    room.RotateEuler);

            var offset =
                GetRotationOffset(
                    room.Size,
                    room.RotateEuler);

            return room.Position
                   - offset
                   + rotatedDoor;
        }

        private Side GetDoorSide(
            Vector2Int size,
            Vector2Int door,
            float rotation)
        {
            var rotatedDoor =
                RotatePoint(
                    door,
                    size,
                    rotation);

            var rotatedSize =
                GetRotatedSize(
                    size,
                    rotation);

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
                case Side.Left:
                    return Vector2Int.Left;

                case Side.Right:
                    return Vector2Int.Right;

                case Side.Top:
                    return Vector2Int.Up;

                case Side.Bottom:
                    return Vector2Int.Down;
            }

            return Vector2Int.Zero;
        }

        private bool IsOpposite(Side a, Side b)
        {
            return (a == Side.Left && b == Side.Right)
                   || (a == Side.Right && b == Side.Left)
                   || (a == Side.Top && b == Side.Bottom)
                   || (a == Side.Bottom && b == Side.Top);
        }

        private bool IsSameDoor(
            Vector2Int a,
            Vector2Int b)
        {
            return a.X == b.X
                   && a.Y == b.Y;
        }

        public string GetName()
        {
            return "Tile Based";
        }
    }
}