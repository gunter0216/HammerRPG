using App.Generation.DungeonGenerator.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Cash;
using UnityEditor;
using UnityEngine;
using Logger = App.Common.Logger.Runtime.Logger;

namespace App.Generation.DungeonGenerator.Editor
{
    [CustomEditor(typeof(MonoDungeonGenerator))]
    public class MonoDungeonGeneratorEditorV2 : UnityEditor.Editor 
    {
        private readonly Runtime.DungeonGenerators.DungeonGenerator m_Generator = new(new Logger());
        private readonly DungeonGenerationDtoToConfigConverter m_DungeonGenerationDtoToConfigConverter = new();
        
        private DungeonGeneration m_Generation;

        void OnEnable()
        {
            SceneView.duringSceneGui += WhenUpdate;
        }

        public override void OnInspectorGUI() 
        {
            DrawDefaultInspector();
            
            var myScript = (MonoDungeonGenerator)target;
            if (GUILayout.Button("Start Generate"))
            {
                var config = m_DungeonGenerationDtoToConfigConverter.Convert(myScript.Config);
                m_Generator.StartGeneration(config);
            }
            
            if (GUILayout.Button("Next Iteration"))
            {
                m_Generator.NextIteration();
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
            DrawRooms();
            DrawTriangulation();
            // Handles.color = Color.magenta;
            // Handles.DrawWireCube(Vector2.zero, Vector3.one);
            //
            // Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            // for (int x = -10; x < 10; ++x)
            // {
            //     for (int y = -10; y < 10; ++y)
            //     {
            //         var position = new Vector3(x + 0.5f, y + 0.5f);
            //         var size = new Vector3(1, 1, 0.1f);
            //         Handles.DrawWireCube(position, size);
            //     }
            // }
            //
            // if (m_ShowTriangulation)
            // {
            //     Handles.color = Color.blue;
            //     foreach (var triangle in m_Triangulation)
            //     {
            //         var points = new Vector3[3];
            //         points[0] = new Vector3(triangle.A.X, triangle.A.Y, 0);
            //         points[1] = new Vector3(triangle.B.X, triangle.B.Y, 0);
            //         points[2] = new Vector3(triangle.C.X, triangle.C.Y, 0);
            //         Handles.DrawPolyLine(points);
            //     }
            // }
            //
            // if (m_ShowTree)
            // {
            //     Handles.color = Color.yellow;
            //     foreach (var edge in m_Tree.MinimumSpanningTree)
            //     {
            //         var point1 = m_IndexToPoint[edge.Source];
            //         var point2 = m_IndexToPoint[edge.Destination];
            //         var points = new Vector3[2];
            //         points[0] = new Vector3(point1.X, point1.Y, 0);
            //         points[1] = new Vector3(point2.X, point2.Y, 0);
            //         Handles.DrawPolyLine(points);
            //     }
            // }

            // Handles.color = Color.red;
            // foreach (var room in m_Dungeon.Data.RoomsData.Rooms)
            // {
            //     var roomPosition = room.GetCenter();
            //     Handles.DrawWireCube(roomPosition, Vector3.one);
            //     Handles.Label(roomPosition, room.ToString());
            // }
        }

        private void DrawTriangulation()
        {
            // if (m_ShowTriangulation)
            // {
            //     Handles.color = Color.blue;
            //     foreach (var triangle in m_Triangulation)
            //     {
            //         var points = new Vector3[3];
            //         points[0] = new Vector3(triangle.A.X, triangle.A.Y, 0);
            //         points[1] = new Vector3(triangle.B.X, triangle.B.Y, 0);
            //         points[2] = new Vector3(triangle.C.X, triangle.C.Y, 0);
            //         Handles.DrawPolyLine(points);
            //     }
            // }
        }

        private void DrawRooms()
        {
            var rooms = m_Generation.Dungeon.Data.RoomsData.Rooms;
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
                var position = new Vector3(roomPosition.x, roomPosition.y);
                var size = new Vector3(room.Size.x, room.Size.y, 0.1f);
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
                
                Handles.DrawWireCube(position, size);
            }
        }
    }
}