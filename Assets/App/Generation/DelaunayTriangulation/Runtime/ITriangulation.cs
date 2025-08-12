using System.Collections.Generic;
using Assets.App.Common.Algorithms.Runtime;

namespace App.Generation.DelaunayTriangulation.Runtime
{
    public interface ITriangulation
    {
        List<Triangle> Triangulate(IEnumerable<Vector2> points);
        List<Edge> GetEdges();
    }
}