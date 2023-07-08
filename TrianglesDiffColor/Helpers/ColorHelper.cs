using System.Windows.Media;

namespace TrianglesDiffColor.Helpers;
internal static class ColorHelper
{
    internal static Color GenerateColorFromSourceByLayer(this Color source, int layer)
    {
        var a = CalculateColor(source.A, layer);
        var r = CalculateColor(source.R, layer);
        var g = CalculateColor(source.G, layer);
        var b = CalculateColor(source.B, layer);

        return Color.FromArgb(a, r, g, b);
    }

    internal static byte CalculateColor(byte source, int layer)
    {
        const int VALUE = 20;

        if (source == 0)
        {
            return source;
        }

        return (byte)(source - VALUE * layer);
    }
}
