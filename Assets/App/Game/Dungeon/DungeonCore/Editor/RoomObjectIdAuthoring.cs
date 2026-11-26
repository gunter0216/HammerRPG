using System;
using System.Collections.Generic;
using App.Game.Dungeon.DungeonCore.External.View;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.Editor
{
    [InitializeOnLoad]
    public static class RoomObjectIdAuthoring
    {
        private const string RoomFolder = "Assets/Prefabs/Tiles/Rooms/";

        private static GameObject _currentRoom;
        private static readonly Dictionary<string, CreatingModuleItemView> _knownOwners = new();

        static RoomObjectIdAuthoring()
        {
            EditorApplication.hierarchyChanged += UpdateCurrentRoom;
            PrefabStage.prefabStageOpened += UpdateRoom;
            PrefabStage.prefabStageClosing += OnPrefabStageClosing;
            PrefabStage.prefabSaving += OnPrefabSaving;
            EditorApplication.delayCall += UpdateCurrentRoom;
        }

        public static bool AssignIds(GameObject roomRoot)
        {
            return AssignIds(roomRoot, new Dictionary<string, CreatingModuleItemView>());
        }

        private static void UpdateCurrentRoom() => UpdateRoom(PrefabStageUtility.GetCurrentPrefabStage());

        private static void OnPrefabSaving(GameObject root)
        {
            UpdateRoom(PrefabStageUtility.GetPrefabStage(root));
        }

        private static void OnPrefabStageClosing(PrefabStage stage)
        {
            if (_currentRoom != stage.prefabContentsRoot)
                return;

            _currentRoom = null;
            _knownOwners.Clear();
        }

        private static void UpdateRoom(PrefabStage stage)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || stage == null ||
                !stage.assetPath.StartsWith(RoomFolder, StringComparison.Ordinal) || stage.prefabContentsRoot == null)
                return;

            var root = stage.prefabContentsRoot;
            if (_currentRoom != root)
            {
                _currentRoom = root;
                _knownOwners.Clear();
            }

            if (AssignIds(root, _knownOwners))
                EditorSceneManager.MarkSceneDirty(root.scene);
        }

        private static bool AssignIds(GameObject roomRoot, Dictionary<string, CreatingModuleItemView> knownOwners)
        {
            var dynamicRoot = roomRoot.transform.Find("[DYNAMIC]");
            if (dynamicRoot == null)
                return false;

            bool changed = false;
            var usedIds = new HashSet<string>();
            foreach (var view in dynamicRoot.GetComponentsInChildren<CreatingModuleItemView>(true))
            {
                var id = view.ObjectId;
                // A duplicate can be reordered ahead of its original before the room is saved.
                if (Guid.TryParseExact(id, "N", out _) &&
                    (!knownOwners.TryGetValue(id, out var owner) || owner == view) && usedIds.Add(id))
                {
                    knownOwners[id] = view;
                    continue;
                }

                do
                {
                    id = Guid.NewGuid().ToString("N");
                } while (knownOwners.ContainsKey(id) || !usedIds.Add(id));

                var serialized = new SerializedObject(view);
                serialized.FindProperty("_objectId").stringValue = id;
                serialized.ApplyModifiedPropertiesWithoutUndo();
                PrefabUtility.RecordPrefabInstancePropertyModifications(view);
                knownOwners.Add(id, view);
                changed = true;
            }

            return changed;
        }
    }
}
