using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrianglesDiffColor.Helpers;

namespace TrianglesDiffColor.Models;

public abstract class Geometric
{
    protected const int SIZE_MULTIPLY = 2;

    private Geometric? _parent;

    public Geometric? Parent
    {
        get => _parent;
        set
        {
            _parent = value;

            if (value != null)
            {
                Layer = value.Layer + 1;
                Color = value.Color?.GenerateColorFromSourceByLayer(Layer);
            }
        }
    }

    public double Square { get; set; }

    public Color? Color { get; set; }

    public List<Geometric> Children { get; }

    public List<double> Segments { get; }

    public List<Point> Positions { get; }

    public bool IsIntersect { get; set; }

    public bool IsCompiled { get; set; }

    public int Layer { get; protected set; }

    protected Geometric(int count)
    {
        Children = new List<Geometric>();
        Segments = new List<double>(count);
        Positions = new List<Point>(count);
    }

    public abstract Polygon CreateVisual();
}
