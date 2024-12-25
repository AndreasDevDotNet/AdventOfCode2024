namespace AoCToolbox
{
    public static class ArrayExtensions
    {
        public static T[,] Reshape<T>(this T[,] array, int rows, int columns)
        {
            T[,] result = new T[rows, columns];
            int index = 0;

            foreach (T element in array)
            {
                result[index / columns, index % columns] = element;
                index++;
            }

            return result;
        }

        public static T[,] Transpose<T>(this T[,] array)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);

            T[,] result = new T[columns, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[j, i] = array[i, j];
                }
            }

            return result;
        }

        public static T[,] Append<T>(this T[,] array, T[] elements)
        {
            int rows = array.GetLength(0);
            int columns = array.GetLength(1);
            int newRows = rows + 1;

            T[,] result = new T[newRows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = array[i, j];
                }
            }

            for (int j = 0; j < columns; j++)
            {
                result[rows, j] = elements[j];
            }

            return result;
        }

        public static char[][] AppendItems(this char[][] source, char[][] elements, char elementToFind)
        {
            int rows = source.Length;
            int columns = source[0].Length;

            char[][] result = new char[rows][];

            for (int i = 0; i < rows; i++)
            {
                result[i] = new char[columns];
                for (int j = 0; j < columns; j++)
                {
                    result[i][j] = source[i][j];
                }
            }

            for (int i = 0; i < elements.Length; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (elements[i][j] == elementToFind)
                        result[i][j] = elements[i][j];
                }
            }

            return result;
        }

        public static T[][] Transpose<T>(this T[][] matrix)
        {
            if (matrix.Length == 0 || matrix[0].Length == 0)
                throw new InvalidOperationException("Invalid matrix dimensions.");

            int rows = matrix.Length;
            int cols = matrix[0].Length;

            T[][] result = new T[cols][];
            for (int c = 0; c < cols; c++)
            {
                result[c] = new T[rows];
                for (int r = 0; r < rows; r++)
                {
                    try
                    {
                        result[c][r] = matrix[r][c];
                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
                }
            }

            return result;
        }
    }
}
