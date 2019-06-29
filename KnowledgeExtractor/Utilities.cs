using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace KnowledgeExtractor
{
    public static class Utilities
    {
        public static char[] WindowsBannedCharsFromFilenames = new char[9] { '/', '\\', '*', ':', '?', '"', '<', '>', '|' };

        public enum OriginalGraphType
        {
            Unknown,
            AlgorithmsKnGraph,
            DataStructuresKnGraph
        }

        public struct UriAndOriginalGraphType
        {
            public Uri Uri;

            public OriginalGraphType OriginalGraphType;
        }

        public static string FormatKnGraphIndexAndLabelsForPrinting(KnowledgeGraph graph)
        {
            StringBuilder sb = new StringBuilder();

            foreach(KnGNode node in graph.KnGraph)
            {
                sb.AppendLine(node.OriginalGraphType + "-" + node.Index + ": " + node.HtmlName + ": " + node.Label + ", " + node.LinkToPage);
                sb.Append(node.Index + ": ");
                foreach (KnGNode neighbor in node.Neighbors)
                {
                    sb.Append(neighbor.Index + ", ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

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
                    DFSRecursive(graph, visited, node, 0, ref sb);
                }
            }

            return sb.ToString();
        }

        private static void DFSRecursive(KnowledgeGraph graph, List<bool> visited, KnGNode currentNode, int level, ref StringBuilder sb)
        {
            sb.Append(currentNode.Label);
            sb.AppendLine();
            visited[currentNode.Index] = true;

            level++;

            foreach (KnGNode neighbor in graph.KnGraph[currentNode.Index].Neighbors)
            {
                if (!visited[neighbor.Index])
                {
                    PrintLevelSpaces(level, ref sb);
                    DFSRecursive(graph, visited, neighbor, level, ref sb);
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

        public static void SaveUriToHtmlFile(Uri uri, string filePath)
        {
            HtmlDocument doc = WKGE.GetHtmlDocumentFromUri(uri);
            File.WriteAllText(filePath, doc.DocumentNode.OuterHtml);
        }

        public static void SaveUriPageContentToTxtFile(Uri uri, string filePath)
        {
            HtmlDocument doc = WKGE.GetHtmlDocumentFromUri(uri);
            HtmlNode contentRoot = WKGE.GetWikipediaPageContentNode(doc);
            if(contentRoot != null)
            {
                File.WriteAllText(filePath, contentRoot.InnerText);
            }
        }

        public static void DownloadAllPagesInKnGraph()
        {
            KnowledgeGraph knowledgeGraph = WKGE.ExtractKnGraphFromUris(WKGE.WikipediaPagesToParse);

            // Download the html files
            SaveUriToHtmlFile(WKGE.GetWikipediaListOfAlgorithmsPageUri(), "../../../DownloadedHtmlPages/listOfAlgos.html");
            SaveUriToHtmlFile(WKGE.GetWikipediaListOfDataStructuresPageUri(), "../../../DownloadedHtmlPages/listOfDataStructures.html");

            // Download the content of each link in the nodes of the kn graph
            foreach (KnGNode node in knowledgeGraph.KnGraph.Where(x => x.LinkToPage != null))
            {
                string[] linkBits = node.LinkToPage.ToString().Split('/');
                string fileName = RemoveCharsFromString(linkBits[linkBits.Length - 1], WindowsBannedCharsFromFilenames) + ".txt";
                string filePath = "../../../DownloadedHtmlPages/" + fileName;

                if (!File.Exists(filePath))
                {
                    SaveUriPageContentToTxtFile(node.LinkToPage, filePath);
                    Console.WriteLine("Wrote file with name " + fileName);
                }
                else
                {
                    Console.WriteLine("!!! ALREADYEXISTS !!! " + filePath);
                }
            }
        }

        public static string RemoveCharsFromString(string source, char[] chars)
        {
            return String.Join("", source.ToCharArray().Where(a => !chars.Contains(a)).ToArray());
        }
    }
}
