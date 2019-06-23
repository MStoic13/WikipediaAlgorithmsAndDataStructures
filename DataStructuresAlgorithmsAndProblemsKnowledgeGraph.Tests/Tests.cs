using System.Collections.Generic;
using Xunit;
using HtmlAgilityPack;

namespace DataStructuresAlgorithmsAndProblemsKnowledgeGraph.Tests
{
    public class Tests
    {
        [Fact]
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

            List<List<int>> graph = Program.ParseNodesListIntoGraph(nodes);

            Assert.Equal(expectedGraph, graph);
        }

        private HtmlNode CreateNodeWithName(string name)
        {
            HtmlNode node = new HtmlNode(HtmlNodeType.Text, new HtmlDocument(), 0);
            node.Name = name;
            return node;
        }
    }
}
