using System;
using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsSeparator.Config;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using Random = System.Random;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsSeparator
{
    public class SeparateRoomsDungeonGenerator : IDungeonGenerator
    {
        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.Dungeon;
            if (!generation.TryGetConfig<SeparateRoomsGenerationConfig>(out var config))
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            const int stupidCount = 10000;
            for (int i = 0; i < stupidCount; ++i)
            {
                // SeparateRectanglesWithGrid(dungeon);
                // break;

                if (Separate(dungeon, config))
                {
                   break;
                }
            }

            return Optional<DungeonGeneration>.Success(generation);
        }

        public bool Separate(Dungeon dungeon, SeparateRoomsGenerationConfig config)
        {
            int maxSpeed = config.Speed;

            // Wether all rooms are not overlapping anymore
            bool isEveryRoomSeperated = true;

            // For each room
            foreach (var room in dungeon.Data.RoomsData.Rooms)
            {
                // The total direction to move in
                Vector2 seperationDirection = new Vector2();
                // var seperationDirection = new Vector2Int();

                foreach (var otherRoom in dungeon.Data.RoomsData.Rooms)
                {
                    // If the other room is the same as the current room, continue to the next room
                    if (ReferenceEquals(room, otherRoom))
                    {
                        continue;
                    }

                    // If the rooms are not overlapping, continue to the next room
                    if (!room.IsOverlapping(otherRoom))
                    {
                        continue;
                    }

                    var direction = new Vector2Int();

                    // right
                    if (room.Left < otherRoom.Right && room.Left > otherRoom.Left)
                    {
                        direction.X = 1;
                    } // left
                    else if (room.Right > otherRoom.Left && room.Right < otherRoom.Right)
                    {
                        direction.X = -1;
                    }

                    // down
                    if (room.Top < otherRoom.Top && room.Top > otherRoom.Bottom)
                    {
                        direction.Y = -1;
                    }
                    else if (room.Bottom > otherRoom.Bottom && room.Bottom < otherRoom.Top)
                    {
                        direction.Y = 1;
                    }
                    // Makes sure false gets returned, which will repeat the seperation

                    // Calculate the direction between the rooms
                    // Vector2 curDirection = new Vector2(room.Position + room.Size / 2 - (otherRoom.Position + otherRoom.Size / 2));
                    // var curDirection = room.GetCenter() - otherRoom.GetCenter();
                    // Normalize the direction
                    // curDirection.Normalize();

                    if (direction.X == 0 && direction.Y == 0)
                    {
                        continue;
                    }

                    isEveryRoomSeperated = false;

                    // curDirection.x = curDirection.x > 0 ? 1 : 0;
                    // curDirection.x = curDirection.x < 0 ? -1 : 0;
                    //
                    // curDirection.y = curDirection.y > 0 ? 1 : 0;
                    // curDirection.y = curDirection.y < 0 ? -1 : 0;

                    // Set the direction to max speed
                    direction *= maxSpeed;

                    // Add the current direction to the total direction
                    seperationDirection += direction;
                }

                // Debug.LogError($"seperationDirection {seperationDirection}");

                // Move the room to the calculated seperation direction
                room.Position += new Vector2Int((int)seperationDirection.X, (int)seperationDirection.Y);
            }

            return isEveryRoomSeperated;
        }

        /// <summary>
        /// Равномерно раздвигает прямоугольники, чтобы они не пересекались
        /// Использует float для вычислений, но результат округляет до int
        /// </summary>
        /// <param name="rectangles">Список прямоугольников для разделения</param>
        /// <param name="maxIterations">Максимальное количество итераций</param>
        /// <param name="separationForce">Сила разделения (чем больше, тем быстрее разделение)</param>
        /// <param name="minDistance">Минимальное расстояние между прямоугольниками</param>
        public void SeparateRectangles(Dungeon dungeon, int maxIterations = 5000, float separationForce = 0.2f,
            int minDistance = 1)
        {
            var rectangles = dungeon.Data.RoomsData.Rooms;
            if (rectangles == null || rectangles.Count < 2)
                return;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                bool hasIntersections = false;
                var movements = new Vector2[rectangles.Count];

                // Проверяем все пары прямоугольников
                for (int i = 0; i < rectangles.Count; i++)
                {
                    for (int j = i + 1; j < rectangles.Count; j++)
                    {
                        var rect1 = rectangles[i];
                        var rect2 = rectangles[j];

                        if (rect1.Intersects(rect2) || GetDistance(rect1, rect2) < minDistance)
                        {
                            hasIntersections = true;

                            // Вычисляем вектор разделения с учетом размеров прямоугольников
                            var separationVector =
                                CalculateSeparationVector(rect1, rect2, minDistance, separationForce);

                            movements[i] = new Vector2(movements[i].X + separationVector.X,
                                movements[i].Y + separationVector.Y);
                            movements[j] = new Vector2(movements[j].X - separationVector.X,
                                movements[j].Y - separationVector.Y);
                        }
                    }
                }

                // Применяем движения с округлением
                for (int i = 0; i < rectangles.Count; i++)
                {
                    if (Math.Abs(movements[i].X) > 0.5f || Math.Abs(movements[i].Y) > 0.5f)
                    {
                        var center = rectangles[i].GetCenter();
                        center = new Vector2(center.X + movements[i].X, center.Y + movements[i].Y);
                        rectangles[i].SetCenter(center);
                    }
                }

                // Если пересечений нет, завершаем
                if (!hasIntersections)
                    break;
            }
        }

        /// <summary>
        /// Вычисляет вектор разделения для двух прямоугольников
        /// </summary>
        private static Vector2 CalculateSeparationVector(DungeonRoomData rect1, DungeonRoomData rect2, int minDistance,
            float separationForce)
        {
            var center1 = rect1.GetCenter();
            var center2 = rect2.GetCenter();

            // Вычисляем перекрытие по каждой оси
            int overlapX = GetOverlapX(rect1, rect2);
            int overlapY = GetOverlapY(rect1, rect2);

            Vector2 separationDirection;
            float separationMagnitude;

            if (overlapX > 0 && overlapY > 0)
            {
                // Прямоугольники пересекаются - выбираем ось с меньшим перекрытием
                if (overlapX <= overlapY)
                {
                    // Разделяем по X
                    separationDirection = center1.X < center2.X ? new Vector2(-1, 0) : new Vector2(1, 0);
                    separationMagnitude = (overlapX * 0.5f + minDistance) * separationForce;
                }
                else
                {
                    // Разделяем по Y
                    separationDirection = center1.Y < center2.Y ? new Vector2(0, -1) : new Vector2(0, 1);
                    separationMagnitude = (overlapY * 0.5f + minDistance) * separationForce;
                }
            }
            else
            {
                // Прямоугольники не пересекаются, но слишком близко
                var direction = new Vector2(center1.X - center2.X, center1.Y - center2.Y);
                float distance = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

                if (distance > 0.001f)
                {
                    separationDirection = new Vector2(direction.X / distance, direction.Y / distance);
                }
                else
                {
                    // Если центры совпадают, выбираем случайное направление
                    var random = new Random();
                    float angle = (float)(random.NextDouble() * Math.PI * 2);
                    separationDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                }

                float requiredDistance = GetRequiredSeparationDistance(rect1, rect2, separationDirection) + minDistance;
                separationMagnitude = Math.Max(0, requiredDistance - distance) * 0.5f * separationForce;
            }

            return new Vector2(
                separationDirection.X * separationMagnitude,
                separationDirection.Y * separationMagnitude
            );
        }

        /// <summary>
        /// Вычисляет перекрытие по оси X
        /// </summary>
        private static int GetOverlapX(DungeonRoomData rect1, DungeonRoomData rect2)
        {
            int left1 = rect1.Position.X;
            int right1 = rect1.Position.X + rect1.Width;
            int left2 = rect2.Position.X;
            int right2 = rect2.Position.X + rect2.Width;

            int overlapStart = Math.Max(left1, left2);
            int overlapEnd = Math.Min(right1, right2);

            return Math.Max(0, overlapEnd - overlapStart);
        }

        /// <summary>
        /// Вычисляет перекрытие по оси Y
        /// </summary>
        private static int GetOverlapY(DungeonRoomData rect1, DungeonRoomData rect2)
        {
            int bottom1 = rect1.Position.Y;
            int top1 = rect1.Position.Y + rect1.Height;
            int bottom2 = rect2.Position.Y;
            int top2 = rect2.Position.Y + rect2.Height;

            int overlapStart = Math.Max(bottom1, bottom2);
            int overlapEnd = Math.Min(top1, top2);

            return Math.Max(0, overlapEnd - overlapStart);
        }

        /// <summary>
        /// Вычисляет необходимое расстояние для разделения в заданном направлении
        /// </summary>
        private static float GetRequiredSeparationDistance(DungeonRoomData rect1, DungeonRoomData rect2,
            Vector2 direction)
        {
            // Проецируем половинные размеры прямоугольников на направление разделения
            float projection1 = Math.Abs(direction.X * rect1.Width * 0.5f) +
                                Math.Abs(direction.Y * rect1.Height * 0.5f);
            float projection2 = Math.Abs(direction.X * rect2.Width * 0.5f) +
                                Math.Abs(direction.Y * rect2.Height * 0.5f);

            return projection1 + projection2;
        }

        /// <summary>
        /// Сеточный метод разделения - оптимальный для integer координат
        /// </summary>
        /// <param name="rectangles">Список прямоугольников</param>
        /// <param name="gridSize">Размер ячейки сетки</param>
        /// <param name="padding">Отступ между прямоугольниками</param>
        public void SeparateRectanglesWithGrid(Dungeon dungeon, int gridSize = 1, int padding = 1)
        {
            var rectangles = dungeon.Data.RoomsData.Rooms;
            if (rectangles == null || rectangles.Count == 0)
                return;

            // Сортируем прямоугольники по площади (большие первыми)
            rectangles.Sort((a, b) => b.GetArea().CompareTo(a.GetArea()));

            var occupiedCells = new HashSet<(int, int)>();

            foreach (var rectangle in rectangles)
            {
                var bestPosition = FindBestGridPosition(rectangle, occupiedCells, gridSize, padding);
                rectangle.Position = new Vector2Int(bestPosition.x * gridSize, bestPosition.y * gridSize);

                // Отмечаем занятые ячейки с учетом padding
                int cellsWidth = (int)Math.Ceiling((double)(rectangle.Width + padding * 2) / gridSize);
                int cellsHeight = (int)Math.Ceiling((double)(rectangle.Height + padding * 2) / gridSize);

                for (int x = bestPosition.x; x < bestPosition.x + cellsWidth; x++)
                {
                    for (int y = bestPosition.y; y < bestPosition.y + cellsHeight; y++)
                    {
                        occupiedCells.Add((x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Простой алгоритм разделения только по целым координатам
        /// </summary>
        public static void SeparateRectanglesInteger(List<DungeonRoomData> rectangles, int maxIterations = 50,
            int minDistance = 1)
        {
            if (rectangles == null || rectangles.Count < 2)
                return;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                bool hasIntersections = false;

                for (int i = 0; i < rectangles.Count; i++)
                {
                    for (int j = i + 1; j < rectangles.Count; j++)
                    {
                        var rect1 = rectangles[i];
                        var rect2 = rectangles[j];

                        if (rect1.Intersects(rect2))
                        {
                            hasIntersections = true;

                            // Определяем направление разделения по минимальному перекрытию
                            int overlapX = GetOverlapX(rect1, rect2);
                            int overlapY = GetOverlapY(rect1, rect2);

                            if (overlapX <= overlapY)
                            {
                                // Разделяем по X
                                int moveDistance = (overlapX + 1) / 2 + minDistance;
                                if (rect1.GetCenter().X < rect2.GetCenter().X)
                                {
                                    rect1.Position = new Vector2Int(rect1.Position.X - moveDistance, rect1.Position.Y);
                                    rect2.Position = new Vector2Int(rect2.Position.X + moveDistance, rect2.Position.Y);
                                }
                                else
                                {
                                    rect1.Position = new Vector2Int(rect1.Position.X + moveDistance, rect1.Position.Y);
                                    rect2.Position = new Vector2Int(rect2.Position.X - moveDistance, rect2.Position.Y);
                                }
                            }
                            else
                            {
                                // Разделяем по Y
                                int moveDistance = (overlapY + 1) / 2 + minDistance;
                                if (rect1.GetCenter().Y < rect2.GetCenter().Y)
                                {
                                    rect1.Position = new Vector2Int(rect1.Position.X, rect1.Position.Y - moveDistance);
                                    rect2.Position = new Vector2Int(rect2.Position.X, rect2.Position.Y + moveDistance);
                                }
                                else
                                {
                                    rect1.Position = new Vector2Int(rect1.Position.X, rect1.Position.Y + moveDistance);
                                    rect2.Position = new Vector2Int(rect2.Position.X, rect2.Position.Y - moveDistance);
                                }
                            }
                        }
                    }
                }

                if (!hasIntersections)
                    break;
            }
        }

        private static (int x, int y) FindBestGridPosition(DungeonRoomData rectangle, HashSet<(int, int)> occupiedCells,
            int gridSize, int padding)
        {
            int cellsWidth = (int)Math.Ceiling((double)(rectangle.Width + padding * 2) / gridSize);
            int cellsHeight = (int)Math.Ceiling((double)(rectangle.Height + padding * 2) / gridSize);

            var originalCenter = rectangle.GetCenter();
            int startX = (int)Math.Round(originalCenter.X / gridSize);
            int startY = (int)Math.Round(originalCenter.Y / gridSize);

            // Поиск по спирали от исходной позиции
            for (int radius = 0; radius < 100; radius++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (Math.Abs(dx) != radius && Math.Abs(dy) != radius)
                            continue;

                        int x = startX + dx;
                        int y = startY + dy;

                        if (IsGridPositionFree(x, y, cellsWidth, cellsHeight, occupiedCells))
                        {
                            return (x, y);
                        }
                    }
                }
            }

            return (startX, startY);
        }

        private static bool IsGridPositionFree(int x, int y, int cellsWidth, int cellsHeight,
            HashSet<(int, int)> occupiedCells)
        {
            for (int dx = 0; dx < cellsWidth; dx++)
            {
                for (int dy = 0; dy < cellsHeight; dy++)
                {
                    if (occupiedCells.Contains((x + dx, y + dy)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static float GetDistance(DungeonRoomData rect1, DungeonRoomData rect2)
        {
            var center1 = rect1.GetCenter();
            var center2 = rect2.GetCenter();
            float dx = center1.X - center2.X;
            float dy = center1.Y - center2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public string GetName()
        {
            return "Separate Rooms";
        }
    }
}