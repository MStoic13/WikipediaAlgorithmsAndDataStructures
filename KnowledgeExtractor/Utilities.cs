using System.Collections.Generic;
using System.Text;

namespace KnowledgeExtractor
{
    public static class Utilities
    {
        public static string FormatKnGraphIndexesForPrinting(KnowledgeGraph graph)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KnGNode node in graph.KnGraph)
            {
                sb.Append(node.Index + ": ");
                foreach (KnGNode neighbor in node.Neighbors)
                {
                    sb.Append(neighbor.Index + ", ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string FormatKnGraphLabelsForPrinting(KnowledgeGraph graph)
        {
            // we'll use DFS to print the names according to the graph and with indentation
            List<bool> visited = new List<bool>();
            graph.KnGraph.ForEach(x => visited.Add(false));
            StringBuilder sb = new StringBuilder();

            foreach (KnGNode node in graph.KnGraph)
            {
                if (!visited[node.Index])
                {
                    DFSRecursive(visited, node, 0, ref sb);
                }
            }

            return sb.ToString();
        }

        private static void DFSRecursive(List<bool> visited, KnGNode currentNode, int level, ref StringBuilder sb)
        {
            sb.Append(currentNode.Label);
            sb.AppendLine();
            visited[currentNode.Index] = true;

            level++;

            foreach (KnGNode neighbor in currentNode.Neighbors)
            {
                if (!visited[neighbor.Index])
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
