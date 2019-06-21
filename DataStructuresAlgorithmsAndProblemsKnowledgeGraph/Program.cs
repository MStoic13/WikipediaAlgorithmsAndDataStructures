using System;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;

namespace DataStructuresAlgorithmsAndProblemsKnowledgeGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            List<HtmlNode> relevantNodes = ExtractRelevantNodes("https://en.wikipedia.org/wiki/List_of_algorithms");

            // the graph uses the nodes list's index as its ints
            List<List<int>> graph = ParseNodesListIntoGraph(relevantNodes);
            PrintGraph(graph);

            Console.ReadKey();
        }

        static List<HtmlNode> ExtractRelevantNodes(string wikipediaPageUrl)
        {
            // declaring & loading dom
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc = web.Load(wikipediaPageUrl);

            // get the div in which the page content is
            HtmlNode contentRoot = doc.DocumentNode.SelectNodes("//div").Where(x => x.HasClass("mw-parser-output")).FirstOrDefault();

            List<HtmlNode> results = contentRoot.ChildNodes.Where(x => x.Name == "h2" || x.Name == "h3" || x.Name == "h4" || x.Name == "ul").ToList();
            return results;
        }

        static List<List<int>> ParseNodesListIntoGraph(List<HtmlNode> nodesList)
        {
            List<List<int>> result = new List<List<int>>();

            // you can rely that the nodes are in order h2 > h3 > h4 however ul can come at any point
            // and there are no duplicates
            int mostRecentHIndex = 0;
            for (int index = 0; index < nodesList.Count; index++)
            {
                // each node in the list needs to be in the graph, their index matching in both the list and the graph
                result.Add(new List<int>());

                if (nodesList[index].Name == "h2")
                {
                    mostRecentHIndex = index;
                }
                else if (nodesList[index].Name.StartsWith("h"))
                {
                    result[mostRecentHIndex].Add(index);
                    mostRecentHIndex = index;
                }
                else if (nodesList[index].Name == "ul")
                {
                    result[mostRecentHIndex].Add(index);
                }
            }

            return result;
        }

        static void PrintGraph(List<List<int>> graph)
        {
            int index = 0;
            foreach (List<int> list in graph)
            {
                Console.Write(index + ": ");
                foreach (int x in list)
                {
                    Console.Write(x + ", ");
                }

                Console.WriteLine();
                index++;
            }
        }

        static void PrintNodes(List<HtmlNode> relevantNodes)
        {
            foreach (HtmlNode node in relevantNodes)
            {
                Console.Write(node.Name + " --- ");

                HtmlNode headline = node.ChildNodes.Where(x => x.HasClass("mw-headline")).FirstOrDefault();

                if (headline != null)
                {
                    // h2, h3, h4
                    Console.WriteLine(headline.InnerText);
                }
                else
                {
                    // ul
                    Console.WriteLine();
                }
            }
        }
    }
}
