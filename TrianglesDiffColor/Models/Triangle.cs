using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrianglesDiffColor.Helpers;

namespace TrianglesDiffColor.Models;
public class Triangle : Geometric
{
    public Point Pos1 { get; }

    public Point Pos2 { get; }

    public Point Pos3 { get; }

    public double LineSegment1 { get; }

    public double LineSegment2 { get; }

    public double LineSegment3 { get; }

    [JsonConstructor]
    public Triangle(int x1, int y1, int x2, int y2, int x3, int y3)
        : this(x1, y1, x2, y2, x3, y3, true, 3)
    {
        
    }

    protected Triangle(int x1, int y1, int x2, int y2, int x3, int y3, bool calculateSquare = true, int count = 3)
        : base(count)
    {
        Pos1 = new Point(x1 * SIZE_MULTIPLY, y1 * SIZE_MULTIPLY);
        Pos2 = new Point(x2 * SIZE_MULTIPLY, y2 * SIZE_MULTIPLY);
        Pos3 = new Point(x3 * SIZE_MULTIPLY, y3 * SIZE_MULTIPLY);

        LineSegment1 = GeometricHelper.CalculateLineSegment(Pos1, Pos2);
        LineSegment2 = GeometricHelper.CalculateLineSegment(Pos2, Pos3);
        LineSegment3 = GeometricHelper.CalculateLineSegment(Pos1, Pos3);

        Segments.Add(LineSegment1);
        Segments.Add(LineSegment2);
        Segments.Add(LineSegment3);

        Positions.Add(Pos1);
        Positions.Add(Pos2);
        Positions.Add(Pos3);

        if (calculateSquare)
        {
            Square = GeometricHelper.CalculateTriangleSquare(LineSegment1, LineSegment2, LineSegment3);
        }
    }

    public override Polygon CreateVisual()
    {
        var polygon = new Polygon()
        {
            Fill = IsIntersect ? new SolidColorBrush(Colors.IndianRed) : new SolidColorBrush((Color)Color!)
        };

        polygon.Points.Add(Pos1);
        polygon.Points.Add(Pos2);
        polygon.Points.Add(Pos3);

        return polygon;
    }
}
