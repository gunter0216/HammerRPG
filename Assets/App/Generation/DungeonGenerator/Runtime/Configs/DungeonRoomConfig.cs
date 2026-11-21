using System;
using System.Collections.Generic;
using App.Common.Configs.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.TileBased;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.Configs
{
    [CreateAssetMenu(fileName = "RoomConfig", menuName = "Autumn/Dungeon/Room Config")]
    public class DungeonRoomConfig : GameConfig
    {
        [SerializeField] private string _id;
        [SerializeField] private RoomType _roomType;
        [SerializeField] private RectInt _bounds;
        [Tooltip("First cell of the two-tile opening: left for horizontal doors, bottom for vertical doors.")]
        [SerializeField] private UnityEngine.Vector2Int _inputDoor;
        [SerializeField] private UnityEngine.Vector2Int[] _outputDoors = Array.Empty<UnityEngine.Vector2Int>();
        [SerializeField] private AssetReferenceGameObject _assetReference = new AssetReferenceGameObject(string.Empty);

        public string Id => _id;
        public RoomType RoomType => _roomType;
        public RectInt Bounds => _bounds;
        public UnityEngine.Vector2Int InputDoor => _inputDoor;
        public IReadOnlyList<UnityEngine.Vector2Int> OutputDoors => _outputDoors;
        public AssetReferenceGameObject AssetReference => _assetReference;

        public bool TryCreateRuntimeConfig(out RoomGenerationConfig config)
        {
            config = null;
            if (string.IsNullOrWhiteSpace(_id) || _bounds.width < 3 || _bounds.height < 3 ||
                _assetReference == null || !_assetReference.RuntimeKeyIsValid() || _outputDoors == null)
                return false;

            var occupied = new HashSet<UnityEngine.Vector2Int>();
            if (!TryAddDoor(_inputDoor, occupied))
                return false;
            foreach (var door in _outputDoors)
                if (!TryAddDoor(door, occupied))
                    return false;

            switch (_roomType)
            {
                case RoomType.Start:
                case RoomType.Transit:
                    if (_outputDoors.Length == 0) return false;
                    break;
                case RoomType.Content:
                    if (_outputDoors.Length != 0) return false;
                    break;
                default:
                    return false;
            }

            Vector2Int Normalize(UnityEngine.Vector2Int cell) => new Vector2Int(cell.x - _bounds.xMin, cell.y - _bounds.yMin);
            var outputs = Array.ConvertAll(_outputDoors, Normalize);
            config = new RoomGenerationConfig(_id, _roomType, new Vector2Int(_bounds.width, _bounds.height),
                Normalize(_inputDoor), new Vector2Int(_bounds.xMin, _bounds.yMin),
                new RoomConfigVariant(_id, _assetReference, outputs));
            return true;
        }

        private bool TryAddDoor(UnityEngine.Vector2Int first, HashSet<UnityEngine.Vector2Int> occupied)
        {
            if (!_bounds.Contains(first)) return false;
            UnityEngine.Vector2Int second;
            if (first.x == _bounds.xMin || first.x == _bounds.xMax - 1)
            {
                if (first.y <= _bounds.yMin || first.y + 1 >= _bounds.yMax - 1) return false;
                second = first + UnityEngine.Vector2Int.up;
            }
            else if (first.y == _bounds.yMin || first.y == _bounds.yMax - 1)
            {
                if (first.x <= _bounds.xMin || first.x + 1 >= _bounds.xMax - 1) return false;
                second = first + UnityEngine.Vector2Int.right;
            }
            else return false;
            return occupied.Add(first) && occupied.Add(second);
        }
    }
}
