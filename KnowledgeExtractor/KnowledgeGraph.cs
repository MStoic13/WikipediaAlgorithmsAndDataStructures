using System.Collections.Generic;
using System.Text;

namespace KnowledgeExtractor
{
    public class KnowledgeGraph
    {
        public List<List<int>> GraphSkeleton { get; set; }

        public List<string> NodeNames { get; set; }

        public string FormatGraphIntsForPrinting()
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (List<int> list in this.GraphSkeleton)
            {
                sb.Append(index + ": ");
                foreach (int x in list)
                {
                    sb.Append(x + ", ");
                }

                sb.AppendLine();
                index++;
            }

            return sb.ToString();
        }

        public string FormatGraphWithNamesForPrinting()
        {
            // we'll use DFS to print the names according to the graph and with indentation
            List<bool> visited = new List<bool>();
            this.GraphSkeleton.ForEach(x => visited.Add(false));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.GraphSkeleton.Count; i++)
            {
                if (!visited[i])
                {
                    DFSRecursive(visited, i, 0, ref sb);
                }
            }

            return sb.ToString();
        }

        private void DFSRecursive(List<bool> visited, int currentNode, int level, ref StringBuilder sb)
        {
            sb.Append(this.NodeNames[currentNode]);
            sb.AppendLine();
            visited[currentNode] = true;

            level++;

            foreach (int neighbor in this.GraphSkeleton[currentNode])
            {
                if (!visited[neighbor])
                {
                    PrintLevelSpaces(level, ref sb);
                    DFSRecursive(visited, neighbor, level, ref sb);
                }
            }

            level--;
        }

        private static void PrintLevelSpaces(int level, ref StringBuilder sb)
        {
            for (int i = 1; i <= level; i++)
            {
                sb.Append("    ");
            }
        }
    }
}
