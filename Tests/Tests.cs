using KnowledgeExtractor;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            List<List<int>> graph = WikipediaKnowledgeGraphExtractor.ParseNodesListIntoGraph(nodes);

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
    }
}
