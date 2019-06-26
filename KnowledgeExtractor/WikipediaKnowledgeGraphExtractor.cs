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
            List<HtmlNode> htmlNodes = ExtractRelevantHtmlNodesFromUrl(wikipediaPageUrl);
            KnowledgeGraph result = ParseHtmlNodesIntoKnGraph(htmlNodes);
            return result;
        }

        public static HtmlDocument GetHtmlDocumentFromUrl(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc = web.Load(url);
            return doc;
        }

        public static HtmlDocument GetHtmlDocumentFromHtmlString(string htmlInput)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);
            return doc;
        }

        private static List<HtmlNode> ExtractRelevantHtmlNodesFromHtmlDoc(HtmlDocument htmlDoc)
        {
            // get the div in which the page content is
            HtmlNode contentRoot = htmlDoc.DocumentNode.SelectNodes("//div").Where(x => x.HasClass("mw-parser-output")).FirstOrDefault();

            List<HtmlNode> results = contentRoot.ChildNodes.Where(x => x.Name == "h2" || x.Name == "h3" || x.Name == "h4" || x.Name == "ul").ToList();
            return results;
        }

        public static List<HtmlNode> ExtractRelevantHtmlNodesFromUrl(string wikipediaPageUrl)
        {
            HtmlDocument doc = GetHtmlDocumentFromUrl(wikipediaPageUrl);
            return ExtractRelevantHtmlNodesFromHtmlDoc(doc);
        }

        public static List<HtmlNode> ExtractRelevantHtmlNodesFromHtmlString(string htmlInput)
        {
            HtmlDocument doc = GetHtmlDocumentFromHtmlString(htmlInput);
            return ExtractRelevantHtmlNodesFromHtmlDoc(doc);
        }

        public static KnowledgeGraph ParseHtmlNodesIntoKnGraph(List<HtmlNode> htmlNodes)
        {
            KnowledgeGraph result = new KnowledgeGraph();

            // you can rely that the nodes are in order h2 > h3 > h4 however ul can come at any point
            // and there are no duplicate pieces of knowledge (I mean you'll see more h2, h3, h4 and ul but they each correspond to a different piece of knowledge)
            // list with the last index of h2, h3, and h4 - compute index of this list by taking the h's number - 2
            List<int> mostRecentHIndexes = new List<int>() { 0, 0, 0 };
            // I need this variable to store the last known h for the ul elements since I can't know which h was laste just with the list
            int mostRecentHIndex = 0;
            int index = 0;

            foreach(var node in htmlNodes)
            {
                if (node.Name == "ul")
                {
                    // ul has more nodes in it which need to be indexed and the subtree added here
                    // the ul itself is not a node but a list of nodes, ul is just a placeholder
                    ExtractAndAddUlSubgraphRecursive(graph: result, parentIndex: mostRecentHIndex, nodeToParse: node, nodeToParseIndex: ref index);
                }
                else if (node.Name.StartsWith("h"))
                {
                    // only add h's and li's to the graph, not ul's
                    result.KnGraph.Add(new KnGNode(index, GetNodeHeadlineText(node), node.Name));

                    mostRecentHIndex = index;

                    // turn the h number into int and -2 to get the index for mostRecentHIndexes
                    int hIndex = int.Parse(node.Name[1].ToString()) - 2;
                    mostRecentHIndexes[hIndex] = index;

                    // if it's h3 then add it to the most recent h2 and if it's h4 add it to the most recent h3 and so on if hN add it to h(N-1)
                    if (hIndex > 0)
                    {
                        result.KnGraph[mostRecentHIndexes[hIndex - 1]].Neighbors.Add(new KnGNode(index, GetNodeHeadlineText(node), node.Name));
                    }

                    index++;
                }
            }

            return result;
        }

        public static KnowledgeGraph ExtractKnGraphFromUrl(string url)
        {
            return ParseHtmlNodesIntoKnGraph(ExtractRelevantHtmlNodesFromUrl(url));
        }

        public static KnowledgeGraph ExtractKnGraphFromHtmlInput(string htmlInput)
        {
            return ParseHtmlNodesIntoKnGraph(ExtractRelevantHtmlNodesFromHtmlString(htmlInput));
        }

        public static void ExtractAndAddUlSubgraphRecursive(KnowledgeGraph graph, int parentIndex, HtmlNode nodeToParse, ref int nodeToParseIndex)
        {
            foreach (var child in nodeToParse.ChildNodes.Where(n => n.Name == "li"))
            {
                // and add it as a parent, too
                graph.KnGraph.Add(new KnGNode(index: nodeToParseIndex, label: GetLiNodeLabel(child), htmlName: "li"));

                // 1 li can only have 1 ul in it
                HtmlNode ulNode = child.ChildNodes.Where(node => node.Name == "ul").FirstOrDefault();

                // if the li has an ul in it, go into recursive
                if (ulNode != null)
                {
                    // add the li node to the graph because it will be the parent of its ul's items and increase the index counter
                    graph.KnGraph[parentIndex].Neighbors.Add(new KnGNode(index: nodeToParseIndex, label: GetLiNodeLabel(child), htmlName: "li"));

                    int newParentNodeIndex = nodeToParseIndex;
                    nodeToParseIndex++;
                    ExtractAndAddUlSubgraphRecursive(
                        graph: graph, 
                        parentIndex: newParentNodeIndex, 
                        nodeToParse: ulNode, 
                        nodeToParseIndex: ref nodeToParseIndex);
                }
                else
                {
                    graph.KnGraph[parentIndex].Neighbors.Add(new KnGNode(index: nodeToParseIndex, label: GetLiNodeLabel(child), htmlName: "li"));
                    nodeToParseIndex++;
                }
            }
        }

        private static string GetLiNodeLabel(HtmlNode node)
        {
            string nodeLabel = string.Empty;

            if(node.ChildNodes.Count == 1)
            {
                nodeLabel = node.InnerText;
            }
            else
            {
                // take the first child which has inner text and is not an ul
                HtmlNode childNode = node.ChildNodes.First();
                while (childNode.Name != "ul" && string.IsNullOrWhiteSpace(childNode.InnerText) && childNode.NextSibling != null)
                {
                    childNode = childNode.NextSibling;
                }
                nodeLabel = childNode.InnerText;
            }

            nodeLabel = CleanNodeLabel(nodeLabel);

            return nodeLabel;
        }

        private static string CleanNodeLabel(string nodeLabel)
        {
            string result = nodeLabel;
            result = result.Replace("\n", string.Empty);
            result = result.Replace("\r", string.Empty);
            result = result.Replace("\t", string.Empty);
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
