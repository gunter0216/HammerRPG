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
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Logger = App.Common.Logger.Runtime.Logger;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Editor
{
    [CustomEditor(typeof(MonoDungeonGenerator))]
    public class MonoDungeonGeneratorEditorV4 : UnityEditor.Editor
    {
        private readonly Runtime.DungeonGenerators.DungeonGenerator m_Generator = new(new Logger());
        private readonly DungeonGenerationDtoToConfigConverter m_DungeonGenerationDtoToConfigConverter = new();

        private DungeonGeneration m_Generation;
        private bool m_ShowLabel;
        private Transform _roomsContent;
        private List<GameObject> _rooms;

        void OnEnable()
        {
            HLogger.SetInstance(new UnityLogger());
            SceneView.duringSceneGui += WhenUpdate;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var myScript = (MonoDungeonGenerator)target;
            m_ShowLabel = myScript.ShowLabel;

            if (GUILayout.Button("Generate"))
            {
                var config = m_DungeonGenerationDtoToConfigConverter.Convert(myScript.Config);
                m_Generation = m_Generator.Generate(config).Value;
                Rebuild();
            }

            if (GUILayout.Button("Clear"))
            {
                UpdateRoot();
                Clear();
            }
        }

        void WhenUpdate(SceneView sceneView)
        {
            if (!m_Generator.IsStart()) return;
            if (m_Generation == null)   return;
            Draw();
        }

        private void Draw()    => DrawRoomLabels();
        private void Rebuild() { UpdateRoot(); Clear(); RebuildRooms().Forget(); }

        private void Clear()
        {
            if (_rooms == null) return;
            for (int i = _roomsContent.childCount - 1; i >= 0; --i)
                Object.DestroyImmediate(_roomsContent.GetChild(i).gameObject);
            _rooms.Clear();
        }

        private void UpdateRoot()
        {
            _rooms ??= new List<GameObject>();
            if (_roomsContent != null) return;

            foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
                if (go.name == "Rooms") { _roomsContent = go.transform; break; }

            if (_roomsContent == null)
                _roomsContent = new GameObject("Rooms").transform;
        }

        private async UniTask RebuildRooms()
        {
            var rooms = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var log   = new StringBuilder();
            log.AppendLine("=== RebuildRooms ===");

            foreach (var room in rooms.Rooms)
            {
                var config = room.ConfigAsset;
                var genPos = room.Position;
                Debug.LogError($"{config.AssetKey} {genPos} {room.RotateEuler}");

                // Компенсация смещения пивота при вращении Unity (ось Y, плоскость XZ).
                // PivotOffset не нужен — prefab сам имеет внутреннее смещение +0.5 по X и Z,
                // которое не меняется при вращении и не влияет на стыковку комнат.
                var comp     = GetRotationCompensation(room.Size, room.RotateEuler);
                var finalPos = new Vector3(genPos.X + comp.x, 0, genPos.Y + comp.y);

                // Ожидаемый AABB в тайловых координатах
                var tileOffset = GetTileRotationOffset(room.Size, room.RotateEuler);
                var aabbSize   = GetRotatedSize(room.Size, room.RotateEuler);
                var aabbMinX   = genPos.X + tileOffset.X;
                var aabbMinY   = genPos.Y + tileOffset.Y;
                var aabbMaxX   = aabbMinX + aabbSize.X - 1;
                var aabbMaxY   = aabbMinY + aabbSize.Y - 1;

                log.AppendLine($"--- {config.AssetKey} depth={room.Depth} ---");
                log.AppendLine($"  genPos=({genPos.X},{genPos.Y})  size={room.Size.X}x{room.Size.Y}  rot={room.RotateEuler}");
                log.AppendLine($"  compensation=({comp.x},{comp.y})");
                log.AppendLine($"  finalPos=({finalPos.x},{finalPos.z}) [X,Z]");
                log.AppendLine($"  expectedAABB X[{aabbMinX}..{aabbMaxX}] Y[{aabbMinY}..{aabbMaxY}]");
                log.AppendLine($"  expectedFinalPos X={aabbMinX} Z={aabbMinY}");

                var roomObj = await Addressables.InstantiateAsync(
                    config.AssetKey,
                    finalPos,
                    Quaternion.Euler(0, room.RotateEuler, 0),
                    _roomsContent);

                roomObj.name = $"{config.AssetKey} depth {room.Depth}";
                _rooms.Add(roomObj);

                // Логируем localPosition (без внутреннего смещения prefab'а +0.5)
                var lp = roomObj.transform.localPosition;
                bool matchX = Mathf.Approximately(lp.x, aabbMinX);
                bool matchZ = Mathf.Approximately(lp.z, aabbMinY);
                log.AppendLine($"  localPos=({lp.x},{lp.z}) [X,Z]  match={matchX && matchZ}");
            }

            Debug.Log(log.ToString());
        }

        // Компенсация смещения пивота при вращении Unity по оси Y (плоскость XZ).
        // Пивот = левый нижний угол комнаты. После вращения он остаётся на месте,
        // но комната "растёт" в другую сторону — компенсируем сдвигом позиции.
        //
        //   rot=0°:   (0, 0)
        //   rot=90°:  (0, W)   — комната уходит по -Z, сдвигаем +Z на W
        //   rot=180°: (W, H)
        //   rot=270°: (H, 0)   — комната уходит по -X, сдвигаем +X на H
        private Vector2 GetRotationCompensation(Vector2Int size, float rotation)
        {
            int w = size.X, h = size.Y;

            if (Mathf.Approximately(rotation, 90))  return new Vector2(0, w);
            if (Mathf.Approximately(rotation, 180)) return new Vector2(w, h);
            if (Mathf.Approximately(rotation, 270)) return new Vector2(h, 0);
            return Vector2.zero;
        }

        // Тайловый offset AABB относительно room.Position (только для логов)
        private Vector2Int GetTileRotationOffset(Vector2Int size, float rotation)
        {
            int w = size.X, h = size.Y;

            if (Mathf.Approximately(rotation, 90))  return new Vector2Int(h - 1, 0);
            if (Mathf.Approximately(rotation, 180)) return new Vector2Int(w - 1, h - 1);
            if (Mathf.Approximately(rotation, 270)) return new Vector2Int(0,     w - 1);
            return new Vector2Int(0, 0);
        }

        private Vector2Int GetRotatedSize(Vector2Int size, float rotation)
        {
            if (Mathf.Approximately(rotation, 90) || Mathf.Approximately(rotation, 270))
                return new Vector2Int(size.Y, size.X);
            return size;
        }

        private void DrawRoomLabels()
        {
            var roomsData = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var rooms = roomsData.Rooms;
            if (rooms == null) return;

            for (int i = 0; i < rooms.Count; ++i)
            {
                var room         = rooms[i];
                var roomPosition = room.GetCenter();
                var position     = new Vector3(roomPosition.X, roomPosition.Y);
                if (m_ShowLabel)
                    Handles.Label(position, RoomToString(room));
            }
        }

        private string RoomToString(DungeonGenerationRoom generationRoom)
        {
            var roomsData = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var start = roomsData.StartGenerationRoom;
            var end   = roomsData.EndGenerationRoom;

            var str = new StringBuilder();
            str.Append("[ ");
            if (generationRoom == start)    str.Append("START\n");
            else if (generationRoom == end) str.Append("END\n");

            str.Append($"UID: {generationRoom.UID}");
            str.Append($"\nIsMain: {generationRoom.IsMainPath}");

            if (generationRoom.RequiredKey != null)
                str.Append($"\nRequiredKey: {generationRoom.RequiredKey.UID}");

            if (generationRoom.ContainsDoorKeys.Count > 0)
                str.Append($"\nContainsKey: {generationRoom.ContainsDoorKeys[0].UID}");

            str.Append(" ]");
            return str.ToString();
        }
    }
}