using Newtonsoft.Json;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TrianglesDiffColor.Helpers;
using TrianglesDiffColor.Models;

namespace TrianglesDiffColor;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var rawTrianglesJson = Properties.Resources.Coordinates;
        var triangles = JsonConvert.DeserializeObject<IEnumerable<Triangle>>(rawTrianglesJson)!
                                   .OrderByDescending(_ => _.Square)
                                   .Compile(Colors.Green);

        var hasIntersect = triangles.Any(_ => _.IsIntersect);

        foreach ( var triangle in triangles )
        {
            var polygon = new Polygon()
            {
                //Stroke = new SolidColorBrush(Colors.Black),
                Fill = triangle.IsIntersect ? new SolidColorBrush(Colors.IndianRed) : new SolidColorBrush((Color)triangle!.Color!)
            };

            polygon.Points.Add(triangle.PosA);
            polygon.Points.Add(triangle.PosB);
            polygon.Points.Add(triangle.PosC);

            RootCanvas.Children.Add(polygon);
        }

        if (hasIntersect)
        {
            ContentTextBlock.Text = "Error";
            ContentTextBlock.Foreground = new SolidColorBrush(Colors.IndianRed);
        }
        else
        {
            ContentTextBlock.Text = $"Count colors - {triangles.MaxBy(_ => _.TreeLevel)!.TreeLevel + 1}";
            ContentTextBlock.Foreground = new SolidColorBrush(Colors.Green);
        }
    }
}