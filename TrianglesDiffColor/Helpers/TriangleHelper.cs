using System.Windows.Media;
using TrianglesDiffColor.Models;

namespace TrianglesDiffColor.Helpers;

internal static class TriangleHelper
{
    internal static IEnumerable<Triangle> Compile(this IEnumerable<Triangle> triangles, Color rootColor)
    {
        var root = triangles.MaxBy(_ => _.Square);

        if (root == null)
        {
            throw new NotImplementedException();
        }

        root.Color = rootColor;
        root.IsCompiled = true;

        if(triangles.Count() == 1) 
        { 
            return triangles;
        }

        var trianglesWithoutRoot = triangles.Skip(1);

        while (trianglesWithoutRoot.Any(_ => _.IsCompiled == false))
        {
            var tempRoot = root;
            var trianglesWithoutCompiled = trianglesWithoutRoot.Where(x => x.IsCompiled == false);

            foreach (var triangle in trianglesWithoutCompiled)
            {
                var isCompiled = CompileByIntersect(tempRoot, triangle);

                if( isCompiled )
                {
                    triangle.IsCompiled = true;
                    tempRoot = triangle;
                    continue;
                }
                else
                {
                    isCompiled = CompileByInclude(tempRoot, triangle);

                    if( isCompiled )
                    {
                        triangle.IsCompiled = true;
                        tempRoot = triangle;
                        continue;
                    }
                }

                if(isCompiled == false)
                {
                    break;
                }
            }
        }

        return triangles;
    }

    internal static bool CompileByIntersect(Triangle source, Triangle target)
    {
        var isIntersect = source.IsIntersect(target);

        if (isIntersect)
        {
            target.IsIntersect = isIntersect;
            target.Parent = source;
            source.Child.Add(target);

            return true;
        }

        return false;
    }

    internal static bool CompileByInclude(Triangle source, Triangle target)
    {
        var isInclude = source.IsInclude(target);

        if (isInclude)
        {
            foreach (var children in source.Child)
            {
                if(CompileByInclude(children, target))
                {
                    return true;
                }
            }

            target.Parent = source;
            source.Child.Add(target);
            return true;
        }

        return false;
    }
}
