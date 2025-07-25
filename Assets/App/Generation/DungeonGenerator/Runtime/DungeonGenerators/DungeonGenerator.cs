﻿using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateDoors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateWalls;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.KeysDistributor;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsSeparator;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndRooms;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators
{
    public class DungeonGenerator
    {
        private readonly ILogger m_Logger;
        private readonly List<IDungeonGenerator> m_Generators;
        
        private int m_CurrentGeneratorIndex;
        private DungeonGeneration m_Generation;

        public DungeonGenerator(ILogger logger)
        {
            m_Logger = logger;
            var generators = new List<IDungeonGenerator>();
            
            var roomCreator = new RoomCreator();

            generators.Add(new CreateRoomsDungeonGenerator(roomCreator));
            generators.Add(new SeparateRoomsDungeonGenerator());
            generators.Add(new SelectSmallRoomsDungeonGenerator());
            generators.Add(new DiscardSmallRoomsDungeonGenerator());
            generators.Add(new SelectBorderingRoomsDungeonGenerator());
            generators.Add(new DiscardBorderingRoomsDungeonGenerator());
            generators.Add(new TriangulationDungeonGenerator());
            generators.Add(new SpanningTreeDungeonGenerator(m_Logger));
            generators.Add(new CreateCorridorsDungeonGenerator(roomCreator));
            generators.Add(new StartEndRoomsDungeonGenerator());
            generators.Add(new StartEndPathDungeonGenerator());
            generators.Add(new DistributeKeysDungeonGenerator(new DungeonKeyCreator()));
            generators.Add(new CreateWallsDungeonGenerator());
            generators.Add(new CreateDoorsDungeonGenerator());
            
            m_Generators = generators;
        }

        public void StartGeneration(DungeonGenerationConfig generationConfig)
        {
            // var matrix = new Matrix.Matrix(dungeonConfig.Width, dungeonConfig.Height);
            var data = new DungeonData
            {
                RoomsData = new DungeonRoomsData(),
                // Matrix = matrix
            };
            var config = new DungeonConfig();
            var dungeon = new Dungeon(config, data);
            
            m_Generation = new DungeonGeneration(dungeon, generationConfig);
            m_CurrentGeneratorIndex = 0;
        }

        public bool NextIteration()
        {
            if (IsComplete())
            {
                m_Logger.LogError($"Generation complete!");
                return false;
            }

            var generator = m_Generators[m_CurrentGeneratorIndex];
            
            m_Logger.LogError($"Next iteration generation: {generator.GetName()}"); 
            
            var generation = generator.Process(m_Generation);
            if (!generation.HasValue)
            {
                m_Logger.LogError($"Ops. Something wrong. Cant generate next iteration.");
                return false;
            }

            m_CurrentGeneratorIndex += 1;
            m_Generation = generation.Value;
            
            return true;
        }

        public bool IsStart()
        {
            return m_Generation != null;
        }

        public bool IsComplete()
        {
            return m_CurrentGeneratorIndex >= m_Generators.Count;
        }

        public Optional<DungeonGeneration> GetGeneration()
        {
            if (m_Generation == null)
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            return Optional<DungeonGeneration>.Success(m_Generation);
        }
    }
}