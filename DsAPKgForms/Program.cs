using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DsAPKgForms
{
    static class Program
    {
        public static List<string> WikipediaPagesToParse = new List<string>()
        {
            "https://en.wikipedia.org/wiki/List_of_algorithms",
            "https://en.wikipedia.org/wiki/List_of_data_structures"
        };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // get the knowlege graph from wikipedia
            Tuple<List<List<int>>, List<string>> knowledgeGraph = GetWikipediaPageKnowledgeGraph(WikipediaPagesToParse[0]);

            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            for (int index = 0; index < knowledgeGraph.Item1.Count; index ++)
            {
                graph.AddNode(knowledgeGraph.Item2[index]);
                knowledgeGraph.Item1[index].ForEach(neighbor => graph.AddEdge(knowledgeGraph.Item2[index], knowledgeGraph.Item2[neighbor]));
            }
            
            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog();
        }

        public static Tuple<List<List<int>>, List<string>> GetWikipediaPageKnowledgeGraph(string wikipediaPageUrl)
        {
            List<string> nodeNames = new List<string>();

            List<HtmlNode> relevantNodes = ExtractRelevantNodes(wikipediaPageUrl);
            relevantNodes.ForEach(node => nodeNames.Add(GetNodeHeadlineText(node)));
            List<List<int>> graph = ParseNodesListIntoGraph(relevantNodes);

            Tuple<List<List<int>>, List<string>> result = new Tuple<List<List<int>>, List<string>>(graph, nodeNames);
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

        public static void PrintGraph(List<List<int>> graph)
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

        public static void PrintGraphWithNames(List<List<int>> graph, List<string> nodeNames)
        {
            // we'll use DFS to print the names according to the graph and with indentation
            List<bool> visited = new List<bool>();
            graph.ForEach(x => visited.Add(false));

            for (int i = 0; i < graph.Count; i++)
            {
                if (!visited[i])
                {
                    DFSRecursive(graph, visited, i, nodeNames, 0);
                }
            }
        }

        public static void DFSRecursive(List<List<int>> graph, List<bool> visited, int currentNode, List<string> nodeNames, int level)
        {
            //Console.Write(currentNode + " - " + nodes[currentNode].Name);
            Console.Write(nodeNames[currentNode]);
            Console.WriteLine();
            visited[currentNode] = true;
            level++;
            graph[currentNode].ForEach(x => {
                if (!visited[x])
                {
                    PrintLevelSpaces(level);
                    DFSRecursive(graph, visited, x, nodeNames, level);
                }
            });
            level--;
        }

        private static void PrintLevelSpaces(int level)
        {
            for (int i = 1; i <= level; i++)
            {
                Console.Write("    ");
            }
        }

        public static void PrintNodes(List<HtmlNode> relevantNodes)
        {
            foreach (HtmlNode node in relevantNodes)
            {
                Console.Write(node.Name + " --- " + GetNodeHeadlineText(node));
            }
        }

        public static string GetNodeHeadlineText(HtmlNode node)
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
