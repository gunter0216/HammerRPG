using App.Generation.DungeonGenerator.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash;
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
            DrawRooms();
            if (m_Generation.TryGetCash<SpanningTreeGenerationCash>(out var spanningTree))
            {
                DrawTree(spanningTree);
            }
            else if (m_Generation.TryGetCash<TriangulationGenerationCash>(out var triangulation))
            {
                DrawTriangulation(triangulation);
            }
        }

        private void DrawTree(SpanningTreeGenerationCash spanningTree)
        {
            var indexToPoint = spanningTree.Result.IndexToPoint;
            var tree = spanningTree.Result.MinimumSpanningTree;
            
            Handles.color = Color.yellow;
            foreach (var edge in tree)
            {
                var point1 = indexToPoint[edge.Source];
                var point2 = indexToPoint[edge.Destination];
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
                
                Handles.DrawWireCube(position, size);
            }
        }
    }
}