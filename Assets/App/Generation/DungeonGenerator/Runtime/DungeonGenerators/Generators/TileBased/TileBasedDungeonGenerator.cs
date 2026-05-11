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
            _result =
                generation
                    .DungeonGenerationResult
                    .GenerationData
                    .GenerationRooms;

            _rooms = new List<DungeonGenerationRoom>();

            _result.Rooms = _rooms;

            var configValue =
                generation.GetConfig<TileBasedGenerationConfig>();

            _config = configValue.Value;

            _typeToRooms =
                new Dictionary<RoomType, List<RoomConfigAsset>>();

            foreach (var roomPresetConfig in _config.Rooms)
            {
                if (!_typeToRooms.TryGetValue(
                        roomPresetConfig.RoomType,
                        out var list))
                {
                    list = new List<RoomConfigAsset>();

                    _typeToRooms.Add(
                        roomPresetConfig.RoomType,
                        list);
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

            var startVariant =
                startConfig.Variants
                    .Random();

            var startRoom =
                _roomCreator.Create(
                    Vector2Int.Zero,
                    startConfig,
                    startVariant);

            startRoom.ConfigVariant = startVariant;
            startRoom.RotateEuler = 0;
            startRoom.Depth = 0;

            _rooms.Add(startRoom);

            foreach (var outputDoor in startVariant.OutputDoors)
            {
                GenerateBranch(
                    startRoom,
                    new Vector2Int(
                        outputDoor.X,
                        outputDoor.Y),
                    0);
            }
        }

        private bool GenerateBranch(
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

            foreach (var config in configs)
            {
                var variants =
                    config.Variants
                        .ToList();

                variants.Shuffle();

                foreach (var variant in variants)
                {
                    foreach (var rotation in new[] { 0f, 90f, 180f, 270f })
                    {
                        var inputDoorSide =
                            GetDoorSide(
                                config.Size,
                                config.InputDoor,
                                rotation);

                        if (!IsOpposite(
                                prevDoorSide,
                                inputDoorSide))
                        {
                            continue;
                        }

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

                        Debug.LogWarning(
                            "TRY PLACE ROOM\n" +
                            $"Config: {config.name}\n" +
                            $"Variant: {variant.AssetKey.name}\n" +
                            $"Position: {roomPosition}\n" +
                            $"Rotation: {rotation}\n" +
                            $"Depth: {depth + 1}");

                        var room =
                            _roomCreator.Create(
                                roomPosition,
                                config,
                                variant);

                        room.ConfigVariant = variant;
                        room.RotateEuler = rotation;
                        room.Depth = depth + 1;

                        if (Intersects(room))
                        {
                            Debug.LogWarning(
                                "ROOM INTERSECTION\n" +
                                $"Config: {config.name}\n" +
                                $"Variant: {variant.AssetKey.name}\n" +
                                $"Position: {roomPosition}\n" +
                                $"Rotation: {rotation}");

                            continue;
                        }

                        _rooms.Add(room);

                        if (createEndRoom)
                        {
                            return true;
                        }

                        var successfulDoors =
                            new List<Vector2Int>();

                        bool failed = false;

                        foreach (var outputDoor in variant.OutputDoors)
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

                            bool branchSuccess =
                                GenerateBranch(
                                    room,
                                    outputDoorPos,
                                    depth + 1);

                            if (branchSuccess)
                            {
                                successfulDoors.Add(outputDoorPos);
                                continue;
                            }

                            if (TryReplaceVariant(
                                    room,
                                    config,
                                    variant,
                                    successfulDoors,
                                    outputDoorPos,
                                    out var newDoors))
                            {
                                foreach (var newDoor in newDoors)
                                {
                                    bool success =
                                        GenerateBranch(
                                            room,
                                            newDoor,
                                            depth + 1);

                                    if (!success)
                                    {
                                        Debug.LogError(
                                            "FAILED GENERATE REPLACEMENT BRANCH\n" +
                                            $"Room Config: {room.ConfigAsset.name}\n" +
                                            $"Room Variant: {room.ConfigVariant.AssetKey.name}\n" +
                                            $"Room Position: {room.Position}\n" +
                                            $"Room Rotation: {room.RotateEuler}\n" +
                                            $"Door: {newDoor}");

                                        return false;
                                    }
                                }

                                return true;
                            }

                            failed = true;
                            break;
                        }

                        if (!failed)
                        {
                            return true;
                        }

                        _rooms.Remove(room);
                    }
                }
            }

            if (roomType != RoomType.End)
            {
                Debug.LogError(
                    "FAILED GENERATE BRANCH\n" +
                    $"Depth: {depth}\n" +
                    $"RoomType: {roomType}\n" +
                    $"PrevRoom Config: {prevRoom.ConfigAsset.name}\n" +
                    $"PrevRoom Variant: {prevRoom.ConfigVariant.AssetKey.name}\n" +
                    $"PrevRoom Position: {prevRoom.Position}\n" +
                    $"PrevRoom Rotation: {prevRoom.RotateEuler}\n" +
                    $"ExitDoor: {exitDoor}\n" +
                    $"PrevDoorWorldPos: {prevDoorWorldPos}\n" +
                    $"PrevDoorSide: {prevDoorSide}\n" +
                    $"TargetDoorWorldPos: {targetDoorWorldPos}");
            }

            return false;
        }

        private bool TryReplaceVariant(
            DungeonGenerationRoom room,
            RoomConfigAsset config,
            RoomConfigVariant currentVariant,
            List<Vector2Int> successfulDoors,
            Vector2Int failedDoor,
            out List<Vector2Int> newDoors)
        {
            newDoors = new List<Vector2Int>();

            var variants =
                config.Variants
                    .ToList();

            variants.Shuffle();

            Debug.LogWarning(
                "TRY REPLACE VARIANT\n" +
                $"Room Config: {config.name}\n" +
                $"Room Position: {room.Position}\n" +
                $"Room Rotation: {room.RotateEuler}\n" +
                $"Current Variant: {currentVariant.AssetKey.name}\n" +
                $"Failed Door: {failedDoor}\n" +
                $"Successful Doors: {string.Join(", ", successfulDoors)}");

            foreach (var variant in variants)
            {
                if (variant == currentVariant)
                {
                    continue;
                }

                bool valid = true;

                foreach (var usedDoor in successfulDoors)
                {
                    bool contains =
                        variant.OutputDoors.Any(x =>
                            x.X == usedDoor.X &&
                            x.Y == usedDoor.Y);

                    if (!contains)
                    {
                        Debug.LogWarning(
                            $"Variant {variant.AssetKey.name} rejected. " +
                            $"Missing successful door {usedDoor}");

                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    continue;
                }

                bool containsFailedDoor =
                    variant.OutputDoors.Any(x =>
                        x.X == failedDoor.X &&
                        x.Y == failedDoor.Y);

                if (containsFailedDoor)
                {
                    Debug.LogWarning(
                        $"Variant {variant.AssetKey.name} rejected. " +
                        $"Still contains failed door {failedDoor}");

                    continue;
                }

                foreach (var outputDoor in variant.OutputDoors)
                {
                    var door =
                        new Vector2Int(
                            outputDoor.X,
                            outputDoor.Y);

                    bool alreadyUsed =
                        successfulDoors.Any(x =>
                            x.X == door.X &&
                            x.Y == door.Y);

                    if (alreadyUsed)
                    {
                        continue;
                    }

                    if (IsSameDoor(
                            door,
                            config.InputDoor))
                    {
                        continue;
                    }

                    newDoors.Add(door);
                }

                room.ConfigVariant = variant;

                Debug.LogWarning(
                    "ROOM VARIANT REPLACED\n" +
                    $"Room Config: {config.name}\n" +
                    $"Room Position: {room.Position}\n" +
                    $"Room Rotation: {room.RotateEuler}\n" +
                    $"Old Variant: {currentVariant.AssetKey.name}\n" +
                    $"New Variant: {variant.AssetKey.name}\n" +
                    $"Failed Door: {failedDoor}\n" +
                    $"Successful Doors: {string.Join(", ", successfulDoors)}\n" +
                    $"New Doors: {string.Join(", ", newDoors)}");

                return true;
            }

            Debug.LogError(
                "FAILED TO REPLACE VARIANT\n" +
                $"Config: {config.name}\n" +
                $"Current Variant: {currentVariant.AssetKey.name}\n" +
                $"Room Position: {room.Position}\n" +
                $"Room Rotation: {room.RotateEuler}\n" +
                $"Failed Door: {failedDoor}\n" +
                $"Successful Doors: {string.Join(", ", successfulDoors)}");

            return false;
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
                {
                    return true;
                }
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
                return new Vector2Int(x, y);
            }

            if (Mathf.Approximately(rotation, 90))
            {
                return new Vector2Int(
                    y,
                    w - 1 - x);
            }

            if (Mathf.Approximately(rotation, 180))
            {
                return new Vector2Int(
                    w - 1 - x,
                    h - 1 - y);
            }

            return new Vector2Int(
                h - 1 - y,
                x);
        }

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

            if (Mathf.Approximately(rotation, 90))
            {
                return new Vector2Int(
                    0,
                    w - 1);
            }

            if (Mathf.Approximately(rotation, 180))
            {
                return new Vector2Int(
                    w - 1,
                    h - 1);
            }

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
            {
                return Side.Left;
            }

            if (rotatedDoor.X == rotatedSize.X - 1)
            {
                return Side.Right;
            }

            if (rotatedDoor.Y == 0)
            {
                return Side.Bottom;
            }

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