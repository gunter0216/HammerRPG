using System;
using System.Collections.Generic;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
{
    public class CreateCorridorsDungeonGenerator : IDungeonGenerator
    {
        private const int m_MinRoomSize = 7;
        private readonly RoomCreator m_RoomCreator;

        public CreateCorridorsDungeonGenerator(RoomCreator roomCreator)
        {
            m_RoomCreator = roomCreator;
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            if (!generation.TryGetCash<SpanningTreeGenerationCash>(out var cash))
            {
                return Optional<DungeonGeneration>.Fail();
            }

            CreateCorridors(generation.Dungeon, cash);
            // CreateRooms(generation.Dungeon, cash);

            return Optional<DungeonGeneration>.Success(generation);
        }

        private void CreateCorridors(Dungeon dungeon, SpanningTreeGenerationCash spanningTree)
        {
            foreach (var edge in spanningTree.Tree)
            {
                var room = CreateCorridor(edge.Room1, edge.Room2);
                
                dungeon.Data.RoomsData.Rooms.Add(room);
            }
        }

        private DungeonRoomData CreateCorridor(DungeonRoomData room1, DungeonRoomData room2)
        {
            var center1 = room1.GetCenter();
            var center2 = room2.GetCenter();

            float deltaX = Math.Abs(center2.X - center1.X);
            float deltaY = Math.Abs(center2.Y - center1.Y);

            bool isHorizontal = deltaX > deltaY;

            DungeonRoomData newRoom;
            if (isHorizontal)
            {
                newRoom = CreateHorizontal(room1, room2);
            }
            else
            {
                newRoom = CreateVertical(room1, room2);
            }

            return newRoom;
            //
            // const int minSize = 20;
            //
            // foreach (var edge in spanningTree.Tree)
            // {
            //     // Get the two rooms connected to this edge
            //
            //     // If the x and y directions go from room 0 to 1 or not
            //     bool directionX01 = false;
            //     bool directionY01 = false;
            //
            //     // The bottom left corner of the new corridor
            //     var bottomLeft = new Vector2();
            //
            //     // Set the x direction and the x component of the leftbottom corner
            //     if (room0Pos.X < room1Pos.X)
            //     {
            //         bottomLeft.X = room0Pos.X;
            //         directionX01 = true;
            //     }
            //     else
            //     {
            //         bottomLeft.X = room1Pos.X;
            //     }
            //
            //     // Set the y direction and the y component of the leftbottom corner
            //     if (room0Pos.Y < room1Pos.Y)
            //     {
            //         bottomLeft.Y = room0Pos.Y;
            //         directionY01 = true;
            //     }
            //     else
            //     {
            //         bottomLeft.Y = room1Pos.Y;
            //     }
            //
            //     // The size of the corridor
            //     var size = new Vector2(Math.Abs(room0Pos.X - room1Pos.X), Math.Abs(room0Pos.Y - room1Pos.Y));
            //
            //     // If the size of the corridor is smaller then the minimum allowed size, scale and move the corridor
            //     if (size.X < minSize)
            //     {
            //         var sizeGrow = minSize - size.X;
            //         size.X = minSize;
            //         bottomLeft.X -= sizeGrow / 2;
            //     }
            //
            //     if (size.Y < minSize)
            //     {
            //         var sizeGrow = minSize - size.Y;
            //         size.Y = minSize;
            //         bottomLeft.Y -= sizeGrow / 2;
            //     }
            //
            //     // The displacements at the left, top, right and bottom
            //     int leftDisplacement = 0;
            //     int rightDisplacement = 0;
            //     int bottomDisplacement = 0;
            //     int topDisplacement = 0;
            //
            //     // Is this room a vertical or horizontal corridor
            //     var isHorizontalCorridor = size.X > size.Y;
            //
            //     // Depending on a horizontal or vertical corridor, calculate the displacement amounts
            //     if (isHorizontalCorridor)
            //     {
            //         if (directionX01)
            //         {
            //             leftDisplacement = (int)((room0Pos + room0.Size.ToVector() / 2).X - bottomLeft.X);
            //             rightDisplacement = (int)(bottomLeft.X + size.X - (room1Pos - room1.Size.ToVector() / 2).X);
            //         }
            //         else
            //         {
            //             leftDisplacement = (int)((room1Pos + room1.Size.ToVector() / 2).X - bottomLeft.X);
            //             rightDisplacement = (int)(bottomLeft.X + size.X - (room0Pos - room1.Size.ToVector() / 2).X);
            //         }
            //     }
            //     else
            //     {
            //         if (directionY01)
            //         {
            //             bottomDisplacement = (int)((room0Pos + room0.Size.ToVector() / 2).Y - bottomLeft.Y);
            //             topDisplacement = (int)(bottomLeft.Y + size.Y - (room1Pos - room1.Size.ToVector() / 2).Y);
            //         }
            //         else
            //         {
            //             bottomDisplacement = (int)((room1Pos + room1.Size.ToVector() / 2).Y - bottomLeft.Y);
            //             topDisplacement = (int)(bottomLeft.Y + size.Y - (room0Pos - room0.Size.ToVector() / 2).Y);
            //         }
            //     }
            //
            //     // Move and scale the corridor according to the calculated displacements
            //     bottomLeft.X += leftDisplacement;
            //     size.X -= leftDisplacement;
            //     size.X -= rightDisplacement;
            //     bottomLeft.Y += bottomDisplacement;
            //     size.Y -= bottomDisplacement;
            //     size.Y -= topDisplacement;
            //     
            //     if (size.X <= 0 || size.Y <= 0) continue;
            //     
            //     var room = m_RoomCreator.Create(
            //         new Vector2Int((int)bottomLeft.X, (int)bottomLeft.Y),
            //         new Vector2Int((int)size.X, (int)size.Y)
            //     );
            //     
            //     dungeon.Data.RoomsData.Rooms.Add(room);
            // }
        }

        private DungeonRoomData CreateHorizontal(DungeonRoomData room1, DungeonRoomData room2)
        {
            var leftRoom = room1.Center.X < room2.Center.X ? room1 : room2;
            var rightRoom = room1.Center.X < room2.Center.X ? room2 : room1;

            var left = leftRoom.Right;
            var right = rightRoom.Left;
            var top = room1.Top < room2.Top ? room1.Top : room2.Top;
            var bottom = room1.Bottom > room2.Bottom ? room1.Bottom : room2.Bottom;

            var height = top - bottom;
            if (height < 0)
            {
                bottom += height;
                top -= height;

                bottom -= m_MinRoomSize / 2 + m_MinRoomSize % 2;
                top += m_MinRoomSize / 2 + m_MinRoomSize % 2;
            }
            
            height = top - bottom;
            if (height < m_MinRoomSize)
            {
                var diff = m_MinRoomSize - height;
                top += diff;
                bottom -= diff;
            }

            var position = new Vector2Int(left, bottom);
            var size = new Vector2Int(right - left, top - bottom);
            
            return m_RoomCreator.Create(position, size);
        }

        private DungeonRoomData CreateVertical(DungeonRoomData room1, DungeonRoomData room2)
        {
            var bottomRoom = room1.Center.Y < room2.Center.Y ? room1 : room2;
            var topRoom = room1.Center.Y < room2.Center.Y ? room2 : room1;
            
            var top = bottomRoom.Top;
            var bottom = topRoom.Bottom;
            var left = room1.Left > room2.Left ? room1.Left : room2.Left;
            var right = room1.Right < room2.Right ? room1.Right : room2.Right;

            var width = right - left;
            if (width < 0)
            {
                left += width;
                right -= width;

                left -= m_MinRoomSize / 2 + m_MinRoomSize % 2;
                right += m_MinRoomSize / 2 + m_MinRoomSize % 2;
            }
            
            width = right - left;
            if (width < m_MinRoomSize)
            {
                var diff = m_MinRoomSize - width;
                right += diff;
                left -= diff;
            }
            
            var position = new Vector2Int(left, bottom);
            var size = new Vector2Int(right - left, top - bottom);
            
            return m_RoomCreator.Create(position, size);
            // return m_RoomCreator.Create(Vector2Int.Zero, Vector2Int.Zero);
        }
        /*
        private void CreateRooms(Dungeon dungeon, SpanningTreeGenerationCash spanningTree)
        {
            foreach (var edge in spanningTree.Tree)
            {
                var room0 = edge.Room1;
                var room1 = edge.Room2;

                var newRoom = CreateRoomBetween(room0, room1, new Vector2Int(3, 3));
                // var rooms = CreateLShapedConnection(room0, room1, new Vector2Int(3, 3));
                // foreach (var room in rooms)
                // {
                // dungeon.Data.RoomsData.Rooms.Add(room);
                // }
                dungeon.Data.RoomsData.Rooms.Add(newRoom);
            }

            return;
        }

        public DungeonRoomData CreateRoomBetween(DungeonRoomData room1, DungeonRoomData room2,
            Vector2Int corridorSize)
        {
            // Определяем центры комнат
            Vector2 center1 = room1.GetCenter();
            Vector2 center2 = room2.GetCenter();

            // Вычисляем расстояние между центрами
            float deltaX = Math.Abs(center2.X - center1.X);
            float deltaY = Math.Abs(center2.Y - center1.Y);

            // Определяем, какое направление преобладает
            bool isHorizontal = deltaX > deltaY;

            if (isHorizontal)
            {
                return CreateHorizontalConnection(room1, room2, corridorSize);
            }
            else
            {
                return CreateVerticalConnection(room1, room2, corridorSize);
            }
        }

        private DungeonRoomData CreateHorizontalConnection(DungeonRoomData room1, DungeonRoomData room2,
            Vector2Int corridorSize)
        {
            // Определяем левую и правую комнаты
            DungeonRoomData leftRoom = room1.GetCenter().X < room2.GetCenter().X ? room1 : room2;
            DungeonRoomData rightRoom = room1.GetCenter().X < room2.GetCenter().X ? room2 : room1;

            // Позиция по X - от правой стенки левой комнаты до левой стенки правой комнаты
            int newLeft = leftRoom.Right;
            int newRight = rightRoom.Left;
            int newWidth = Math.Max(corridorSize.X, newRight - newLeft);

            // Если комнаты перекрываются по X, создаем коридор минимальной ширины
            if (newRight <= newLeft)
            {
                newLeft = Math.Min(leftRoom.Right, rightRoom.Left);
                newWidth = corridorSize.X;
            }

            // Позиция по Y - находим пересечение комнат по вертикали
            int overlapBottom = Math.Max(leftRoom.Bottom, rightRoom.Bottom);
            int overlapTop = Math.Min(leftRoom.Top, rightRoom.Top);

            int newBottom, newHeight;

            if (overlapTop > overlapBottom)
            {
                // Комнаты пересекаются по Y - используем область пересечения
                newBottom = overlapBottom;
                newHeight = Math.Max(corridorSize.Y, overlapTop - overlapBottom);
            }
            else
            {
                // Комнаты не пересекаются по Y - создаем коридор между ними
                int centerY = (int)Math.Round((leftRoom.GetCenter().Y + rightRoom.GetCenter().Y) / 2.0f);
                newBottom = centerY - corridorSize.Y / 2;
                newHeight = corridorSize.Y;

                // Проверяем, не выходит ли коридор за границы комнат
                int minY = Math.Min(leftRoom.Bottom, rightRoom.Bottom);
                int maxY = Math.Max(leftRoom.Top, rightRoom.Top);

                // Корректируем позицию, чтобы коридор не выходил за разумные границы
                if (newBottom < minY - corridorSize.Y)
                {
                    newBottom = minY - corridorSize.Y / 2;
                }

                if (newBottom + newHeight > maxY + corridorSize.Y)
                {
                    newBottom = maxY - corridorSize.Y / 2;
                }
            }

            return m_RoomCreator.Create(new Vector2Int(newLeft, newBottom), new Vector2Int(newWidth, newHeight));
        }

        private DungeonRoomData CreateVerticalConnection(DungeonRoomData room1, DungeonRoomData room2,
            Vector2Int corridorSize)
        {
            // Определяем нижнюю и верхнюю комнаты
            DungeonRoomData bottomRoom = room1.GetCenter().Y < room2.GetCenter().Y ? room1 : room2;
            DungeonRoomData topRoom = room1.GetCenter().Y < room2.GetCenter().Y ? room2 : room1;

            // Позиция по Y - от верхней стенки нижней комнаты до нижней стенки верхней комнаты
            int newBottom = bottomRoom.Top;
            int newTop = topRoom.Bottom;
            int newHeight = Math.Max(corridorSize.Y, newTop - newBottom);

            // Если комнаты перекрываются по Y, создаем коридор минимальной высоты
            if (newTop <= newBottom)
            {
                newBottom = Math.Min(bottomRoom.Top, topRoom.Bottom);
                newHeight = corridorSize.Y;
            }

            // Позиция по X - находим пересечение комнат по горизонтали
            int overlapLeft = Math.Max(bottomRoom.Left, topRoom.Left);
            int overlapRight = Math.Min(bottomRoom.Right, topRoom.Right);

            int newLeft, newWidth;

            if (overlapRight > overlapLeft)
            {
                // Комнаты пересекаются по X - используем область пересечения
                newLeft = overlapLeft;
                newWidth = Math.Max(corridorSize.X, overlapRight - overlapLeft);
            }
            else
            {
                // Комнаты не пересекаются по X - создаем коридор между ними
                int centerX = (int)Math.Round((bottomRoom.GetCenter().X + topRoom.GetCenter().X) / 2.0f);
                newLeft = centerX - corridorSize.X / 2;
                newWidth = corridorSize.X;

                // Проверяем, не выходит ли коридор за границы комнат
                int minX = Math.Min(bottomRoom.Left, topRoom.Left);
                int maxX = Math.Max(bottomRoom.Right, topRoom.Right);

                // Корректируем позицию, чтобы коридор не выходил за разумные границы
                if (newLeft < minX - corridorSize.X)
                {
                    newLeft = minX - corridorSize.X / 2;
                }

                if (newLeft + newWidth > maxX + corridorSize.X)
                {
                    newLeft = maxX - corridorSize.X / 2;
                }
            }

            return m_RoomCreator.Create(new Vector2Int(newLeft, newBottom), new Vector2Int(newWidth, newHeight));
        }

// Перегрузка с размером коридора по умолчанию
        public DungeonRoomData CreateRoomBetween(DungeonRoomData room1, DungeonRoomData room2)
        {
            return CreateRoomBetween(room1, room2, new Vector2Int(3, 3));
        }

// Дополнительная функция для создания L-образного соединения через две комнаты
        public DungeonRoomData[] CreateLShapedConnection(DungeonRoomData room1, DungeonRoomData room2,
            Vector2Int corridorSize)
        {
            Vector2 center1 = room1.GetCenter();
            Vector2 center2 = room2.GetCenter();

            // Создаем промежуточную точку для L-образного соединения
            Vector2Int intermediatePoint = new Vector2Int((int)center1.X, (int)center2.Y);

            // Создаем промежуточную комнату в углу L
            var cornerRoom = m_RoomCreator.Create(
                new Vector2Int(intermediatePoint.X - corridorSize.X / 2, intermediatePoint.Y - corridorSize.Y / 2),
                corridorSize
            );

            // Создаем два соединения: от первой комнаты к углу и от угла ко второй комнате
            DungeonRoomData connection1 = CreateRoomBetween(room1, cornerRoom, corridorSize);
            DungeonRoomData connection2 = CreateRoomBetween(cornerRoom, room2, corridorSize);

            return new DungeonRoomData[] { connection1, cornerRoom, connection2 };
        }


        public DungeonRoomData CreateRoomBetween(DungeonRoomData room1, DungeonRoomData room2, Vector2Int corridorSize)
        {
            // Определяем центры комнат
            Vector2 center1 = room1.GetCenter();
            Vector2 center2 = room2.GetCenter();

            // Вычисляем направление от первой комнаты ко второй
            Vector2 direction = new Vector2(center2.X - center1.X, center2.Y - center1.Y);

            // Определяем, какое направление преобладает (горизонтальное или вертикальное)
            bool isHorizontal = Math.Abs(direction.X) > Math.Abs(direction.Y);

            Vector2Int newPosition;
            Vector2Int newSize;

            if (isHorizontal)
            {
                // Горизонтальное соединение
                int leftBound, rightBound;

                if (room1.GetCenter().X < room2.GetCenter().X)
                {
                    // room1 левее room2
                    leftBound = room1.Right;
                    rightBound = room2.Left;
                }
                else
                {
                    // room2 левее room1
                    leftBound = room2.Right;
                    rightBound = room1.Left;
                }

                // Определяем вертикальные границы (пересечение по Y)
                int topBound = Math.Min(room1.Top, room2.Top);
                int bottomBound = Math.Max(room1.Bottom, room2.Bottom);

                // Если комнаты не пересекаются по вертикали, создаем коридор заданного размера
                if (bottomBound >= topBound)
                {
                    int centerY = (int)Math.Round((room1.GetCenter().Y + room2.GetCenter().Y) / 2.0f);
                    bottomBound = centerY - corridorSize.Y / 2;
                    topBound = bottomBound + corridorSize.Y;
                }

                newPosition = new Vector2Int(leftBound, bottomBound);
                newSize = new Vector2Int(rightBound - leftBound, topBound - bottomBound);
            }
            else
            {
                // Вертикальное соединение
                int bottomBound, topBound;

                if (room1.GetCenter().Y < room2.GetCenter().Y)
                {
                    // room1 ниже room2
                    bottomBound = room1.Top;
                    topBound = room2.Bottom;
                }
                else
                {
                    // room2 ниже room1
                    bottomBound = room2.Top;
                    topBound = room1.Bottom;
                }

                // Определяем горизонтальные границы (пересечение по X)
                int rightBound = Math.Min(room1.Right, room2.Right);
                int leftBound = Math.Max(room1.Left, room2.Left);

                // Если комнаты не пересекаются по горизонтали, создаем коридор заданного размера
                if (leftBound >= rightBound)
                {
                    int centerX = (int)Math.Round((room1.GetCenter().X + room2.GetCenter().X) / 2.0f);
                    leftBound = centerX - corridorSize.X / 2;
                    rightBound = leftBound + corridorSize.X;
                }

                newPosition = new Vector2Int(leftBound, bottomBound);
                newSize = new Vector2Int(rightBound - leftBound, topBound - bottomBound);
            }

            // Проверяем, что размеры положительные
            if (newSize.X <= 0 || newSize.Y <= 0)
            {
                // Если размеры некорректные, создаем минимальный коридор между комнатами
                Vector2 midPoint = new Vector2(
                    (center1.X + center2.X) / 2.0f,
                    (center1.Y + center2.Y) / 2.0f
                );

                newPosition = new Vector2Int(
                    (int)Math.Round(midPoint.X - corridorSize.X / 2.0f),
                    (int)Math.Round(midPoint.Y - corridorSize.Y / 2.0f)
                );
                newSize = corridorSize;
            }

            return m_RoomCreator.Create(newPosition, newSize);
        }

        public DungeonRoomData CreateConnectingRoom(DungeonRoomData a, DungeonRoomData b)
        {
            // Получаем центры комнат
            Vector2Int centerA = new Vector2Int((int)a.GetCenter().X, (int)a.GetCenter().Y);
            Vector2Int centerB = new Vector2Int((int)b.GetCenter().X, (int)b.GetCenter().Y);

            // Определим направление соединения (по X или по Y)
            bool horizontal = Math.Abs(centerA.X - centerB.X) > Math.Abs(centerA.Y - centerB.Y);

            if (horizontal)
            {
                // Горизонтальное соединение (комната-коридор вдоль X)
                int y = Math.Max(a.Bottom, b.Bottom);
                int height = Math.Min(a.Top, b.Top) - y;
                height = Math.Max(1, height); // хотя бы 1, если нет пересечения по Y

                int left = Math.Min(centerA.X, centerB.X);
                int right = Math.Max(centerA.X, centerB.X);
                int width = right - left;

                Vector2Int position = new Vector2Int(left, y);
                Vector2Int size = new Vector2Int(Math.Max(1, width), height);

                return m_RoomCreator.Create(position, size);
            }
            else
            {
                // Вертикальное соединение (комната-коридор вдоль Y)
                int x = Math.Max(a.Left, b.Left);
                int width = Math.Min(a.Right, b.Right) - x;
                width = Math.Max(1, width); // хотя бы 1, если нет пересечения по X

                int bottom = Math.Min(centerA.Y, centerB.Y);
                int top = Math.Max(centerA.Y, centerB.Y);
                int height = top - bottom;

                Vector2Int position = new Vector2Int(x, bottom);
                Vector2Int size = new Vector2Int(width, Math.Max(1, height));

                return m_RoomCreator.Create(position, size);
            }
        }*/

        public string GetName()
        {
            return "Create Corridors";
        }
    }
}