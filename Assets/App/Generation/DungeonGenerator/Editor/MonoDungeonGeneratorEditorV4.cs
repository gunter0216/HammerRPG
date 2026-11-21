using System;
using System.Reflection;
using App.Common.Logger.External;
using App.Common.Logger.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Converter;
using App.Generation.DungeonGenerator.External;
using App.Generation.DungeonGenerator.Runtime.Configs;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Logger = App.Common.Logger.Runtime.Logger;

namespace App.Generation.DungeonGenerator.Editor
{
    [CustomEditor(typeof(MonoDungeonGenerator))]
    public class MonoDungeonGeneratorEditorV4 : UnityEditor.Editor
    {
        private readonly Runtime.DungeonGenerators.DungeonGenerator m_Generator = new(new Logger());
        private readonly GenerationConfigConverter m_ConfigConverter = new();
        private DungeonGeneration m_Generation;
        private Transform m_Root;

        private void OnEnable()
        {
            HLogger.SetInstance(new UnityLogger());
            SceneView.duringSceneGui += DrawLabels;
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= DrawLabels;
            Undo.undoRedoPerformed -= OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            m_Generation = null;
            SceneView.RepaintAll();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var component = (MonoDungeonGenerator)target;
            if (component.GenerationConfig == null)
                EditorGUILayout.ObjectField("Default Generation Config", GetConfig(), typeof(GenerationConfig), false);

            using (new EditorGUI.DisabledScope(EditorApplication.isPlayingOrWillChangePlaymode))
            {
                if (GUILayout.Button("Generate") && !Generate())
                    Debug.LogError("Could not generate the 2D dungeon. Check the generation config and room prefabs.", component);
                if (GUILayout.Button("Clear") && !ClearGenerated())
                    Debug.LogError("Could not clear the generated dungeon.", component);
            }
        }

        private GenerationConfig GetConfig() => ((MonoDungeonGenerator)target).GenerationConfig != null
            ? ((MonoDungeonGenerator)target).GenerationConfig
            : AssetDatabase.LoadAssetAtPath<GenerationConfig>("Assets/Configs/Dungeon/GenerationConfig.asset");

        public bool Generate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return false;
            var component = (MonoDungeonGenerator)target;
            if (component.ClearConsole)
                Type.GetType("UnityEditor.LogEntries, UnityEditor.dll")?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public)?.Invoke(null, null);
            var configs = m_ConfigConverter.Convert(GetConfig());
            if (!configs.HasValue) return false;
            var key = string.IsNullOrWhiteSpace(component.GenerationKey) ? "Default" : component.GenerationKey;
            var config = Array.Find(configs.Value.Configs, item => item.Key == key);
            if (config == null) return false;
            var result = m_Generator.Generate(config);
            if (!result.HasValue) return false;
            var rooms = result.Value.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms;
            foreach (var room in rooms)
            {
                var prefab = room.ConfigVariant.AssetReference?.editorAsset;
                if (prefab == null || FindMap(prefab, "Floor") == null || FindMap(prefab, "Walls") == null)
                    return false;
            }

            Undo.IncrementCurrentGroup();
            var undoGroup = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Generate 2D dungeon");
            try
            {
                UpdateRoot();
                ClearRooms();
                foreach (var room in rooms)
                {
                    var prefab = room.ConfigVariant.AssetReference.editorAsset;
                    var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, m_Root);
                    Undo.RegisterCreatedObjectUndo(instance, "Create dungeon room");
                    RoomPlacement.Apply(instance.transform, room.Position, room.GenerationConfig.PrefabOrigin, room.RotateEuler);
                    instance.transform.localScale = Vector3.one;
                    instance.name = $"{prefab.name} depth {room.Depth}";
                    PrefabUtility.RecordPrefabInstancePropertyModifications(instance.transform);
                }
                m_Generation = result.Value;
                EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
                Undo.CollapseUndoOperations(undoGroup);
                SceneView.RepaintAll();
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, component);
                Undo.RevertAllDownToGroup(undoGroup);
                return false;
            }
        }

        private static Tilemap FindMap(GameObject room, string name) =>
            Array.Find(room.GetComponentsInChildren<Tilemap>(true), map => map.name == name);

        private void UpdateRoot()
        {
            var component = (MonoDungeonGenerator)target;
            m_Root = component.transform.Find("Rooms");
            if (m_Root != null) return;
            var root = new GameObject("Rooms");
            SceneManager.MoveGameObjectToScene(root, component.gameObject.scene);
            Undo.RegisterCreatedObjectUndo(root, "Create dungeon root");
            Undo.SetTransformParent(root.transform, component.transform, "Parent dungeon root");
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            m_Root = root.transform;
        }

        private void ClearRooms()
        {
            for (var i = m_Root.childCount - 1; i >= 0; i--)
                Undo.DestroyObjectImmediate(m_Root.GetChild(i).gameObject);
        }

        public bool ClearGenerated()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return false;
            m_Root = ((MonoDungeonGenerator)target).transform.Find("Rooms");
            if (m_Root == null) return true;
            Undo.IncrementCurrentGroup();
            var group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Clear 2D dungeon");
            ClearRooms();
            m_Generation = null;
            Undo.CollapseUndoOperations(group);
            return true;
        }

        private void DrawLabels(SceneView view)
        {
            if (m_Generation == null || m_Root == null || !((MonoDungeonGenerator)target).ShowLabel) return;
            foreach (var room in m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms)
            {
                var index = m_Generation.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms.IndexOf(room);
                if (index >= m_Root.childCount) continue;
                var instance = m_Root.GetChild(index);
                var origin = room.GenerationConfig.PrefabOrigin;
                var center = instance.TransformPoint(new Vector3(origin.X + room.Size.X * .5f, origin.Y + room.Size.Y * .5f));
                Handles.Label(center, $"{room.GenerationConfig.RoomType}\nUID: {room.UID}\nDepth: {room.Depth}");
            }
        }
    }
}
