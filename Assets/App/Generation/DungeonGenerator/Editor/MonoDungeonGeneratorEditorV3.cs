using System.Collections.Generic;
using System.Text;
using App.Common.Logger.External;
using App.Common.Logger.Runtime;
using App.Generation.DungeonGenerator.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.CreateDoors.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SquarePartition;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Logger = App.Common.Logger.Runtime.Logger;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Editor
{
    [CustomEditor(typeof(MonoDungeonGenerator))]
    public class MonoDungeonGeneratorEditorV3 : UnityEditor.Editor
    {
        private class MonoTile
        {
            public SpriteRenderer SpriteRenderer;
            public GeneraitonTile GeneraitonTile;
        }

        private class DrawRectangle
        {
            public Color Color;
            public Vector2 Position;
            public Vector2 Size;
            public bool IsFill;
        }

        private List<DrawRectangle> _rectangles; 

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
            HLogger.SetInstance(new UnityLogger());
            SceneView.duringSceneGui += WhenUpdate;
        }

        private bool _needUpdate;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var myScript = (MonoDungeonGenerator)target;
            m_ShowLabel = myScript.ShowLabel;
            m_ShowRoomBorders = myScript.ShowRoomBorders;
            if (GUILayout.Button("Generate"))
            {
                var config = m_DungeonGenerationDtoToConfigConverter.Convert(myScript.Config);
                m_Generation = m_Generator.Generate(config).Value;
                Rebuild();
                SceneView.RepaintAll();
            }

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

            if (m_Generation == null)
            {
                return;
            }

            if (_rectangles == null)
            {
                return;
            }
            
            Draw();
        }

        private void Draw()
        {
            foreach (var rectangle in _rectangles)
            {
                Handles.color = rectangle.Color;

                if (rectangle.IsFill)
                {
                    var tilePosition = rectangle.Position;
                    var rect = new Rect
                    {
                        xMin = tilePosition.x - 0.5f,
                        xMax = tilePosition.x + 0.5f,
                        yMin = tilePosition.y - 0.5f,
                        yMax = tilePosition.y + 0.5f
                    };
                    Handles.DrawSolidRectangleWithOutline(rect, rectangle.Color, rectangle.Color);
                }
                else
                {
                    Handles.DrawWireCube(rectangle.Position, rectangle.Size);
                }
            }
            
            if (m_Generation.TryGetCash<SpanningTreeGenerationCash>(out var spanningTreeGenerationCash))
            {
                DrawTree(spanningTreeGenerationCash);
            }
        }

        private void Rebuild()
        {
            _rectangles ??= new List<DrawRectangle>();
            _rectangles.Clear();

            RebuildMatrix();
            RebuildAreas();
            RebuildRooms();
            RebuildCorridors();
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

        private void RebuildRooms()
        {
            var data = m_Generation.DungeonGenerationResult.GenerationData;
            var rooms = data.GenerationRooms;

            for (int i = 0; i < rooms.Rooms.Count; ++i)
            {
                var room = rooms.Rooms[i];
                var roomPosition = room.Position.ToVector();
                var roomSize = room.Size.ToVector();
                var center = roomPosition + (roomSize / 2);
                var position = new Vector3(center.X, center.Y);
                var size = new Vector3(room.Size.X, room.Size.Y, 0.1f);

                Color color = Color.green;
                if (room == rooms.StartGenerationRoom)
                {
                    color = Color.cyan;
                } 
                else if (room == rooms.EndGenerationRoom)
                {
                    color = Color.magenta;
                }
                
                AddRectangle(color, position, size, isFill: false);
            }
        }

        private void RebuildCorridors()
        {
            var rooms = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            for (int i = 0; i < rooms.Rooms.Count; ++i)
            {
                var room = rooms.Rooms[i];
                var corridor = room.Corridor;
                if (corridor == null)
                {
                    continue;
                }

                var area = corridor.Area;
                
                var roomPosition = room.LocalToWorld(area.Position).ToVector();
                var roomSize = area.Size.ToVector();
                var center = roomPosition + (roomSize / 2);
                var position = new Vector3(center.X, center.Y);
                var size = new Vector3(area.Size.X, area.Size.Y, 0.1f);

                AddRectangle(Color.blue, position, size, isFill: false);
            }
        }

        private void RebuildAreas()
        {
            var squareGenerationCash = m_Generation.GetCash<SquareGenerationCash>();
            for (int i = 0; i < squareGenerationCash.Value.Areas.Count; ++i)
            {
                var area = squareGenerationCash.Value.Areas[i];
                var roomPosition = area.Position.ToVector();
                var roomSize = area.Size.ToVector();
                var center = roomPosition + (roomSize / 2);
                var position = new Vector3(center.X, center.Y);
                var size = new Vector3(area.Size.X, area.Size.Y, 0.1f);

                AddRectangle(Color.red, position, size, isFill: false);
            }
        }

        private void AddRectangle(Color color,
            Vector2 center,
            Vector2 position,
            bool isFill)
        {
            _rectangles.Add(new DrawRectangle()
            {
                Color = color, 
                Position = center, 
                Size = position,
                IsFill = isFill
            });
        }

        private void RebuildMatrix()
        {
            var rooms = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms;
            foreach (var room in rooms)
            {
                var matrix = room.Tiles;
                foreach (var roomTile in room.Tiles)
                {
                    var position = roomTile.Key;
                    var tile = roomTile.Value;
                    
                    position = room.LocalToWorld(position);

                    if (tile.Id == DungeonTile.Empty)
                    {
                        continue;
                    }

                    var color = Color.black;
                    if (tile.Id == DungeonTile.Door)
                    {
                        color = Color.green;
                    }

                    var center = new Vector2(position.X + 0.5f, position.Y + 0.5f);
                    AddRectangle(color, center, new Vector2(1, 1), isFill: true);
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