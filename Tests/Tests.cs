using KnowledgeExtractor;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ParseNodesListIntoGraphTest()
        {
            List<HtmlNode> nodes = new List<HtmlNode>();
            nodes.Add(CreateNodeWithName("h2"));
            nodes.Add(CreateNodeWithName("h2"));
            nodes.Add(CreateNodeWithName("h3"));
            nodes.Add(CreateNodeWithName("ul"));
            nodes.Add(CreateNodeWithName("h4"));
            nodes.Add(CreateNodeWithName("ul"));
            nodes.Add(CreateNodeWithName("h3"));
            nodes.Add(CreateNodeWithName("ul"));
            nodes.Add(CreateNodeWithName("h2"));
            nodes.Add(CreateNodeWithName("ul"));

            List<List<int>> expectedGraph = new List<List<int>>()
            {
                new List<int>(),
                new List<int>() { 2, 6 },
                new List<int>() { 3, 4 },
                new List<int>(),
                new List<int>() { 5 },
                new List<int>(),
                new List<int>() { 7 },
                new List<int>(),
                new List<int>() { 9 },
                new List<int>()
            };

            List<List<int>> graph = WikipediaKnowledgeGraphExtractor.ParseHtmlNodesIntoKnGraph(nodes);

            // assert.equal in .net core is smart enough to just compare expectedGraph and graph
            // collectionAssert are equivalent is not smart enough to do this in VS test tools
            // so I have to write it myself

            Assert.AreEqual(expectedGraph.Count, graph.Count);
            for(int index = 0; index < expectedGraph.Count; index++)
            {
                Assert.AreEqual(expectedGraph[index].Count, graph[index].Count);

                if(expectedGraph[index].Count > 0)
                {
                    for(int index2 = 0; index2 < expectedGraph[index].Count; index2++)
                    {
                        Assert.AreEqual(expectedGraph[index][index2], graph[index][index2]);
                    }
                }
            }
        }

        private HtmlNode CreateNodeWithName(string name)
        {
            HtmlNode node = new HtmlNode(HtmlNodeType.Text, new HtmlDocument(), 0);
            node.Name = name;
            return node;
        }

        //[TestMethod]
        //public void ExtractAndAddUlSubgraphRecursiveTest()
        //{
        //    string htmlInput = @"
        //    <h3>Title</h3>
        //    <ul>
        //        <li>li-1</li>
        //        <li>li-2</li>
        //        <li>
        //            <p>li-3</p>
        //            <ul>
        //                <li>li-3.1</li>
        //                <li>li-3.2</li>
        //                <li>li-3.3</li>
        //                <li>
        //                    <p>li-3.4</p>
        //                    <ul>
        //                        <li>li-3.4.1</li>
        //                        <li>li-3.4.2</li>
        //                    </ul>
        //                </li>
        //                <li>li-3.5</li>
        //                <li>
        //                    <p>li-3.6</p>
        //                    <ul>
        //                        <li>
        //                            <p>li-3.6.1</p>
        //                            <ul>
        //                                <li>li-3.6.1.1</li>
        //                                <li>li-3.6.1.2</li>
        //                            </ul>
        //                        </li>
        //                    </ul>
        //                </li>
        //                <li>li-3.7</li>
        //            </ul>
        //        </li>
        //        <li>li-4</li>
        //        <li>li-5</li>
        //    </ul>";

        //    // parse the html input
        //    HtmlDocument doc = new HtmlDocument();
        //    doc.LoadHtml(htmlInput);
        //    HtmlNode rootUl = doc.DocumentNode.FirstChild.NextSibling; // first child is h3, next sibling is the ul

        //    // expected graph
        //    List<List<KnGNode>> expectedGraph = new List<List<KnGNode>>()
        //    {
        //        new List<KnGNode>() { }, // index 0 - h3
        //        new List<KnGNode>() { }, // index 1
        //        new List<KnGNode>() { }, // index 2
        //        new List<KnGNode>()
        //        {
        //            new KnGNode(4,  "li-3.1", "li"),
        //            new KnGNode(5,  "li-3.2", "li"),
        //            new KnGNode(6,  "li-3.3", "li"),
        //            new KnGNode(7,  "li-3.4", "li"),
        //            new KnGNode(10,  "li-3.5", "li"),
        //            new KnGNode(11, "li-3.6", "li"),
        //            new KnGNode(15, "li-3.7", "li")
        //        }, // index 3
        //        new List<KnGNode>() { }, // index 4
        //        new List<KnGNode>() { }, // index 5
        //        new List<KnGNode>() { }, // index 6
        //        new List<KnGNode>()
        //        {
        //            new KnGNode(8,  "li-3.4.1", "li"),
        //            new KnGNode(9,  "li-3.4.2", "li")
        //        }, // index 7
        //        new List<KnGNode>() { }, // index 8
        //        new List<KnGNode>() { }, // index 9
        //        new List<KnGNode>() { }, // index 10
        //        new List<KnGNode>()
        //        {
        //            new KnGNode(12,  "li-3.6", "li")
        //        }, // index 11
        //        new List<KnGNode>()
        //        {
        //            new KnGNode(13,  "li-3.6.1.1", "li"),
        //            new KnGNode(14,  "li-3.6.1.2", "li")
        //        }, // index 12
        //        new List<KnGNode>() { }, // index 13
        //        new List<KnGNode>() { }, // index 14
        //        new List<KnGNode>() { }, // index 15
        //        new List<KnGNode>() { }, // index 16
        //        new List<KnGNode>() { }  // index 17
        //    };

        //    // resulting graph
        //    List<List<KnGNode>> resultingGraph = new List<List<KnGNode>>();
        //    resultingGraph.Add(new List<KnGNode>());
        //    int index = 1;
        //    WKGE.ExtractAndAddUlSubgraphRecursive(graph:resultingGraph, parentIndex:0, nodeToParse:rootUl, nodeToParseIndex:ref index);

        //    // assert
        //    Assert.AreEqual(index, 18);
        //}
    }
}
