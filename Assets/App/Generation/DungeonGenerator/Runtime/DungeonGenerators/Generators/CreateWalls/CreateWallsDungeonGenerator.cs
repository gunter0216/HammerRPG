using System.Collections.Generic;
using System.Linq;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.Matrix;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateWalls
{
    public class CreateWallsDungeonGenerator : IDungeonGenerator
    {
        private const int m_WallSize = 1;
        
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var roomsData = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = roomsData.StartGenerationRoom;
            var rooms = roomsData.Rooms;
            ExpandRooms(null, startRoom, 0);

            CreateWalls(rooms);
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateWalls(List<DungeonGenerationRoom> rooms)
        {
            foreach (var room in rooms)
            {
                CreateWalls(room);
            }
        }

        private void CreateWalls(DungeonGenerationRoom generationRoom)
        {
            var matrix = new Matrix<GeneraitonTile>(generationRoom.Width, generationRoom.Height);
            generationRoom.Matrix = matrix;
            for (int i = 0; i < matrix.Height; ++i)
            {
                for (int j = 0; j < matrix.Width; ++j)
                {
                    matrix.SetCell(i, j, new GeneraitonTile(TileConstants.Empty));
                }
            }
            
            for (int i = 0; i < generationRoom.Width; ++i)
            {
                matrix[0, i] = new GeneraitonTile(TileConstants.Wall);
                matrix[matrix.Height - 1, i] = new GeneraitonTile(TileConstants.Wall);
            }
                 
            for (int i = 1; i < generationRoom.Height - 1; ++i)
            {
                matrix[i, 0] = new GeneraitonTile(TileConstants.Wall);  
                matrix[i, matrix.Width - 1] = new GeneraitonTile(TileConstants.Wall); 
            }
            
            // var tilesAmount = room.Width * 2 + room.Height * 2 - 4;
            // var tiles = new List<TileData>(tilesAmount);
            // room.Tiles = tiles;
            // for (int i = room.Left; i < room.Right; ++i)
            // {
            //     tiles.Add(new TileData(TileConstants.Wall)
            //     {
            //         Position = new Vector2Int(i, room.Top - 1)
            //     });
            //     tiles.Add(new TileData(TileConstants.Wall)
            //     {
            //         Position = new Vector2Int(i, room.Bottom)
            //     });
            // }
            //     
            // for (int i = room.Bottom + 1; i < room.Top - 1; ++i)
            // {
            //     tiles.Add(new TileData(TileConstants.Wall)
            //     {
            //         Position = new Vector2Int(room.Left, i)
            //     });
            //     tiles.Add(new TileData(TileConstants.Wall)
            //     {
            //         Position = new Vector2Int(room.Right - 1, i)
            //     });
            // }
        }

        private void ExpandRooms(DungeonGenerationRoom prevGenerationRoom, DungeonGenerationRoom curGenerationRoom, int stupidCounter)
        {
            stupidCounter += 1;
            if (stupidCounter > 1000)
            {
                return;
            }
            
            var connections = curGenerationRoom.GetConnectionsExclude(prevGenerationRoom);

            foreach (var connection in connections)
            {
                if (connection.Side == RoomConnectSide.Top)
                {
                    curGenerationRoom.IncreaseHeight(m_WallSize);
                } 
                else if (connection.Side == RoomConnectSide.Bottom)
                {
                    curGenerationRoom.IncreaseHeight(m_WallSize);
                    curGenerationRoom.Move(Vector2Int.Bottom * m_WallSize);
                } 
                else if (connection.Side == RoomConnectSide.Right)
                {
                    curGenerationRoom.IncreaseWidth(m_WallSize);
                }
                else if (connection.Side == RoomConnectSide.Left)
                {
                    curGenerationRoom.IncreaseWidth(m_WallSize);
                    curGenerationRoom.Move(Vector2Int.Left * m_WallSize);
                }
            }

            foreach (var connection in connections)
            {
                ExpandRooms(curGenerationRoom, connection.GenerationRoom, stupidCounter);
            }
        }

        public string GetName()
        {
            return "Create Walls";
        }
    }
}