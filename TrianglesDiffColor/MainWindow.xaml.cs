using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
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
        var root = new Rectangle(0, 0, 500, 500, 0, 0, 500, 500)
        {
            Color = Colors.LightGreen
        };

        var triangles = JsonConvert.DeserializeObject<IEnumerable<Triangle>>(rawTrianglesJson)!
                                   .OrderByDescending(_ => _.Square)
                                   .ToList();

        root.BuildTriangles(triangles);

        RootCanvas.Children.Add(root.CreateVisual());

        foreach ( var triangle in triangles )
        {
            RootCanvas.Children.Add(triangle.CreateVisual());
        }

        if (triangles.Any(_ => _.IsIntersect))
        {
            ContentTextBlock.Text = "Error";
            ContentTextBlock.Foreground = new SolidColorBrush(Colors.IndianRed);
        }
        else
        {
            var maxLayer = triangles.MaxBy(_ => _.Layer)!.Layer + 1;

            ContentTextBlock.Text = $"Count colors - {maxLayer}";
            ContentTextBlock.Foreground = new SolidColorBrush(Colors.Green);
        }
    }
}