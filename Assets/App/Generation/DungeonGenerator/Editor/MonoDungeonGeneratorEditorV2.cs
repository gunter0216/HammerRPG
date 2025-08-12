using System.Collections.Generic;
using System.Text;
using App.Generation.DungeonGenerator.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateDoors.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Logger = App.Common.Logger.Runtime.Logger;
using Vector2Int = Assets.App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Editor
{
    [CustomEditor(typeof(MonoDungeonGenerator))]
    public class MonoDungeonGeneratorEditorV2 : UnityEditor.Editor 
    {
        private class MonoTile
        {
            public SpriteRenderer SpriteRenderer;
            public GeneraitonTile GeneraitonTile;
        }
        
        private readonly Runtime.DungeonGenerators.DungeonGenerator m_Generator = new(new Logger());
        private readonly DungeonGenerationDtoToConfigConverter m_DungeonGenerationDtoToConfigConverter = new();
        
        private DungeonGeneration m_Generation;
        private bool m_ShowLabel;
        private bool m_ShowRoomBorders;
        private bool m_IsDoorShow;
        private Transform m_TilesContent;
        private Dictionary<Vector2Int, MonoTile> m_Tiles;

        void OnEnable()
        {
            SceneView.duringSceneGui += WhenUpdate;
        }

        public override void OnInspectorGUI() 
        {
            DrawDefaultInspector();
            
            var myScript = (MonoDungeonGenerator)target;
            m_ShowLabel = myScript.ShowLabel;
            m_ShowRoomBorders = myScript.ShowRoomBorders;
            if (GUILayout.Button("Start Generate"))
            {
                var config = m_DungeonGenerationDtoToConfigConverter.Convert(myScript.Config);
                m_Generator.StartGeneration(config);
            }
            
            if (GUILayout.Button("Next Iteration"))
            {
                m_Generator.NextIteration();
                SceneView.RepaintAll();
            }
        }

        void WhenUpdate(SceneView sceneView)
        {
            if (!m_Generator.IsStart())
            {
                return;
            }

            m_Generation = m_Generator.GetGeneration().Value;
            
            Draw();
        }
        
        private void Draw()
        {
            var tiles = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms[0].Tiles;
            if (tiles != null && tiles.Count > 0)
            {
                // DrawTiles(); 
            }
            
            var matrix = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms[0].Matrix;
            if (matrix != null)
            {
                DrawMatrix();
            }

            if (m_ShowRoomBorders)
            {
                DrawRooms();
            }

            if (m_ShowLabel)
            {
                DrawRoomLabels();
            }

            if (m_Generation.TryGetCash<StartEndPathGenerationCash>(out var startEndPath))
            {
                DrawStartEndPath(startEndPath);
            }
            else if (m_Generation.TryGetCash<SpanningTreeGenerationCash>(out var spanningTree))
            {
                DrawTree(spanningTree);
            }
            else if (m_Generation.TryGetCash<TriangulationGenerationCash>(out var triangulation))
            {
                DrawTriangulation(triangulation);
            }
        }

        private void DrawMatrix()
        {
            if (m_Generation.HasCash<CreateDoorsGenerationCash>() && !m_IsDoorShow)
            {
                m_IsDoorShow = true;
                UpdateMatrix();
                return;
            }
            
            if (m_TilesContent != null)
            {
                return;
            }
            
            Debug.LogError("Create Tiles");

            m_TilesContent = new GameObject().transform;
            m_Tiles = new Dictionary<Vector2Int, MonoTile>(1000);
            
            var rooms = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms;
            foreach (var room in rooms)
            {
                var matrix = room.Matrix;
                for (int i = 0; i < matrix.Height; ++i)
                {
                    for (int j = 0; j < matrix.Width; ++j)
                    {
                        var tile = matrix[i, j];
                        var position = room.LocalToWorld(j, i);
                        if (m_Tiles.ContainsKey(position))
                        {
                            continue;
                        }

                        if (tile.Id == TileConstants.Empty)
                        {
                            continue;
                        }

                        var color = Color.black;
                        color = tile.Id == TileConstants.Door ? Color.red : color;
                        
                        var tileObj = new GameObject();
                        tileObj.transform.parent = m_TilesContent;
                        tileObj.transform.position = new Vector3(position.X, position.Y);
                        var spriteRenderer = tileObj.AddComponent<SpriteRenderer>();
                        spriteRenderer.sprite = Sprite.Create(
                            new Texture2D(100, 100),
                            new Rect(0, 0, 100, 100),
                            new Vector2(0, 0),
                            100.0f);
                        spriteRenderer.color = color;
                        
                        m_Tiles.Add(position, new MonoTile()
                        {
                            SpriteRenderer = spriteRenderer,
                            GeneraitonTile = tile
                        });
                    }
                }
            }
        }

        private void UpdateMatrix()
        {
            foreach (var monoTile in m_Tiles)
            {
                var tileId = monoTile.Value.GeneraitonTile.Id;
                if (tileId == TileConstants.Empty)
                {
                    continue;
                }
                
                var color = Color.black;
                color = tileId == TileConstants.Door ? Color.red : color;
                monoTile.Value.SpriteRenderer.color = color;
            }
        }

        // private void DrawTiles()
        // {
        //     var rooms = m_Generation.Dungeon.Data.RoomsData.Rooms;
        //     foreach (var room in rooms)
        //     {
        //         foreach (var tile in room.Tiles)
        //         {
        //             var tilePosition = tile.Position;
        //             var rect = new Rect();
        //             rect.xMin = tilePosition.X;
        //             rect.xMax = tilePosition.X + 1;
        //             rect.yMin = tilePosition.Y;
        //             rect.yMax = tilePosition.Y + 1;
        //             var color = tile.Id == TileConstants.Wall ? Color.black : Color.red;
        //             Handles.DrawSolidRectangleWithOutline(rect, color, color);
        //         }
        //     }
        // }

        private void DrawStartEndPath(StartEndPathGenerationCash startEndPath)
        {
            Handles.color = Color.cyan;
            var points = new Vector3[startEndPath.Path.Count];
            int i = 0;
            foreach (var room in startEndPath.Path)
            {
                var center = room.GetCenter();
                var point = new Vector3(center.X, center.Y);
                points[i++] = point;
            }
            
            Handles.DrawPolyLine(points);
        }

        private void DrawTree(SpanningTreeGenerationCash spanningTree)
        {
            var tree = spanningTree.Tree;
            
            Handles.color = Color.yellow;
            foreach (var edge in tree)
            {
                var point1 = edge.Room1.GetCenter();
                var point2 = edge.Room2.GetCenter();
                var points = new Vector3[2];
                points[0] = new Vector3(point1.X, point1.Y, 0);
                points[1] = new Vector3(point2.X, point2.Y, 0);
                Handles.DrawPolyLine(points);
            }
        }

        private void DrawTriangulation(TriangulationGenerationCash triangulation)
        {
            Handles.color = Color.blue;
            foreach (var triangle in triangulation.Triangles)
            {
                var points = new Vector3[3];
                points[0] = new Vector3(triangle.A.X, triangle.A.Y, 0);
                points[1] = new Vector3(triangle.B.X, triangle.B.Y, 0);
                points[2] = new Vector3(triangle.C.X, triangle.C.Y, 0);
                Handles.DrawPolyLine(points);
            }
        }

        private void DrawRooms()
        {
            var roomsData = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var rooms = roomsData.Rooms;
            if (rooms == null)
            {
                return;
            }
            
            var smallRooms = m_Generation.GetCash<SmallRoomsGenerationCash>();
            var borderingRooms = m_Generation.GetCash<BorderingRoomsGenerationCash>();
            for (int i = 0; i < rooms.Count; ++i)
            {
                Handles.color = Color.white;
                var room = rooms[i];
                var roomPosition = room.GetCenter();
                var position = new Vector3(roomPosition.X, roomPosition.Y);
                var size = new Vector3(room.Size.X, room.Size.Y, 0.1f);
                if (smallRooms.HasValue)
                {
                    if (smallRooms.Value.SmallRooms.Contains(room.UID))
                    {
                        Handles.color = Color.red;   
                    }
                }
                
                if (borderingRooms.HasValue)
                {
                    if (borderingRooms.Value.Rooms.Contains(room.UID))
                    {
                        Handles.color = Color.red;
                    }
                }

                if (roomsData.StartGenerationRoom != null && roomsData.StartGenerationRoom == room)
                {
                    Handles.color = Color.blue;
                }
                
                if (roomsData.EndGenerationRoom != null && roomsData.EndGenerationRoom == room)
                {
                    Handles.color = Color.blue;
                }
                
                Handles.DrawWireCube(position, size);
                if (m_ShowLabel)
                {
                    Handles.Label(position, RoomToString(room));
                }
            }
        }
        
        private void DrawRoomLabels()
        {
            var roomsData = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var rooms = roomsData.Rooms;
            if (rooms == null)
            {
                return;
            }
            
            for (int i = 0; i < rooms.Count; ++i)
            {
                var room = rooms[i];
                var roomPosition = room.GetCenter();
                var position = new Vector3(roomPosition.X, roomPosition.Y);
                if (m_ShowLabel)
                {
                    Handles.Label(position, RoomToString(room));
                }
            }
        }

        private string RoomToString(DungeonGenerationRoom generationRoom)
        {
            var roomsData = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var start = roomsData.StartGenerationRoom;
            var end = roomsData.EndGenerationRoom;
            
            var str = new StringBuilder();
            str.Append("[ ");
            if (generationRoom == start)
            {
                str.Append($"START\n");   
            } 
            else if (generationRoom == end)
            {
                str.Append($"END\n");   
            }
            
            str.Append($"UID: {generationRoom.UID}");
            str.Append($"\nIsMain: {generationRoom.IsMainPath}");
            if (generationRoom.RequiredKey != null)
            {
                str.Append($"\nRequiredKey: {generationRoom.RequiredKey.UID}");
            }
            
            if (generationRoom.ContainsDoorKeys.Count > 0)
            {
                str.Append($"\nContainsKey: {generationRoom.ContainsDoorKeys[0].UID}");
            }
            
            // if (room.Connections.Count > 0)
            // {
            //     str.Append("\n Connections: ");
            //     foreach (var connection in room.Connections)
            //     {
            //         str.Append($"{connection.UID} ");
            //     }
            // }
            
            str.Append(" ]");

            return str.ToString();
        }
    }
}