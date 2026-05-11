using System;
using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generators.PullRooms
{
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

            var root = _startEndPath.Count > 0 ? _startEndPath[0] : rooms[0];
            root.Position = Vector2Int.Zero;
            placed.Add(root);

            var adjacency = BuildAdjacency();

            var queue = new Queue<DungeonGenerationRoom>();
            queue.Enqueue(root);

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
        // CORE
        // =========================

        private Vector2Int PlaceAdjacent(
            DungeonGenerationRoom from,
            DungeonGenerationRoom to,
            RoomConnectSide side,
            HashSet<DungeonGenerationRoom> placed)
        {
            var candidates = GenerateCandidates(from, to, side);

            Vector2Int bestPos = GetFallbackPosition(from, to, side);
            int bestScore = int.MinValue;

            foreach (var pos in candidates)
            {
                if (IsOverlappingAny(to, pos, placed))
                    continue;

                int score = ScorePosition(to, pos, placed);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPos = pos;
                }
            }

            return bestPos;
        }

        private IEnumerable<Vector2Int> GenerateCandidates(
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
                {
                    int y = fromPos.Y + fromSize.Y;
                    int minX = fromPos.X - toSize.X + 1;
                    int maxX = fromPos.X + fromSize.X - 1;

                    for (int x = minX; x <= maxX; x++)
                        yield return new Vector2Int(x, y);

                    break;
                }

                case RoomConnectSide.Bottom:
                {
                    int y = fromPos.Y - toSize.Y;
                    int minX = fromPos.X - toSize.X + 1;
                    int maxX = fromPos.X + fromSize.X - 1;

                    for (int x = minX; x <= maxX; x++)
                        yield return new Vector2Int(x, y);

                    break;
                }

                case RoomConnectSide.Left:
                {
                    int x = fromPos.X - toSize.X;
                    int minY = fromPos.Y - toSize.Y + 1;
                    int maxY = fromPos.Y + fromSize.Y - 1;

                    for (int y = minY; y <= maxY; y++)
                        yield return new Vector2Int(x, y);

                    break;
                }

                case RoomConnectSide.Right:
                {
                    int x = fromPos.X + fromSize.X;
                    int minY = fromPos.Y - toSize.Y + 1;
                    int maxY = fromPos.Y + fromSize.Y - 1;

                    for (int y = minY; y <= maxY; y++)
                        yield return new Vector2Int(x, y);

                    break;
                }
            }
        }

        private int ScorePosition(
            DungeonGenerationRoom room,
            Vector2Int pos,
            HashSet<DungeonGenerationRoom> placed)
        {
            int score = 0;

            foreach (var other in placed)
            {
                if (!AreConnected(room, other))
                    continue;

                if (AreTouchingSide(pos, room.Size, other.Position, other.Size))
                    score += 1000;

                score += GetAxisOverlap(pos, room.Size, other.Position, other.Size);
            }

            return score;
        }

        private Vector2Int GetFallbackPosition(
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
                    return new Vector2Int(fromPos.X, fromPos.Y + fromSize.Y);

                case RoomConnectSide.Bottom:
                    return new Vector2Int(fromPos.X, fromPos.Y - toSize.Y);

                case RoomConnectSide.Left:
                    return new Vector2Int(fromPos.X - toSize.X, fromPos.Y);

                case RoomConnectSide.Right:
                    return new Vector2Int(fromPos.X + fromSize.X, fromPos.Y);
            }

            return fromPos;
        }

        // =========================
        // OVERLAP
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

        private bool AreTouchingSide(
            Vector2Int aPos, Vector2Int aSize,
            Vector2Int bPos, Vector2Int bSize)
        {
            bool xOverlap = aPos.X < bPos.X + bSize.X &&
                            aPos.X + aSize.X > bPos.X;

            bool yOverlap = aPos.Y < bPos.Y + bSize.Y &&
                            aPos.Y + aSize.Y > bPos.Y;

            bool touchingX = aPos.X + aSize.X == bPos.X ||
                             bPos.X + bSize.X == aPos.X;

            bool touchingY = aPos.Y + aSize.Y == bPos.Y ||
                             bPos.Y + bSize.Y == aPos.Y;

            return (touchingX && yOverlap) || (touchingY && xOverlap);
        }

        private int GetAxisOverlap(
            Vector2Int aPos, Vector2Int aSize,
            Vector2Int bPos, Vector2Int bSize)
        {
            int overlapX = Math.Min(aPos.X + aSize.X, bPos.X + bSize.X) - Math.Max(aPos.X, bPos.X);
            int overlapY = Math.Min(aPos.Y + aSize.Y, bPos.Y + bSize.Y) - Math.Max(aPos.Y, bPos.Y);

            return Math.Max(0, overlapX) + Math.Max(0, overlapY);
        }

        // =========================
        // GRAPH
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

        private bool AreConnected(DungeonGenerationRoom a, DungeonGenerationRoom b)
        {
            foreach (var c in a.Connections)
            {
                if (c.GenerationRoom == b)
                    return true;
            }

            return false;
        }

        public string GetName()
        {
            return "Pull Rooms";
        }
    }
}