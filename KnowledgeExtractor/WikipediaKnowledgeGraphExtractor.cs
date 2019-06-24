using HtmlAgilityPack;
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
            // and there are no duplicates

            // list with the last index of h2, h3, and h4 - compute index of this list by taking the h's number - 2
            List<int> mostRecentHIndexes = new List<int>() { 0, 0, 0 };
            // I need this variable to store the last known h for the ul elements since I can't know which h was laste just with the list
            int mostRecentHIndex = 0;

            for (int index = 0; index < nodesList.Count; index++)
            {
                // each node in the list needs to be in the graph, their index matching in both the list and the graph
                result.Add(new List<int>());

                if (nodesList[index].Name == "ul")
                {
                    result[mostRecentHIndex].Add(index);
                }
                else if (nodesList[index].Name.StartsWith("h"))
                {
                    mostRecentHIndex = index;

                    // turn the h number into int and -2 to get the index for mostRecentHIndexes
                    int hIndex = int.Parse(nodesList[index].Name[1].ToString()) - 2;
                    mostRecentHIndexes[hIndex] = index;

                    // if it's h3 then add it to the most recent h2 and if it's h4 add it to the most recent h3 and so on if hN add it to h(N-1)
                    if (hIndex > 0)
                    {
                        result[mostRecentHIndexes[hIndex - 1]].Add(index);
                    }
                }
            }

            return result;
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
