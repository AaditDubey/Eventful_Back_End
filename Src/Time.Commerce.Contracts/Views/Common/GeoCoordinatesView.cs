namespace Time.Commerce.Contracts.Views.Common;

public class GeoCoordinatesView
{
    public GeoCoordinatesView(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets the X coordinate.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the Y coordinate.
    /// </summary>
    public double Y { get; }
}
