using App.Game.DungeonGenerator.External;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
using App.Game.DungeonGenerator.Runtime.Rooms;
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

            foreach (var roomData in m_Dungeon.Data.RoomsData.Rooms)
            {
                Debug.LogError($"{roomData}");
            }
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
                    
                    foreach (var roomData in m_Dungeon.Data.RoomsData.Rooms)
                    {
                        Debug.LogError($"{roomData}");
                    }
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
            
            Handles.color = Color.white;
            foreach (var room in m_Dungeon.Data.RoomsData.Rooms)
            {
                var roomPosition = room.GetCenter();
                var position = new Vector3(roomPosition.x, roomPosition.y);
                var size = new Vector3(room.Size.x, room.Size.y, 0.1f);
                // position *= 2;
                // size *= 2;
                // Debug.LogError($"position {position} size {size}");
                Handles.DrawWireCube(position, size);
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