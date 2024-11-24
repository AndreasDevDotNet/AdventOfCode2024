using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AoCToolbox;

public static class PointExtensions
{
    public static int Distance(this Point point, Point other)
    {
        return Math.Abs(point.X - other.X) + Math.Abs(point.Y - other.Y);
    }

    public static int ReadingOrder(this Point point)
    {
        return point.Y * 1000 + point.X;
    }

    public static Point Left(this Point point)
    {
        return new Point(point.X - 1, point.Y);
    }

    public static Point Right(this Point point)
    {
        return new Point(point.X + 1, point.Y);
    }

    public static Point Up(this Point point)
    {
        return new Point(point.X, point.Y - 1);
    }

    public static Point Down(this Point point)
    {
        return new Point(point.X, point.Y + 1);
    }

    public static bool IsNeighbour(this Point point, Point otherPoint)
    {
        if (point.X == otherPoint.X)
        {
            return Math.Abs(point.Y - otherPoint.Y) == 1;
        }
        else if (point.Y == otherPoint.Y)
        {
            return Math.Abs(point.X - otherPoint.X) == 1;
        }

        return false;
    }

    public static bool IsNeighbourWithDiagnoals(this Point point, Point otherPoint)
    {
        var absDiffX = Math.Abs(point.X - otherPoint.X);
        var absDiffY = Math.Abs(point.Y - otherPoint.Y);

        return absDiffX + absDiffY > 0 &&
            absDiffX < 2 &&
            absDiffY < 2;
    }

    public static bool IsNeighbourWithDiagnoalsOnRange(this Point point, IEnumerable<Point> otherPoints)
    {
        foreach (var otherPoint in otherPoints)
        {
            var absDiffX = Math.Abs(point.X - otherPoint.X);
            var absDiffY = Math.Abs(point.Y - otherPoint.Y);

            var isNeighbour = absDiffX + absDiffY > 0 &&
                absDiffX < 2 &&
                absDiffY < 2;
            
            if (isNeighbour)
            {
                return true;
            }
        }

        return false;
    }

    // Shoelace formula, found it on the internet:
    // https://rosettacode.org/wiki/Shoelace_formula_for_polygonal_area#C#
    public static double ShoelaceArea(this List<Point> polygon)
    {
        var n = polygon.Count;
        var result = 0.0;
        for (var i = 0; i < n - 1; i++)
        {
            result += polygon[i].Y * polygon[i + 1].X - polygon[i + 1].Y * polygon[i].X;
        }

        result = Math.Abs(result + polygon[n - 1].Y * polygon[0].X - polygon[0].Y * polygon[n - 1].X) / 2.0;
        return result;
    }

    // Shoelace formula, found it on the internet:
    // https://rosettacode.org/wiki/Shoelace_formula_for_polygonal_area#C#
    public static double ShoelaceArea(this List<(long Row, long Col)> polygon)
    {
        var n = polygon.Count;
        var result = 0.0;
        for (var i = 0; i < n - 1; i++)
        {
            result += polygon[i].Row * polygon[i + 1].Col - polygon[i + 1].Row * polygon[i].Col;
        }

        result = Math.Abs(result + polygon[n - 1].Row * polygon[0].Col - polygon[0].Row * polygon[n - 1].Col) / 2.0;
        return result;
    }

    public static double ShoelaceArea(this List<(int Row, int Col)> polygon)
    {
        var n = polygon.Count;
        var result = 0.0;
        for (var i = 0; i < n - 1; i++)
        {
            result += polygon[i].Row * polygon[i + 1].Col - polygon[i + 1].Row * polygon[i].Col;
        }

        result = Math.Abs(result + polygon[n - 1].Row * polygon[0].Col - polygon[0].Row * polygon[n - 1].Col) / 2.0;
        return result;
    }

    public static List<Point> GetPointsBetween(this Point p1, Point p2)
    {
        List<Point> points = new List<Point>();

        // no slope (vertical line)
        if (p1.X == p2.X)
        {
            if (p1.Y > p2.Y)
            {
                for (int y = p1.Y; y <= p2.Y; y++)
                {
                    Point p = new Point(p1.X, y);
                    points.Add(p);
                } 
            }
            else
            {
                for (int y = p2.Y; y >= p1.Y; y--)
                {
                    Point p = new Point(p1.X, y);
                    points.Add(p);
                }
            }
        }
        else
        {
            // swap p1 and p2 if p2.X < p1.X
            if (p2.X < p1.X)
            {
                Point temp = p1;
                p1 = p2;
                p2 = temp;
            }

            double deltaX = p2.X - p1.X;
            double deltaY = p2.Y - p1.Y;
            double error = -1.0f;
            double deltaErr = Math.Abs(deltaY / deltaX);

            int y = p1.Y;
            for (int x = p1.X; x <= p2.X; x++)
            {
                Point p = new Point(x, y);
                points.Add(p);

                error += deltaErr;

                while (error >= 0.0f)
                {
                    y++;
                    points.Add(new Point(x, y));
                    error -= 1.0f;
                }
            }

            if (points.Last() != p2)
            {
                int index = points.IndexOf(p2);
                points.RemoveRange(index + 1, points.Count - index - 1);
            }
            
        }

        return points.OrderByDescending(p => p.X).ThenByDescending(p => p.Y).ToList();
    }

    public static Point MoveAfterPoint(this Point p1, Point p2)
    {
        int distance = Math.Abs(p2.X - p1.X) + Math.Abs(p2.Y - p1.Y);

        if (distance <= 1) return p1;

        var xDiff = p2.X - p1.X;
        var yDiff = p2.Y - p1.Y;

        if (xDiff == 0)
        {
            return new Point(p1.X, p1.Y + (yDiff > 0 ? 1 : -1));
        }
        else if (yDiff == 0)
        {
            return new Point(p1.X + (xDiff > 0 ? 1 : -1), p1.Y);
        }
        else if (distance > 2)
        {
            return new Point(p1.X + (xDiff > 0 ? 1 : -1), p1.Y + (yDiff > 0 ? 1 : -1));
        }
        else return (p1);
    }

    public static List<Point> CreatePointRange(this Point start, Point end)
    {
        var points = new List<Point>();

        points.Add(start);

        var xDiff = end.X - start.X;
        var yDiff = end.Y - start.Y;

        if (xDiff == 0)
        {
            for (int y = start.Y+1; y < end.Y+1; y++)
            {
                points.Add(new Point(start.X, y));
            }
        }
        if(yDiff == 0)
        {
            for(int x = start.X + 1;x < end.X + 1; x++)
            {
                points.Add(new Point(x, end.Y));
            }
        }

        return points;    
    }

    public static bool PointRangeOverlapsWith(this List<Point> points1, List<Point> points2)
    {
        return points1.Intersect(points2).Count() > 0;
    }

    public static int GetNumberOfOverlappingPoints(this List<Point> points1, List<Point> points2)
    {
        return points1.Intersect(points2).Count();
    }
}

public class PointsComparer : IEqualityComparer<Point>
{
    public bool Equals(Point x, Point y)
    {
        return y.X == x.X && y.Y == x.Y;
    }

    public int GetHashCode([DisallowNull] Point obj)
    {
        int hashCode = 256;

        hashCode += hashCode ^ obj.GetHashCode();

        return hashCode;
    }
}
