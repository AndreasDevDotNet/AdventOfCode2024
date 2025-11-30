namespace AoCToolbox
{
    public class RowCol : IComparable<RowCol>, IEquatable<RowCol>
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public RowCol(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public List<RowCol> GetDirections()
        {
            return new List<RowCol>
            {
                new RowCol(Row - 1, Col),
                new RowCol(Row + 1, Col),
                new RowCol(Row, Col - 1),
                new RowCol(Row, Col + 1)
            };
        }

        public List<RowCol> GetDirectionsWithDiagonals()
        {
            return new List<RowCol>
            {
                new RowCol(Row - 1, Col),
                new RowCol(Row + 1, Col),
                new RowCol(Row, Col - 1),
                new RowCol(Row, Col + 1),
                new RowCol(Row - 1, Col - 1),
                new RowCol(Row - 1, Col + 1),
                new RowCol(Row + 1, Col - 1),
                new RowCol(Row + 1, Col + 1)
            };
        }

        public override string ToString()
        {
            return $"Row: {Row}, Col: {Col}";
        }

        public int CompareTo(RowCol? other)
        {
            if (other == null) return 1;

            int rowComparison = Row.CompareTo(other.Row);
            if (rowComparison != 0) return rowComparison;

            return Col.CompareTo(other.Col);
        }

        public bool Equals(RowCol? other)
        {
            if (other == null) return false;
            return this.Row == other.Row && this.Col == other.Col;
        }

        public override int GetHashCode()
        {
            return this.Row.GetHashCode() ^ this.Col.GetHashCode();
        }

        public static RowCol operator +(RowCol left, RowCol right)
        {
            return new RowCol(left.Row + right.Row, left.Col + right.Col);
        }

        public static bool operator ==(RowCol left, RowCol right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(RowCol left, RowCol right)
        {
            return !(left == right);
        }

    }

    public static class RowColExtentions
    {
        public static int Distance(this RowCol rowCol, RowCol other)
        {
            return Math.Abs(rowCol.Row - other.Row) + Math.Abs(rowCol.Col - other.Col);
        }

        public static int ReadingOrder(this RowCol rowCol)
        {
            return rowCol.Col * 1000 + rowCol.Row;
        }

        public static RowCol Left(this RowCol rowCol)
        {
            return new RowCol(rowCol.Row, rowCol.Col - 1);
        }

        public static RowCol Right(this RowCol rowCol)
        {
            return new RowCol(rowCol.Row, rowCol.Col + 1);
        }

        public static RowCol Up(this RowCol rowCol)
        {
            return new RowCol(rowCol.Row - 1, rowCol.Col);
        }

        public static RowCol Down(this RowCol rowCol)
        {
            return new RowCol(rowCol.Row + 1, rowCol.Col);
        }

        public static bool IsNeighbour(this RowCol rowCol, RowCol otherRowCol)
        {
            if (rowCol.Row == otherRowCol.Row)
            {
                return Math.Abs(rowCol.Col - otherRowCol.Col) == 1;
            }
            else if (rowCol.Col == otherRowCol.Col)
            {
                return Math.Abs(rowCol.Row - otherRowCol.Row) == 1;
            }
            return false;
        }

        public static bool IsNeighbourWithDiagnoals(this RowCol rowCol, RowCol otherRowCol)
        {
            var absDiffRow = Math.Abs(rowCol.Row - otherRowCol.Row);
            var absDiffCol = Math.Abs(rowCol.Col - otherRowCol.Col);
            return absDiffRow + absDiffCol > 0 &&
                absDiffRow < 2 &&
                absDiffCol < 2;
        }

        public static bool IsNeighbourWithDiagnoalsOnRange(this RowCol rowCol, IEnumerable<RowCol> otherRowCols)
        {
            foreach (var otherRowCol in otherRowCols)
            {
                if (rowCol.IsNeighbourWithDiagnoals(otherRowCol))
                {
                    return true;
                }
            }
            return false;
        }

        public static double ShoelaceArea(this List<RowCol> polygon)
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
    }

    public class RowColComparer : IEqualityComparer<RowCol>
    {
        public bool Equals(RowCol x, RowCol y)
        {
            return x.Row == y.Row && x.Col == y.Col;
        }
        public int GetHashCode(RowCol obj)
        {
            return obj.Row.GetHashCode() ^ obj.Col.GetHashCode();
        }
    }
}
