using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Game.Core.Modules.Config.Editor.ScriptableWindows
{
    public static class ScriptableObjectTagsExtensions
    {
        public static List<string> GetTagsFromDatabase(this ScriptableObject obj)
            => ScriptableObjectTagsDatabase.Instance?.GetTags(obj, true) ?? new List<string>();
    }
    
    [CreateAssetMenu(
        menuName = "Config/Tags/ScriptableObject Tags Database",
        fileName = "ScriptableObjectTagsDatabase")]
    public class ScriptableObjectTagsDatabase : ScriptableObject
    {
        [Serializable]
        public class Entry
        {
            public string guid;
            public List<string> tags = new();
        }

        [SerializeField]
        private List<Entry> _entries = new();

        private Dictionary<string, Entry> _cache;

        private void OnEnable()
        {
            _cache = _entries
                .Where(e => !string.IsNullOrEmpty(e.guid))
                .ToDictionary(e => e.guid, e => e);
        }

        private Entry GetOrCreateEntry(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return null;

            if (_cache == null)
                OnEnable();

            if (_cache.TryGetValue(guid, out var entry))
                return entry;

            entry = new Entry { guid = guid, tags = new List<string>() };
            _entries.Add(entry);
            _cache[guid] = entry;
            return entry;
        }

        public List<string> GetTags(ScriptableObject obj, bool createIfMissing = false)
        {
            if (obj == null) return new List<string>();

            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) return new List<string>();

            string guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid)) return new List<string>();

            var entry = createIfMissing ? GetOrCreateEntry(guid) : _cache?.GetValueOrDefault(guid);
            return entry?.tags ?? new List<string>();
        }

        public void SetTags(ScriptableObject obj, IEnumerable<string> tags)
        {
            if (obj == null) return;

            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) return;

            string guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid)) return;

            var entry = GetOrCreateEntry(guid);
            entry.tags = tags?
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim())
                .Distinct()
                .OrderBy(t => t)
                .ToList()
                ?? new List<string>();

            EditorUtility.SetDirty(this);
        }

        public void AddTag(ScriptableObject obj, string tag)
        {
            if (obj == null || string.IsNullOrWhiteSpace(tag)) return;
            tag = tag.Trim();
            var list = GetTags(obj, true);
            if (!list.Contains(tag))
            {
                list.Add(tag);
                list.Sort(StringComparer.Ordinal);
                EditorUtility.SetDirty(this);
            }
        }

        public void RemoveTagEverywhere(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return;
            tag = tag.Trim();

            bool changed = false;
            foreach (var e in _entries)
            {
                if (e.tags == null) continue;
                if (e.tags.RemoveAll(t => string.Equals(t, tag, StringComparison.Ordinal)) > 0)
                    changed = true;
            }

            if (changed)
                EditorUtility.SetDirty(this);
        }

        public List<string> GetAllTagsFromObjects(IEnumerable<ScriptableObject> objects)
        {
            var set = new HashSet<string>();
            if (objects == null) return new List<string>();

            foreach (var obj in objects)
            {
                var tags = GetTags(obj, false);
                foreach (var t in tags)
                {
                    var v = (t ?? "").Trim();
                    if (!string.IsNullOrEmpty(v))
                        set.Add(v);
                }
            }

            return set.OrderBy(s => s).ToList();
        }

        private static ScriptableObjectTagsDatabase _instance;
        public static ScriptableObjectTagsDatabase Instance
        {
            get
            {
#if UNITY_EDITOR
                
                if (_instance == null)
                {
                    var guids = AssetDatabase.FindAssets("t:ScriptableObjectTagsDatabase");
                    if (guids.Length > 0)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        _instance = AssetDatabase.LoadAssetAtPath<ScriptableObjectTagsDatabase>(path);
                    }
                }
#endif
                return _instance;
            }
        }
    }
}