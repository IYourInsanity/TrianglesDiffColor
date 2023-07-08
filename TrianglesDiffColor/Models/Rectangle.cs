using Newtonsoft.Json;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrianglesDiffColor.Helpers;

namespace TrianglesDiffColor.Models;
public class Rectangle : Triangle
{
    public Point Pos4 { get; }

    public double LineSegment4 { get; }

    [JsonConstructor]
    public Rectangle(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        : base(x1, y1, x2, y2, x3, y3, calculateSquare: false, count: 4)
    {
        Pos4 = new Point(x4 * SIZE_MULTIPLY, y4 * SIZE_MULTIPLY);

        LineSegment4 = GeometricHelper.CalculateLineSegment(Pos1, Pos4);

        Segments.Add(LineSegment4);
        Positions.Add(Pos4);

        Square = GeometricHelper.CalculateRectangleSquare(LineSegment1, LineSegment2, LineSegment3, LineSegment4);
    }

    public override Polygon CreateVisual()
    {
        var polygon = new Polygon()
        {
            Fill = new SolidColorBrush(Colors.Black)
        };

        polygon.Points.Add(Pos1);
        polygon.Points.Add(Pos2);
        polygon.Points.Add(Pos3);
        polygon.Points.Add(Pos4);

        return polygon;
    }
}
