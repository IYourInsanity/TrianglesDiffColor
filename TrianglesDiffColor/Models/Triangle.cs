using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using TrianglesDiffColor.Helpers;

namespace TrianglesDiffColor.Models;
internal sealed class Triangle
{
    const int SIZE_MULTIPLY = 2;

    private Triangle? _parent;
    internal Triangle? Parent
    {
        get => _parent;
        set 
        {
            _parent = value;

            if(value != null)
            {
                TreeLevel = value.TreeLevel + 1;

                var parentColor = value.Color!.Value;

                var a = TriangleHelper.CalculateColor(parentColor.A);
                var r = TriangleHelper.CalculateColor(parentColor.R);
                var g = TriangleHelper.CalculateColor(parentColor.G);
                var b = TriangleHelper.CalculateColor(parentColor.B);

                Color = System.Windows.Media.Color.FromArgb(a, r, g, b);
            }
        }
    }

    internal List<Triangle> Child { get; }

    internal bool IsIntersect { get; set; }

    internal bool IsCompiled { get; set; }

    internal int TreeLevel { get; private set; }   

    internal Point PosA { get; }

    internal Point PosB { get; }

    internal Point PosC { get; }

    internal double LineSegmentAB { get; }

    internal double LineSegmentBC { get; }

    internal double LineSegmentAC { get; }

    internal double Square { get; }

    internal Color? Color { get; set; }

    [JsonConstructor]
    public Triangle(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        Child = new List<Triangle>();
        IsIntersect = false;

        PosA = new Point(x1 * SIZE_MULTIPLY, y1 * SIZE_MULTIPLY);
        PosB = new Point(x2 * SIZE_MULTIPLY, y2 * SIZE_MULTIPLY);
        PosC = new Point(x3 * SIZE_MULTIPLY, y3 * SIZE_MULTIPLY);

        LineSegmentAB = MathHelper.GetLineSegment(PosA, PosB);
        LineSegmentBC = MathHelper.GetLineSegment(PosB, PosC);
        LineSegmentAC = MathHelper.GetLineSegment(PosA, PosC);

        Square = MathHelper.GetTriangleSquare(LineSegmentAB, LineSegmentBC, LineSegmentAC);
    }
}
