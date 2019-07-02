using HtmlAgilityPack;
using Newtonsoft.Json;
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
                string filePath = GetFilePathFromNodeLinkTopage(node.LinkToPage);

                if (!File.Exists(filePath))
                {
                    SaveUriPageContentToTxtFile(node.LinkToPage, filePath);
                    Console.WriteLine("Wrote file with name " + Path.GetFileName(filePath));
                }
                else
                {
                    Console.WriteLine("!!! ALREADYEXISTS !!! " + filePath);
                }
            }
        }

        public static string GetFilePathFromNodeLinkTopage(Uri linkToPage)
        {
            string[] linkBits = linkToPage.ToString().Split('/');
            string fileName = RemoveCharsFromString(linkBits[linkBits.Length - 1], WindowsBannedCharsFromFilenames) + ".txt";
            string filePath = "../../../DownloadedHtmlPages/" + fileName;
            return filePath;
        }

        public static string RemoveCharsFromString(string source, char[] chars)
        {
            return String.Join("", source.ToCharArray().Where(a => !chars.Contains(a)).ToArray());
        }

        public static List<string> GetDataStructureWordsFromGraph(KnowledgeGraph knowledgeGraph)
        {
            List<string> result = new List<string>();

            List<KnGNode> dataStructureNodes = knowledgeGraph.KnGraph.Where(n => n.OriginalGraphType == OriginalGraphType.DataStructuresKnGraph && n.Neighbors.Count == 0).ToList();
            result = dataStructureNodes.Select(x => x.Label).Distinct().ToList();

            return result;
        }

        public static List<WordCount> GetDataStructureWordCountsFromGraph(KnowledgeGraph knowledgeGraph)
        {
            // compute the list of DS words
            List<string> dataStructureWords = Utilities.GetDataStructureWordsFromGraph(knowledgeGraph);
            List<WordCount> result = new List<WordCount>();

            // foreach node in the graph which has a link to page and downloaded file for that link, get the ds word count
            knowledgeGraph.KnGraph
            .Where(node => node.OriginalGraphType == OriginalGraphType.AlgorithmsKnGraph && node.LinkToPage != null && File.Exists(Utilities.GetFilePathFromNodeLinkTopage(node.LinkToPage))).ToList()
            .ForEach(node =>
            {
                // make empty ds words dictionary for this node
                Dictionary<string, int> dsWordsCountDictForOneNode = new Dictionary<string, int>();
                dataStructureWords.ForEach(dsWord =>
                {
                    dsWordsCountDictForOneNode.Add(dsWord, 0);
                });

                // get file content and count the ds words in it
                string fileContent = File.ReadAllText(Utilities.GetFilePathFromNodeLinkTopage(node.LinkToPage));
                string[] fileContentWords = fileContent.Split(' ');
                foreach (string word in fileContentWords)
                {
                    if (dsWordsCountDictForOneNode.ContainsKey(word))
                    {
                        dsWordsCountDictForOneNode[word]++;
                    }
                }

                // remove words with 0 count from words count dictionary
                dsWordsCountDictForOneNode = dsWordsCountDictForOneNode.Where(x => x.Value > 0).ToDictionary(y => y.Key, y => y.Value);

                // only add it to the list if there is at least one ds word
                if (dsWordsCountDictForOneNode.Any())
                {
                    WordCount wordCountForNode = new WordCount()
                    {
                        Index = node.Index,
                        WordsCount = new Dictionary<string, int>(dsWordsCountDictForOneNode)
                    };

                    result.Add(wordCountForNode);
                }
            });

            return result;
        }

        public static void SaveDSWordCountsToJsonFile(KnowledgeGraph knowledgeGraph)
        {
            List<WordCount> dsWordCountsForGraph = GetDataStructureWordCountsFromGraph(knowledgeGraph);
            string jsonFilePath = "../../../dataStructureWordsCountForNodesInGraph.json";
            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(dsWordCountsForGraph));
        }
    }
}
