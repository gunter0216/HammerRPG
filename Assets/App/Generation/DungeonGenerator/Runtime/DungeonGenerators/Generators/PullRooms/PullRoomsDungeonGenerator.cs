using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

public class PullRoomsDungeonGenerator : IDungeonGenerator
{
    private List<DungeonGenerationRoom> _startEndPath;
    private List<WeightRoomPair> _spanningTree;

    public Optional<DungeonGeneration> Process(DungeonGeneration generation)
    {
        var rooms = generation.DungeonGenerationResult.GenerationData.GenerationRooms.Rooms;

        _spanningTree = generation.GetCash<SpanningTreeGenerationCash>().Value.Tree;
        _startEndPath = generation.GetCash<StartEndPathGenerationCash>().Value.Path;
        
        PullRooms(rooms);

        return Optional<DungeonGeneration>.Success(generation);
    }

    private void PullRooms(List<DungeonGenerationRoom> rooms)
    {
        if (rooms == null || rooms.Count == 0)
            return;

        var placed = new HashSet<DungeonGenerationRoom>();

        // 1. Ставим стартовую комнату
        var root = _startEndPath.Count > 0 ? _startEndPath[0] : rooms[0];
        root.Position = Vector2Int.Zero;
        placed.Add(root);

        // 2. Строим adjacency из spanningTree
        var adjacency = BuildAdjacency();

        var queue = new Queue<DungeonGenerationRoom>();
        queue.Enqueue(root);

        // 3. BFS
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (!adjacency.TryGetValue(current, out var neighbors))
                continue;

            foreach (var next in neighbors)
            {
                if (placed.Contains(next))
                    continue;

                var side = GetConnectionSide(current, next);

                var pos = PlaceAdjacent(current, next, side, placed);

                next.Position = pos;

                placed.Add(next);
                queue.Enqueue(next);
            }
        }
    }

    // =========================
    // 🔧 CORE LOGIC
    // =========================

    private Vector2Int PlaceAdjacent(
        DungeonGenerationRoom from,
        DungeonGenerationRoom to,
        RoomConnectSide side,
        HashSet<DungeonGenerationRoom> placed)
    {
        var basePos = GetBasePosition(from, to, side);

        // если не пересекается — отлично
        if (!IsOverlappingAny(to, basePos, placed))
            return basePos;

        // иначе двигаем вдоль стороны
        return SlideAlongEdge(from, to, side, placed);
    }

    private Vector2Int SlideAlongEdge(
        DungeonGenerationRoom from,
        DungeonGenerationRoom to,
        RoomConnectSide side,
        HashSet<DungeonGenerationRoom> placed)
    {
        int maxOffset = 50;

        for (int offset = 1; offset < maxOffset; offset++)
        {
            foreach (var dir in GetSlideDirections(side))
            {
                var pos = GetBasePosition(from, to, side) + dir * offset;

                if (!IsOverlappingAny(to, pos, placed))
                    return pos;
            }
        }

        // fallback (не должно происходить)
        return GetBasePosition(from, to, side);
    }

    private IEnumerable<Vector2Int> GetSlideDirections(RoomConnectSide side)
    {
        switch (side)
        {
            case RoomConnectSide.Top:
            case RoomConnectSide.Bottom:
                yield return Vector2Int.Left;
                yield return Vector2Int.Right;
                break;

            case RoomConnectSide.Left:
            case RoomConnectSide.Right:
                yield return Vector2Int.Top;
                yield return Vector2Int.Bottom;
                break;
        }
    }

    private Vector2Int GetBasePosition(
        DungeonGenerationRoom from,
        DungeonGenerationRoom to,
        RoomConnectSide side)
    {
        var fromPos = from.Position;
        var fromSize = from.Size;
        var toSize = to.Size;

        switch (side)
        {
            case RoomConnectSide.Top:
                return new Vector2Int(
                    AlignCenter(fromPos.X, fromSize.X, toSize.X),
                    fromPos.Y + fromSize.Y
                );

            case RoomConnectSide.Bottom:
                return new Vector2Int(
                    AlignCenter(fromPos.X, fromSize.X, toSize.X),
                    fromPos.Y - toSize.Y
                );

            case RoomConnectSide.Left:
                return new Vector2Int(
                    fromPos.X - toSize.X,
                    AlignCenter(fromPos.Y, fromSize.Y, toSize.Y)
                );

            case RoomConnectSide.Right:
                return new Vector2Int(
                    fromPos.X + fromSize.X,
                    AlignCenter(fromPos.Y, fromSize.Y, toSize.Y)
                );
        }

        return fromPos;
    }

    private int AlignCenter(int fromPos, int fromSize, int toSize)
    {
        int center = fromPos + fromSize / 2;
        return center - toSize / 2;
    }

    // =========================
    // 🔍 OVERLAP
    // =========================

    private bool IsOverlappingAny(
        DungeonGenerationRoom room,
        Vector2Int pos,
        HashSet<DungeonGenerationRoom> placed)
    {
        foreach (var other in placed)
        {
            if (IsOverlapping(pos, room.Size, other.Position, other.Size))
                return true;
        }

        return false;
    }

    private bool IsOverlapping(
        Vector2Int aPos, Vector2Int aSize,
        Vector2Int bPos, Vector2Int bSize)
    {
        return !(
            aPos.X + aSize.X <= bPos.X ||
            bPos.X + bSize.X <= aPos.X ||
            aPos.Y + aSize.Y <= bPos.Y ||
            bPos.Y + bSize.Y <= aPos.Y
        );
    }

    // =========================
    // 🔗 GRAPH
    // =========================

    private Dictionary<DungeonGenerationRoom, List<DungeonGenerationRoom>> BuildAdjacency()
    {
        var dict = new Dictionary<DungeonGenerationRoom, List<DungeonGenerationRoom>>();

        foreach (var edge in _spanningTree)
        {
            if (!dict.ContainsKey(edge.Room1))
                dict[edge.Room1] = new List<DungeonGenerationRoom>();

            if (!dict.ContainsKey(edge.Room2))
                dict[edge.Room2] = new List<DungeonGenerationRoom>();

            dict[edge.Room1].Add(edge.Room2);
            dict[edge.Room2].Add(edge.Room1);
        }

        return dict;
    }

    private RoomConnectSide GetConnectionSide(
        DungeonGenerationRoom from,
        DungeonGenerationRoom to)
    {
        foreach (var c in from.Connections)
        {
            if (c.GenerationRoom == to)
                return c.Side;
        }

        return RoomConnectSide.Right;
    }

    public string GetName()
    {
        return "Pull Rooms";
    }
}