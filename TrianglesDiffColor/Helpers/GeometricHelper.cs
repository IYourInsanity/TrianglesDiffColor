using System.Collections.Concurrent;
using System.Windows;
using TrianglesDiffColor.Models;

namespace TrianglesDiffColor.Helpers;
internal static class GeometricHelper
{
    internal static void BuildTriangles(this Rectangle root, IEnumerable<Triangle> triangles)
    {
        Geometric tempRoot = root;

        while (triangles.Any(_ => _.IsCompiled == false))
        {
            var filteredTriangles = triangles.Where(x => x.IsCompiled == false);

            foreach (var triangle in filteredTriangles)
            {
                if (tempRoot is Rectangle)
                {
                    Combine(tempRoot, triangle);

                    tempRoot = triangle;
                    triangle.IsCompiled = true;
                    continue;
                }

                var isCompiled = CompileByIntersect(tempRoot, triangle);

                if (isCompiled)
                {
                    triangle.IsCompiled = true;
                    tempRoot = triangle;
                    continue;
                }
                else
                {
                    isCompiled = CompileByInclude(tempRoot, triangle);

                    if (isCompiled)
                    {
                        triangle.IsCompiled = true;
                        tempRoot = triangle;
                        continue;
                    }
                }

                if (isCompiled == false)
                {
                    tempRoot = tempRoot!.Parent!;
                    break;
                }
            }
        }
    }

    internal static void Combine<TGeometric1, TGeometric2>(TGeometric1 source, TGeometric2 target)
        where TGeometric1 : Geometric
        where TGeometric2 : Geometric
    {
        target.Parent = source;
        source.Children.Add(target);
    }

    internal static bool CompileByIntersect<TGeometric1, TGeometric2>(TGeometric1 source, TGeometric2 target)
        where TGeometric1 : Geometric
        where TGeometric2 : Geometric
    {
        var isIntersect = source.TryIntersect(target);

        if (isIntersect)
        {
            target.IsIntersect = isIntersect;
            Combine(source, target);

            return true;
        }

        return false;
    }

    internal static bool CompileByInclude<TGeometric1, TGeometric2>(TGeometric1 source, TGeometric2 target)
        where TGeometric1 : Geometric
        where TGeometric2 : Geometric
    {
        var isInclude = source.TryInclude(target);

        if (isInclude)
        {
            Combine(source, target);
            return true;
        }
        else
        {
            foreach (var children in source.Children)
            {
                if (CompileByInclude(children, target))
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool TryIntersect<TGeometric1, TGeometric2>(this TGeometric1 source, TGeometric2 target)
        where TGeometric1 : Geometric
        where TGeometric2 : Geometric
    {
        if (source is Triangle tSource &&
           target is Triangle tTarget)
        {
            return TryIntersectTriangle(tSource, tTarget);
        }

        if (source is Rectangle rSource &&
            target is Rectangle rTarget)
        {
            return TryIntersectRectangle(rSource, rTarget);
        }

        return false;
    }

    internal static bool TryInclude<TGeometric1, TGeometric2>(this TGeometric1 source, TGeometric2 target) 
        where TGeometric1 : Geometric 
        where TGeometric2 : Geometric
    {
        if (source.Square < target.Square)
        {
            return false;
        }

        
        var sourceSegmentCount = source.Segments.Count;
        var targetSegmentCount = target.Segments.Count;

        if (sourceSegmentCount != targetSegmentCount)
        {
            return false;
        }

        var resultBag = new List<bool>(sourceSegmentCount);

        for (int i = 0; i < sourceSegmentCount; i++)
        {
            var shouldInverse = i + 1 > sourceSegmentCount;

            var sourceSegment = source.Segments[i];
            var targetSegment = target.Segments[shouldInverse ? i + 1 : 0];

            var sourcePos1 = source.Positions[i];
            var sourcePos2 = source.Positions[shouldInverse ? i + 1 : 0];

            var targetPos1 = target.Positions[i];
            var targetPos2 = target.Positions[shouldInverse ? i + 1 : 0];

            var result = CheckSegment(sourceSegment, targetSegment, sourcePos1, sourcePos2, targetPos1, targetPos2);

            resultBag.Add(result);
        }

        if (resultBag.All(include => include))
        {
            return true;
        }

        return false;
    }

    internal static bool CheckSegment(double sourceLen, double targetLen, Point sourcePos1, Point sourcePos2, Point targetPos1, Point targetPos2)
    {
        var t1 = sourceLen - targetLen;

        if (t1 > 0)
        {
            var source1ToTarget1 = CalculateLineSegment(sourcePos1, targetPos1);
            var source2ToSource2 = CalculateLineSegment(sourcePos2, targetPos2);

            if (source1ToTarget1 + source2ToSource2 - sourceLen < sourceLen)
            {
                return true;
            }

            return false;
        }

        return false;
    }

    internal static double CalculateLineSegment(Point point1, Point point2)
    {
        return Math.Floor(Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2)));
    }

    internal static bool TryIntersect<TGeometric>(this TGeometric source, TGeometric target)
        where TGeometric : Geometric
    {
        if(source is Triangle tSource &&
           target is Triangle tTarget)
        {
            return TryIntersectTriangle(tSource, tTarget);
        }

        if (source is Rectangle rSource &&
            target is Rectangle rTarget)
        {
            return TryIntersectRectangle(rSource, rTarget);
        }

        return false;
    }

    private static bool TryIntersectTriangle(Triangle source, Triangle target)
    {
        var diff12 = IsSegmentLinesIntersect(source.Pos1, source.Pos2, target.Pos1, target.Pos2);
        var diff23 = IsSegmentLinesIntersect(source.Pos2, source.Pos3, target.Pos2, target.Pos3);
        var diff13 = IsSegmentLinesIntersect(source.Pos1, source.Pos3, target.Pos1, target.Pos3);

        if (diff12 || diff23 || diff13)
        {
            return true;
        }

        return false;
    }

    private static bool TryIntersectRectangle(Rectangle source, Rectangle target)
    {
        var diff12 = IsSegmentLinesIntersect(source.Pos1, source.Pos2, target.Pos1, target.Pos2);
        var diff23 = IsSegmentLinesIntersect(source.Pos2, source.Pos3, target.Pos2, target.Pos3);
        var diff34 = IsSegmentLinesIntersect(source.Pos3, source.Pos4, target.Pos3, target.Pos4);
        var diff14 = IsSegmentLinesIntersect(source.Pos1, source.Pos4, target.Pos1, target.Pos4);

        if (diff12 || diff23 || diff34 || diff14)
        {
            return true;
        }

        return false;
    }

    internal static bool IsSegmentLinesIntersect(Point source1, Point source2, Point target1, Point target2)
    {
        var directionSourceX = source2.X - source1.X;
        var directionSourceY = source2.Y - source1.Y;

        var directionTargetX = target2.X - target1.X;
        var directionTargetY = target2.Y - target1.Y;

        if (directionSourceX == 0 &&
            directionSourceY == 0 &&
            directionTargetX == 0 &&
            directionTargetY == 0)
        {
            return false;
        }
        else if (directionSourceX == 0 &&
                directionSourceY == 0)
        {
            return false;
        }
        else if (directionTargetX == 0 &&
                directionTargetY == 0)
        {
            return false;
        }

        var lenSource = Math.Sqrt(directionSourceX * directionSourceX + directionSourceY * directionSourceY);
        var lenTarget = Math.Sqrt(directionTargetX * directionTargetX + directionTargetY * directionTargetY);

        double x = directionSourceX / lenSource;
        double y = directionSourceY / lenSource;
        double x2 = directionTargetX / lenTarget;
        double y2 = directionTargetY / lenTarget;

        var epsilon = 0.000001;

        if (source1.X == target1.X &&
            source1.Y == target1.Y &&
            source2.X == target2.X &&
            source2.Y == target2.Y)
        {
            return false;
        }

        if (Math.Abs(x - x2) < epsilon && Math.Abs(y - y2) < epsilon)
        {
            return false;
        }

        var part1 = (-directionSourceY * target1.X + directionSourceY * source1.X + directionSourceX * target1.Y - directionSourceX * source1.Y);
        var part2 = (directionSourceY * directionTargetX - directionSourceX * directionTargetY);

        var t2 = part1 / part2;

        if (directionSourceX == 0)
        {
            return false;
        }

        var t = (target1.X - source1.X + directionTargetX * t2) / directionSourceX;

        if (t < 0 || t > 1 || t2 < 0 || t2 > 1)
        {
            return false;
        }

        return true;
    }

    internal static double CalculateRectangleSquare(double value1, double value2, double value3, double value4)
    {
        var p = (value1 + value2 + value3 + value4) / 2;

        return Math.Floor(Math.Sqrt(p * (p - value1) * (p - value2) * (p - value3) * (p - value4)));
    }

    internal static double CalculateTriangleSquare(double value1, double value2, double value3)
    {
        var p = (value1 + value2 + value3) / 2;

        return Math.Floor(Math.Sqrt(p * (p - value1) * (p - value2) * (p - value3)));
    }
}
