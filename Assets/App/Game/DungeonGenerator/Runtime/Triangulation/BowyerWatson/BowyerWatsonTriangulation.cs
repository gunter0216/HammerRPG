using System;
using System.Collections.Generic;

public class Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double DistanceTo(Point other)
    {
        return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }
}

public class Triangle
{
    public Point P1 { get; set; }
    public Point P2 { get; set; }
    public Point P3 { get; set; }

    public Triangle(Point p1, Point p2, Point p3)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    public bool ContainsPoint(Point p)
    {
        double area1 = Area(P1, P2, p);
        double area2 = Area(P2, P3, p);
        double area3 = Area(P3, P1, p);

        double totalArea = Area(P1, P2, P3);

        return Math.Abs(totalArea - (area1 + area2 + area3)) < 1e-10;
    }

    private double Area(Point a, Point b, Point c)
    {
        return (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)) / 2.0;
    }
}

public class BowyerWatsonTriangulation
{
    private List<Point> points = new List<Point>();
    private List<Triangle> triangles = new List<Triangle>();

    public void AddPoint(Point p)
    {
        points.Add(p);
        List<Triangle> badTriangles = new List<Triangle>();

        foreach (var triangle in triangles)
        {
            if (triangle.ContainsPoint(p))
            {
                badTriangles.Add(triangle);
            }
        }

        List<Edge> polygon = new List<Edge>();

        foreach (var badTriangle in badTriangles)
        {
            polygon.Add(new Edge(badTriangle.P1, badTriangle.P2));
            polygon.Add(new Edge(badTriangle.P2, badTriangle.P3));
            polygon.Add(new Edge(badTriangle.P3, badTriangle.P1));
        }

        foreach (var badTriangle in badTriangles)
        {
            triangles.Remove(badTriangle);
        }

        foreach (var edge in polygon)
        {
            if (!IsEdgeShared(edge, polygon))
            {
                triangles.Add(new Triangle(edge.Start, edge.End, p));
            }
        }
    }

    public List<Triangle> GetTriangles()
    {
        return triangles;
    }

    private bool IsEdgeShared(Edge edge, List<Edge> polygon)
    {
        int count = 0;
        foreach (var otherEdge in polygon)
        {
            if ((edge.Start == otherEdge.Start && edge.End == otherEdge.End) ||
                (edge.Start == otherEdge.End && edge.End == otherEdge.Start))
            {
                count++;
            }
        }
        return count == 2;
    }

    public void Initialize(List<Point> initialPoints)
    {
        points = initialPoints;
        triangles.Clear();

        // Добавляем начальные треугольники
        // Например, можно использовать произвольный треугольник для инициализации
        var p1 = new Point(-1000, -1000);
        var p2 = new Point(1000, -1000);
        var p3 = new Point(0, 1000);

        triangles.Add(new Triangle(p1, p2, p3));

        foreach (var point in points)
        {
            AddPoint(point);
        }
    }

    public class Edge
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public Edge(Point start, Point end)
        {
            Start = start;
            End = end;
        }
    }
}