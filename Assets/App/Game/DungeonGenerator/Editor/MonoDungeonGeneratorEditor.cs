using System.Collections.Generic;
using App.Game.DelaunayTriangulation.Runtime;
using App.Game.DungeonGenerator.External;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Game.DungeonGenerator.Runtime.Rooms;
using App.Game.KruskalAlgorithm.Runtime;
using UnityEditor;
using UnityEngine;

namespace App.Game.DungeonGenerator.Editor
{
    [CustomEditor(typeof(MonoDungeonGenerator))]
    public class MonoDungeonGeneratorEditor : UnityEditor.Editor 
    {
        private readonly Runtime.DungeonGenerators.DungeonGenerator m_DungeonGenerator = new();

        private Dungeon m_Dungeon;

        private DungeonGenerateStage m_Stage = DungeonGenerateStage.None;
        private bool m_ShowDiscardingRooms;
        private HashSet<int> m_SmallRooms;
        private bool m_ShowBorderingRooms;
        private HashSet<int> m_BorderingRooms;
        private bool m_ShowTriangulation;
        private List<Triangle> m_Triangulation;
        private bool m_ShowTree;
        private KruskalResult m_Tree;
        private Dictionary<int, Point> m_IndexToPoint;

        void OnEnable()
        {
            SceneView.duringSceneGui += WhenUpdate;
        }

        public override void OnInspectorGUI() 
        {
            DrawDefaultInspector();
            
            var myScript = (MonoDungeonGenerator)target;
            if (GUILayout.Button("Generate")) 
            {
                Generate(myScript);
            }
            
            if (GUILayout.Button("Separate")) 
            {
                m_Stage = DungeonGenerateStage.SeparateRooms;
            }

            if (m_ShowDiscardingRooms = GUILayout.Toggle(m_ShowDiscardingRooms, "Show Small Rooms"))
            {
                if (m_Dungeon != null)
                {
                    if (m_SmallRooms == null)
                    {
                        m_SmallRooms = m_DungeonGenerator.GetSmallRooms(m_Dungeon);
                    }
                }
            }
            else
            {
                m_SmallRooms = null;
            }
            
            if (GUILayout.Button("Discard Small Rooms")) 
            {
                m_DungeonGenerator.DiscardRooms(m_Dungeon, m_SmallRooms);
            }
            
            if (m_ShowBorderingRooms = GUILayout.Toggle(m_ShowBorderingRooms, "Show Bordering Rooms"))
            {
                if (m_Dungeon != null)
                {
                    if (m_BorderingRooms == null)
                    {
                        m_BorderingRooms = m_DungeonGenerator.GetBorderingRooms(m_Dungeon);
                    }
                }
            }
            else
            {
                m_BorderingRooms = null;
            }
            
            if (GUILayout.Button("Discard Bordering Rooms")) 
            {
                if (m_Dungeon != null && m_BorderingRooms != null)
                {
                    m_Dungeon.DiscardRooms(m_BorderingRooms);
                }
            }
            
            if (m_ShowTriangulation = GUILayout.Toggle(m_ShowTriangulation, "Show Triangulation"))
            {
                if (m_Dungeon != null)
                {
                    if (m_Triangulation == null)
                    {
                        m_Triangulation = m_DungeonGenerator.Triangulate(m_Dungeon);
                    }
                }
            }
            else
            {
                m_Triangulation = null;
            }
            
            if (m_ShowTree = GUILayout.Toggle(m_ShowTree, "Show Tree"))
            {
                if (m_Dungeon != null)
                {
                    if (m_Tree == null)
                    {
                        var result = m_DungeonGenerator.FindMinimumSpanningTree(m_Triangulation);
                        m_Tree = result.result;
                        m_IndexToPoint = result.indexToPoint;
                    }
                }
            }
            else
            {
                m_Tree = null;
                m_IndexToPoint = null;
            }
        }

        private void Generate(MonoDungeonGenerator mono)
        {
            // var config = new DungeonConfig()
            // {
            //     Width = mono.Width,
            //     Height = mono.Height,
            //     DungeonRoomsConfig = new DungeonRoomsConfig()
            //     {
            //         CountRooms = mono.CountRooms,
            //         MinWidthRoom = mono.MinWidthRoom,
            //         MaxWidthRoom = mono.MaxWidthRoom,
            //         MinHeightRoom = mono.MinHeightRoom,
            //         MaxHeightRoom = mono.MaxHeightRoom,
            //         RoomsRadius = mono.RoomsRadius
            //     }
            // };
            
            m_Dungeon = m_DungeonGenerator.Generate(mono.Config);
            m_DungeonGenerator.GenerateRooms(m_Dungeon);

            // foreach (var roomData in m_Dungeon.Data.RoomsData.Rooms)
            // {
            //     Debug.LogError($"{roomData}");
            // }
        }

        void WhenUpdate(SceneView sceneView)
        {
            if (m_Dungeon == null)
            {
                return;
            }
            
            DrawRooms();
            if (m_Stage == DungeonGenerateStage.SeparateRooms)
            {
                // m_DungeonGenerator.SeparateRooms(m_Dungeon);
                if (m_DungeonGenerator.SeparateRooms(m_Dungeon))
                {
                    Debug.LogError("End separated");
                    m_Stage = DungeonGenerateStage.None;
                }
            }
        }

        private void DrawRooms()
        {
            Handles.color = Color.magenta;
            Handles.DrawWireCube(Vector2.zero, Vector3.one);
            
            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            for (int x = -10; x < 10; ++x)
            {
                for (int y = -10; y < 10; ++y)
                {
                    var position = new Vector3(x + 0.5f, y + 0.5f);
                    var size = new Vector3(1, 1, 0.1f);
                    Handles.DrawWireCube(position, size);
                }
            }

            var rooms = m_Dungeon.Data.RoomsData.Rooms;
            for (int i = 0; i < rooms.Count; ++i)
            {
                Handles.color = Color.white;
                var room = rooms[i];
                var roomPosition = room.GetCenter();
                var position = new Vector3(roomPosition.x, roomPosition.y);
                var size = new Vector3(room.Size.x, room.Size.y, 0.1f);
                // position *= 2;
                // size *= 2;
                // Debug.LogError($"position {position} size {size}");
                if (m_ShowDiscardingRooms)
                {
                    if (m_SmallRooms.Contains(room.UID))
                    {
                        Handles.color = Color.red;   
                    }
                }
                
                if (m_ShowBorderingRooms)
                {
                    if (m_BorderingRooms.Contains(room.UID))
                    {
                        Handles.color = Color.magenta;   
                    }
                }
                
                Handles.DrawWireCube(position, size);
            }

            if (m_ShowTriangulation)
            {
                Handles.color = Color.blue;
                foreach (var triangle in m_Triangulation)
                {
                    var points = new Vector3[3];
                    points[0] = new Vector3(triangle.A.X, triangle.A.Y, 0);
                    points[1] = new Vector3(triangle.B.X, triangle.B.Y, 0);
                    points[2] = new Vector3(triangle.C.X, triangle.C.Y, 0);
                    Handles.DrawPolyLine(points);
                }
            }

            if (m_ShowTree)
            {
                Handles.color = Color.yellow;
                foreach (var edge in m_Tree.MinimumSpanningTree)
                {
                    var point1 = m_IndexToPoint[edge.Source];
                    var point2 = m_IndexToPoint[edge.Destination];
                    var points = new Vector3[2];
                    points[0] = new Vector3(point1.X, point1.Y, 0);
                    points[1] = new Vector3(point2.X, point2.Y, 0);
                    Handles.DrawPolyLine(points);
                }
            }
            
            // Handles.color = Color.red;
            // foreach (var room in m_Dungeon.Data.RoomsData.Rooms)
            // {
            //     var roomPosition = room.GetCenter();
            //     Handles.DrawWireCube(roomPosition, Vector3.one);
            //     Handles.Label(roomPosition, room.ToString());
            // }
        }
    }
}