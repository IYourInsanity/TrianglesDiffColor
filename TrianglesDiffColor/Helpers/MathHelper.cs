using System.Windows;
using TrianglesDiffColor.Models;

namespace TrianglesDiffColor.Helpers;
internal static class MathHelper
{
    internal static double GetLineSegment(Point point1, Point point2)
    {
        return Math.Floor(Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2)));
    }

    internal static double CalculateDifference(Point point1, Point point2)
    {
        return (point1.X - point2.X) + (point1.Y - point2.Y);
    }

    internal static double GetTriangleSquare(double value1, double value2, double value3)
    {
        var p = (value1 + value2 + value3) / 2;

        return Math.Floor(Math.Sqrt(p * (p - value1) * (p - value2) * (p - value3)));
    }

    internal static bool IsIntersect(this Triangle source, Triangle target)
    {
        var diffAB = IsSegmentLinesIntersect(source.PosA, source.PosB, target.PosA, target.PosB);
        var diffBC = IsSegmentLinesIntersect(source.PosB, source.PosC, target.PosB, target.PosC);
        var diffAC = IsSegmentLinesIntersect(source.PosA, source.PosC, target.PosA, target.PosC);

        if (diffAB || diffBC || diffAC)
        {
            return true;
        }

        return false;
    }

    internal static bool IsInclude(this Triangle source, Triangle target)
    {
        if(source.Square < target.Square)
        {
            return false;
        }

        var segmentAB = CheckSegment(source.LineSegmentAB, target.LineSegmentAB, source.PosA, source.PosB, target.PosA, target.PosB);
        var segmentBC = CheckSegment(source.LineSegmentBC, target.LineSegmentBC, source.PosB, source.PosC, target.PosB, target.PosC);
        var segmentAC = CheckSegment(source.LineSegmentAC, target.LineSegmentAC, source.PosA, source.PosC, target.PosA, target.PosC);
        
        if(segmentAB && segmentBC && segmentAC)
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
            var source1ToTarget1 = GetLineSegment(sourcePos1, targetPos1);
            var source2ToSource2 = GetLineSegment(sourcePos2, targetPos2);

            if (source1ToTarget1 + source2ToSource2 - sourceLen < sourceLen)
            {
                return true;
            }

            return false;
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
        else if(directionSourceX == 0 &&
                directionSourceY == 0)
        {
            return false;
        }
        else if(directionTargetX == 0 &&
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

        if(directionSourceX == 0)
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

    internal static byte CalculateColor(byte source)
    {
        const int VALUE = 20;

        if (source == 0)
        {
            return source;
        }

        return (byte)(source - VALUE);
    }
}
