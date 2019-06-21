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
            // PrintNodes(relevantNodes);

            List<Tuple<HtmlNode, List<HtmlNode>>> graph = ParseNodesListIntoGraph(relevantNodes);

            // todo: parse this list into a graph

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

        static List<Tuple<HtmlNode, List<HtmlNode>>> ParseNodesListIntoGraph(List<HtmlNode> nodesList)
        {
            List<Tuple<HtmlNode, List<HtmlNode>>> result = new List<Tuple<HtmlNode, List<HtmlNode>>>();



            return result;
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
