using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private StringBuilder _log;

        public TileBasedDungeonGenerator(RoomCreator roomCreator)
        {
            _roomCreator = roomCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            _log = new StringBuilder();

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

            Debug.Log(_log.ToString());

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void Log(string message)
        {
            _log.AppendLine(message);
        }

        private void Generate()
        {
            var startConfig = _typeToRooms[RoomType.Start].Random();

            var startRoom = _roomCreator.Create(Vector2Int.Zero, startConfig);
            startRoom.RotateEuler = 0;
            startRoom.Depth = 0;

            _rooms.Add(startRoom);

            Log("=== START ROOM ===");
            Log($"  Config size: {startConfig.Size.X}x{startConfig.Size.Y}");
            Log($"  Position: ({startRoom.Position.X},{startRoom.Position.Y}), Rotation: {startRoom.RotateEuler}");
            Log($"  Output doors: {startConfig.OutputDoors.Length}");

            foreach (var outputDoor in startConfig.OutputDoors)
            {
                Log($"  Processing output door ({outputDoor.X},{outputDoor.Y})");
                GenerateBranch(startRoom, new Vector2Int(outputDoor.X, outputDoor.Y), 0);
            }
        }

        private void GenerateBranch(
            DungeonGenerationRoom prevRoom,
            Vector2Int exitDoor,
            int depth)
        {
            bool createEndRoom = depth >= _config.MaxDepth;
            var roomType = createEndRoom ? RoomType.End : RoomType.Fight;

            Log($"\n--- GenerateBranch depth={depth} createEnd={createEndRoom} ---");
            Log($"  Prev room: pos=({prevRoom.Position.X},{prevRoom.Position.Y}) size={prevRoom.Size.X}x{prevRoom.Size.Y} rot={prevRoom.RotateEuler}");
            Log($"  Exit door local: ({exitDoor.X},{exitDoor.Y})");

            var prevDoorWorldPos = GetDoorWorldPosition(prevRoom, exitDoor);
            Log($"  Exit door WORLD: ({prevDoorWorldPos.X},{prevDoorWorldPos.Y})");

            var prevDoorSide = GetDoorSide(prevRoom.Size, exitDoor, prevRoom.RotateEuler);
            var sideVec = SideToVector(prevDoorSide);
            var newDoorWorldTarget = prevDoorWorldPos + sideVec;

            Log($"  Exit door side: {prevDoorSide}  SideToVector=({sideVec.X},{sideVec.Y})");
            Log($"  Target WORLD for new input door: ({newDoorWorldTarget.X},{newDoorWorldTarget.Y})");

            var configs = _typeToRooms[roomType];
            configs.Shuffle();

            bool branchCreated = false;

            foreach (var config in configs)
            {
                if (branchCreated) break;

                Log($"  Trying config: size={config.Size.X}x{config.Size.Y} inputDoor=({config.InputDoor.X},{config.InputDoor.Y})");

                foreach (var rotation in new[] { 0f, 90f, 180f, 270f })
                {
                    var rotatedInputDoor = RotatePoint(config.InputDoor, config.Size, rotation);
                    var inputDoorSide    = GetDoorSide(config.Size, config.InputDoor, rotation);

                    Log($"    rot={rotation}: rotatedInputDoor=({rotatedInputDoor.X},{rotatedInputDoor.Y}) side={inputDoorSide}");

                    if (!IsOpposite(prevDoorSide, inputDoorSide))
                    {
                        Log($"    SKIP: {prevDoorSide} vs {inputDoorSide} not opposite");
                        continue;
                    }

                    var rotationOffset = GetRotationOffset(config.Size, rotation);
                    var roomPosition   = newDoorWorldTarget - rotationOffset - rotatedInputDoor;

                    Log($"    rotationOffset=({rotationOffset.X},{rotationOffset.Y})");
                    Log($"    roomPos = ({newDoorWorldTarget.X},{newDoorWorldTarget.Y}) - ({rotationOffset.X},{rotationOffset.Y}) - ({rotatedInputDoor.X},{rotatedInputDoor.Y}) = ({roomPosition.X},{roomPosition.Y})");

                    var verifyDoor = roomPosition + rotationOffset + rotatedInputDoor;
                    bool match = verifyDoor.X == newDoorWorldTarget.X && verifyDoor.Y == newDoorWorldTarget.Y;
                    Log($"    VERIFY door=({verifyDoor.X},{verifyDoor.Y}) expected=({newDoorWorldTarget.X},{newDoorWorldTarget.Y}) => {(match ? "OK" : "MISMATCH!")}");

                    var room = _roomCreator.Create(roomPosition, config);
                    room.RotateEuler = rotation;
                    room.Depth = depth + 1;

                    if (Intersects(room))
                    {
                        Log($"    SKIP: intersects with existing room");
                        continue;
                    }

                    _rooms.Add(room);
                    branchCreated = true;

                    var aabbSize = GetRotatedSize(config.Size, rotation);
                    Log($"    PLACED at ({roomPosition.X},{roomPosition.Y}) rot={rotation}");
                    Log($"    AABB origin=({roomPosition.X + rotationOffset.X},{roomPosition.Y + rotationOffset.Y}) size={aabbSize.X}x{aabbSize.Y}");

                    if (!createEndRoom)
                    {
                        foreach (var outputDoor in config.OutputDoors)
                        {
                            var outputDoorPos = new Vector2Int(outputDoor.X, outputDoor.Y);

                            if (IsSameDoor(outputDoorPos, config.InputDoor))
                            {
                                Log($"    Skip output door ({outputDoor.X},{outputDoor.Y}): same as input");
                                continue;
                            }

                            var outDoorWorld = GetDoorWorldPosition(room, outputDoorPos);
                            Log($"    Output door local=({outputDoor.X},{outputDoor.Y}) world=({outDoorWorld.X},{outDoorWorld.Y}) -> branching");

                            GenerateBranch(room, outputDoorPos, depth + 1);
                        }
                    }

                    break;
                }
            }

            if (!branchCreated)
                Log($"  WARNING: could not place any room at depth={depth + 1}!");
        }

        private bool Intersects(DungeonGenerationRoom room)
        {
            var rect1 = GetRoomRect(room);

            foreach (var other in _rooms)
            {
                var rect2 = GetRoomRect(other);

                if (rect1.Overlaps(rect2))
                    return true;
            }

            return false;
        }

        private RectInt GetRoomRect(DungeonGenerationRoom room)
        {
            var size   = GetRotatedSize(room.Size, room.RotateEuler);
            var offset = GetRotationOffset(room.Size, room.RotateEuler);

            return new RectInt(
                room.Position.X + offset.X,
                room.Position.Y + offset.Y,
                size.X,
                size.Y);
        }

        // Rotation matrix (counter-clockwise), origin = bottom-left corner
        // 0°:   (x,       y      )  size stays (W, H)
        // 90°:  (H-1-y,   x      )  size becomes (H, W)
        // 180°: (W-1-x,   H-1-y  )  size stays (W, H)
        // 270°: (y,       W-1-x  )  size becomes (H, W)
        private Vector2Int RotatePoint(
            Vector2Int point,
            Vector2Int size,
            float rotation)
        {
            int x = point.X, y = point.Y;
            int w = size.X,  h = size.Y;

            if (Mathf.Approximately(rotation, 0))
                return new Vector2Int(x, y);

            if (Mathf.Approximately(rotation, 90))
                return new Vector2Int(h - 1 - y, x);

            if (Mathf.Approximately(rotation, 180))
                return new Vector2Int(w - 1 - x, h - 1 - y);

            // 270
            return new Vector2Int(y, w - 1 - x);
        }

        // Offset of the AABB bottom-left corner relative to room.Position after rotation.
        // 0°:   (0,     0    )
        // 90°:  (H-1,   0    )
        // 180°: (W-1,   H-1  )
        // 270°: (0,     W-1  )
        private Vector2Int GetRotationOffset(
            Vector2Int size,
            float rotation)
        {
            int w = size.X, h = size.Y;

            if (Mathf.Approximately(rotation, 0))
                return Vector2Int.Zero;

            if (Mathf.Approximately(rotation, 90))
                return new Vector2Int(h - 1, 0);

            if (Mathf.Approximately(rotation, 180))
                return new Vector2Int(w - 1, h - 1);

            // 270
            return new Vector2Int(0, w - 1);
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

        // World position of a door = roomPosition + rotationOffset + rotatedDoor
        private Vector2Int GetDoorWorldPosition(
            DungeonGenerationRoom room,
            Vector2Int localDoor)
        {
            var rotatedDoor = RotatePoint(localDoor, room.Size, room.RotateEuler);
            var offset      = GetRotationOffset(room.Size, room.RotateEuler);
            return room.Position + offset + rotatedDoor;
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