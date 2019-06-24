using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnowledgeExtractor
{
    public static class WikipediaKnowledgeGraphExtractor
    {
        private static List<string> WikipediaPagesToParse = new List<string>()
        {
            "https://en.wikipedia.org/wiki/List_of_algorithms",
            "https://en.wikipedia.org/wiki/List_of_data_structures"
        };

        public static string GetWikipediaListOfAlgorithmsPageUrl()
        {
            return WikipediaPagesToParse[0];
        }

        public static string GetWikipediaListOfDataStructuresPageUrl()
        {
            return WikipediaPagesToParse[1];
        }

        public static KnowledgeGraph GetWikipediaPageKnowledgeGraph(string wikipediaPageUrl)
        {
            List<string> nodeNames = new List<string>();

            List<HtmlNode> relevantNodes = ExtractRelevantNodes(wikipediaPageUrl);
            relevantNodes.ForEach(node => nodeNames.Add(GetNodeHeadlineText(node)));
            List<List<int>> graph = ParseNodesListIntoGraph(relevantNodes);

            KnowledgeGraph result = new KnowledgeGraph()
            {
                GraphSkeleton = graph,
                NodeNames = nodeNames
            };

            return result;
        }

        public static List<HtmlNode> ExtractRelevantNodes(string wikipediaPageUrl)
        {
            // declaring & loading dom
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = web.Load(wikipediaPageUrl);

            // get the div in which the page content is
            HtmlNode contentRoot = doc.DocumentNode.SelectNodes("//div").Where(x => x.HasClass("mw-parser-output")).FirstOrDefault();

            List<HtmlNode> results = contentRoot.ChildNodes.Where(x => x.Name == "h2" || x.Name == "h3" || x.Name == "h4" || x.Name == "ul").ToList();
            return results;
        }

        public static List<List<int>> ParseNodesListIntoGraph(List<HtmlNode> nodesList)
        {
            List<List<int>> result = new List<List<int>>();

            // you can rely that the nodes are in order h2 > h3 > h4 however ul can come at any point
            // and there are no duplicate pieces of knowledge (I mean you'll see more h2, h3, h4 and ul but they each correspond to a different piece of knowledge)

            // list with the last index of h2, h3, and h4 - compute index of this list by taking the h's number - 2
            List<int> mostRecentHIndexes = new List<int>() { 0, 0, 0 };
            // I need this variable to store the last known h for the ul elements since I can't know which h was laste just with the list
            int mostRecentHIndex = 0;
            int index = 0;

            foreach(var node in nodesList)
            {
                // each node in the list needs to be in the graph, their index matching in both the list and the graph
                result.Add(new List<int>());

                if (node.Name == "ul")
                {
                    // ul has more nodes in it which need to be indexed and the subtree added here
                    // the ul itself is not a node but a list of nodes, ul is just a placeholder
                    
                    result[mostRecentHIndex].Add(index);

                    //ExtractAndAddUlSubgraphRecursive(graph:result, parentIndex:mostRecentHIndex, nodeToParse: node, nodeToParseIndex:ref index);
                }
                else if (node.Name.StartsWith("h"))
                {
                    mostRecentHIndex = index;

                    // turn the h number into int and -2 to get the index for mostRecentHIndexes
                    int hIndex = int.Parse(node.Name[1].ToString()) - 2;
                    mostRecentHIndexes[hIndex] = index;

                    // if it's h3 then add it to the most recent h2 and if it's h4 add it to the most recent h3 and so on if hN add it to h(N-1)
                    if (hIndex > 0)
                    {
                        result[mostRecentHIndexes[hIndex - 1]].Add(index);
                    }
                }

                index++;
            }

            return result;
        }

        private static void ExtractAndAddUlSubgraphRecursive(List<List<int>> graph, int parentIndex, HtmlNode nodeToParse, ref int nodeToParseIndex)
        {
            
        }

        private static string GetNodeHeadlineText(HtmlNode node)
        {
            HtmlNode headline = node.ChildNodes.Where(x => x.HasClass("mw-headline")).FirstOrDefault();
            if (headline != null)
            {
                // h2, h3, h4
                return headline.InnerText;
            }
            else
            {
                // ul
                return node.Name;
            }
        }
    }
}
