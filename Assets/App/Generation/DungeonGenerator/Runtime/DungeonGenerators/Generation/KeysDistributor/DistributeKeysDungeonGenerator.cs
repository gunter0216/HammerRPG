﻿using System.Collections.Generic;
using System.Linq;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.KeysDistributor
{
    public class DistributeKeysDungeonGenerator : IDungeonGenerator
    {
        private readonly DungeonKeyCreator m_KeyCreator;
        
        public DistributeKeysDungeonGenerator(DungeonKeyCreator keyCreator)
        {
            m_KeyCreator = keyCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            if (!generation.TryGetCash<StartEndPathGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            var roomsData = generation.Dungeon.Data.RoomsData;
            var rooms = roomsData.Rooms;
            var startRoom = roomsData.StartRoom;
            var endRoom = roomsData.EndRoom;
            var visitedRooms = new HashSet<DungeonRoomData>();
            
            var path = cash.Path;
            var roomsInPath = new HashSet<DungeonRoomData>(path);
            foreach (var room in rooms)
            {
                if (roomsInPath.Contains(room))
                {
                    continue;
                }

                if (room.Connections
                        .Select(x => x.Room)
                        .Count(x => roomsInPath.Contains(x)) >= 2)
                {
                    roomsInPath.Add(room);
                }
            }

            foreach (var roomData in roomsInPath)
            {
                roomData.IsMainPath = true;
            }

            var doorKeys = new Stack<DungeonKeyData>();
            var stack = new List<DungeonRoomData>();
            stack.Add(startRoom);
            visitedRooms.Add(startRoom);
            for (int i = 0; i < 100000; ++i)
            {
                var room = stack.Last();
                if (room == endRoom)
                {
                    break;
                }
                
                if (room.Connections.Count <= 1)
                {
                    if (stack.Count <= 1)
                    {
                        AddRoomInStack(room.Connections[0].Room);
                    }
                    else
                    {
                        var doorKey = doorKeys.Peek();
                        room.AddDoorKey(doorKey);
                        
                        for (int j = stack.Count - 1; j >= 0; j--)
                        {
                            if (stack[j].Connections.Count < 3)
                            {
                                continue;
                            }

                            if (stack[j].Connections
                                .Select(x => x.Room)
                                .All(x => visitedRooms.Contains(x)))
                            {
                                continue;
                            }

                            stack = stack.GetRange(0, j + 1);
                            break;
                        }
                    }

                    continue;
                }

                if (room.Connections.Count <= 2)
                {
                    var nextRoom = room.Connections.First(x => !visitedRooms.Contains(x.Room)); 
                    AddRoomInStack(nextRoom.Room);
                    continue;
                }
                
                if (room.Connections.Count >= 3)
                {
                    var notVisitedRooms = room.Connections
                        .Select(x => x.Room)
                        .Where(x => !visitedRooms.Contains(x))
                        .ToArray();
                    
                    var pathRoom = notVisitedRooms
                        .FirstOrDefault(x => roomsInPath.Contains(x));
                    
                    var notPathRooms = notVisitedRooms
                        .Where(x => !roomsInPath.Contains(x))
                        .ToArray();

                    if (notPathRooms.Length <= 0)
                    {
                        AddRoomInStack(pathRoom);
                        doorKeys.Pop();
                    }
                    else
                    {
                        var doorKey = doorKeys.Count > 0 ? doorKeys.Peek() : null;
                        var roomWithRequiredDoorKey = room.Connections
                            .Select(x => x.Room)
                            .FirstOrDefault(x => x.RequiredKey != null && x.RequiredKey == doorKey);
                        if (roomWithRequiredDoorKey == default)
                        {
                            doorKey = m_KeyCreator.Create();
                            doorKeys.Push(doorKey);
                            if (notPathRooms.Length >= 2)
                            {
                                notPathRooms[0].RequiredKey = doorKey;
                                AddRoomInStack(notPathRooms[1]);
                            }
                            else
                            {
                                pathRoom.RequiredKey = doorKey;
                                AddRoomInStack(notPathRooms[0]);
                            }
                        }
                        else
                        {
                            doorKeys.Pop();
                            AddRoomInStack(roomWithRequiredDoorKey);
                        }
                    }
                    
                    continue;
                }
            }

            void AddRoomInStack(DungeonRoomData room)
            {
                stack.Add(room);
                visitedRooms.Add(room);
            }

            return Optional<DungeonGeneration>.Success(generation);
        }

        public string GetName()
        {
            return "Distribute keys";
        }
    }
}