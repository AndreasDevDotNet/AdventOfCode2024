namespace AoCToolbox
{
    public class SortHelpers
    {
        public static List<int> TopologicalSort(List<int> nodes, List<(int, int)> edges)
        {
            var graph = new Dictionary<int, List<int>>();
            var inDegree = new Dictionary<int, int>();

            foreach (var node in nodes)
            {
                graph[node] = new List<int>();
                inDegree[node] = 0;
            }

            foreach (var (x, y) in edges)
            {
                if (nodes.Contains(x) && nodes.Contains(y))
                {
                    graph[x].Add(y);
                    inDegree[y]++;
                }
            }

            var sorted = new List<int>();
            var queue = new Queue<int>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                sorted.Add(node);

                foreach (var neighbor in graph[node])
                {
                    inDegree[neighbor]--;
                    if (inDegree[neighbor] == 0)
                        queue.Enqueue(neighbor);
                }
            }

            return sorted;
        }
        public int[] BubbleSort(int[] numArray)
        {
            var n = numArray.Length;
            bool swapRequired;
            for (int i = 0; i < n - 1; i++)
            {
                swapRequired = false;
                for (int j = 0; j < n - i - 1; j++)
                    if (numArray[j] > numArray[j + 1])
                    {
                        var tempVar = numArray[j];
                        numArray[j] = numArray[j + 1];
                        numArray[j + 1] = tempVar;
                        swapRequired = true;
                    }
                if (swapRequired == false)
                    break;
            }
            return numArray;
        }

    }
}
