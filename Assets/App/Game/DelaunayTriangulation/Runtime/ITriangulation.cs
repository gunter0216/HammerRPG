using System.Collections.Generic;

namespace App.Game.DelaunayTriangulation.Runtime
{
    public interface ITriangulation
    {
        List<Triangle> Triangulate(IEnumerable<Point> points);
        List<Edge> GetEdges();
    }
}