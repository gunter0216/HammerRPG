using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.KeysDistributor
{
    public class DistributeKeysDungeonGenerator : IDungeonGenerator
    {
        private class KeyContext
        {
            public DungeonKeyData Key;
            public DungeonGenerationRoom BranchRoot;
        }

        private readonly DungeonKeyCreator m_KeyCreator;

        public DistributeKeysDungeonGenerator(DungeonKeyCreator keyCreator)
        {
            m_KeyCreator = keyCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = roomsData.StartGenerationRoom;
            var endRoom = roomsData.EndGenerationRoom;

            var visitedRooms = new HashSet<DungeonGenerationRoom>();
            var doorKeys = new Stack<DungeonKeyData>();
            var stack = new List<DungeonGenerationRoom>();

            stack.Add(startRoom);
            visitedRooms.Add(startRoom);

            int safety = 0;

            while (stack.Count > 0 && safety++ < 100000)
            {
                var room = stack[^1];

                if (room == endRoom)
                    break;

                var connections = room.Connections
                    .Select(x => x.GenerationRoom)
                    .ToList();

                var unvisited = connections
                    .Where(x => !visitedRooms.Contains(x))
                    .ToList();

                // Тупик или возврат
                if (connections.Count <= 1)
                {
                    if (stack.Count <= 1)
                    {
                        if (connections.Count > 0)
                            AddRoomInStack(connections[0]);
                    }
                    else
                    {
                        // безопасный Peek
                        if (doorKeys.Count > 0)
                        {
                            var doorKey = doorKeys.Peek();
                            room.AddDoorKey(doorKey);
                        }

                        // 🔥 ВАЖНО: правильный откат без пересоздания списка
                        for (int j = stack.Count - 2; j >= 0; j--)
                        {
                            var candidate = stack[j];

                            var candidateConnections = candidate.Connections
                                .Select(x => x.GenerationRoom);

                            var hasUnvisited = candidateConnections
                                .Any(x => !visitedRooms.Contains(x));

                            if (candidate.Connections.Count >= 3 && hasUnvisited)
                            {
                                // удаляем всё после j
                                stack.RemoveRange(j + 1, stack.Count - (j + 1));
                                break;
                            }
                        }
                    }

                    continue;
                }

                // Прямая линия
                if (connections.Count == 2)
                {
                    var nextRoom = connections
                        .FirstOrDefault(x => !visitedRooms.Contains(x));

                    if (nextRoom != null)
                        AddRoomInStack(nextRoom);

                    continue;
                }

                // Развилка
                if (connections.Count >= 3)
                {
                    var notVisitedRooms = unvisited;

                    var mainRoom = notVisitedRooms
                        .FirstOrDefault(x => x.IsMainPath);

                    var notMainRooms = notVisitedRooms
                        .Where(x => !x.IsMainPath)
                        .ToList();

                    // если всё посещено → откат
                    if (mainRoom == null && notMainRooms.Count == 0)
                    {
                        stack.RemoveAt(stack.Count - 1);
                        continue;
                    }

                    if (notMainRooms.Count == 0)
                    {
                        if (mainRoom != null)
                            AddRoomInStack(mainRoom);

                        if (doorKeys.Count > 0)
                            doorKeys.Pop();

                        continue;
                    }

                    var doorKey = doorKeys.Count > 0 ? doorKeys.Peek() : null;

                    var roomWithRequiredDoorKey = connections
                        .FirstOrDefault(x => x.RequiredKey != null && x.RequiredKey == doorKey);

                    if (roomWithRequiredDoorKey == null)
                    {
                        doorKey = m_KeyCreator.Create();
                        doorKeys.Push(doorKey);

                        if (notMainRooms.Count >= 2)
                        {
                            notMainRooms[0].RequiredKey = doorKey;
                            AddRoomInStack(notMainRooms[1]);
                        }
                        else
                        {
                            if (mainRoom != null)
                                mainRoom.RequiredKey = doorKey;

                            AddRoomInStack(notMainRooms[0]);
                        }
                    }
                    else
                    {
                        if (doorKeys.Count > 0)
                            doorKeys.Pop();

                        AddRoomInStack(roomWithRequiredDoorKey);
                    }

                    continue;
                }
            }

            return Optional<DungeonGeneration>.Success(generation);

            void AddRoomInStack(DungeonGenerationRoom room)
            {
                if (room == null) return;

                stack.Add(room);
                visitedRooms.Add(room);
            }
        }

        public string GetName()
        {
            return "Distribute keys";
        }
    }
}