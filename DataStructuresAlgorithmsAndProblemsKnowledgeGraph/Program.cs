using System;
using HtmlAgilityPack;
using System.Linq;

namespace DataStructuresAlgorithmsAndProblemsKnowledgeGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtractH2H3H4UlNodes("https://en.wikipedia.org/wiki/List_of_algorithms");
            Console.ReadKey();
        }

        static void PrintNodes(HtmlNode contentRoot)
        {
            foreach (HtmlNode node in contentRoot.ChildNodes.Where(x => x.Name == "h2" || x.Name == "h3" || x.Name == "h4" || x.Name == "ul"))
            {
                Console.Write(node.Name + " --- ");
                Console.WriteLine(node.InnerText);
            }
        }

        static void ExtractH2H3H4UlNodes(string URL)
        {
            // declaring & loading dom
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc = web.Load(URL);

            // get the div in which the page content is
            HtmlNode contentRoot = doc.DocumentNode.SelectNodes("//div").Where(x => x.HasClass("mw-parser-output")).FirstOrDefault();

            foreach (HtmlNode node in contentRoot.ChildNodes.Where(x => x.Name == "h2" || x.Name == "h3" || x.Name == "h4" || x.Name == "ul"))
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
