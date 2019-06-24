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
            string htmlInput = @"
            <div class=""mw-parser-output"">
                <h2></h2>
                <h2></h2>
                <h3></h3>
                <ul></ul>
                <h4></h4>
                <ul></ul>
                <h3></h3>
                <ul></ul>
                <h2></h2>
                <ul></ul>
            </div>";

            KnowledgeGraph expectedKnGraph = new KnowledgeGraph();
            expectedKnGraph.KnGraph.Add(new KnGNode(0, "h2-1", "h2"));
            expectedKnGraph.KnGraph.Add(new KnGNode(1, "h2-2", "h2"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(2, "h3-1", "h3"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(6, "h3-2", "h3"));
            expectedKnGraph.KnGraph.Add(new KnGNode(2, "h3-1", "h3"));
            expectedKnGraph.KnGraph[2].Neighbors.Add(new KnGNode(3, "ul-1", "ul"));
            expectedKnGraph.KnGraph[2].Neighbors.Add(new KnGNode(4, "h4-1", "h4"));
            expectedKnGraph.KnGraph.Add(new KnGNode(3, "ul-1", "ul"));
            expectedKnGraph.KnGraph.Add(new KnGNode(4, "h4-1", "h4"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(5, "ul-2", "ul"));
            expectedKnGraph.KnGraph.Add(new KnGNode(5, "ul-2", "ul"));
            expectedKnGraph.KnGraph.Add(new KnGNode(6, "h3-3", "h4"));
            expectedKnGraph.KnGraph[6].Neighbors.Add(new KnGNode(7, "ul-3", "ul"));
            expectedKnGraph.KnGraph.Add(new KnGNode(7, "ul-3", "ul"));
            expectedKnGraph.KnGraph.Add(new KnGNode(8, "h2-3", "h2"));
            expectedKnGraph.KnGraph[8].Neighbors.Add(new KnGNode(9, "ul-4", "ul"));
            expectedKnGraph.KnGraph.Add(new KnGNode(9, "ul-4", "ul"));
            
            List<HtmlNode> nodes = WKGE.ExtractRelevantHtmlNodesFromHtmlString(htmlInput);
            KnowledgeGraph actualKnGraph = WKGE.ParseHtmlNodesIntoKnGraph(nodes);

            // assert.equal in .net core is smart enough to just compare expectedGraph and graph
            // collectionAssert are equivalent is not smart enough to do this in VS test tools
            // so I have to write it myself

            Assert.AreEqual(expectedKnGraph.KnGraph.Count, actualKnGraph.KnGraph.Count);
            for (int index = 0; index < expectedKnGraph.KnGraph.Count; index++)
            {
                Assert.AreEqual(expectedKnGraph.KnGraph[index].Neighbors.Count, actualKnGraph.KnGraph[index].Neighbors.Count);

                if (expectedKnGraph.KnGraph[index].Neighbors.Count > 0)
                {
                    for (int index2 = 0; index2 < expectedKnGraph.KnGraph[index].Neighbors.Count; index2++)
                    {
                        Assert.AreEqual(expectedKnGraph.KnGraph[index].Neighbors[index2].Index, actualKnGraph.KnGraph[index].Neighbors[index2].Index);
                    }
                }
            }
        }

        [TestMethod]
        public void ExtractAndAddUlSubgraphRecursiveTest()
        {
            string htmlInput = @"
            <div class=""mw-parser-output"">
                <h2>H2Title</h2>
                <h3>H3Title</h3>
                <ul>
                    <li>li-1</li>
                    <li>li-2</li>
                    <li>
                        <p>li-3</p>
                        <ul>
                            <li>li-3.1</li>
                            <li>li-3.2</li>
                            <li>li-3.3</li>
                            <li>
                                <p>li-3.4</p>
                                <ul>
                                    <li>li-3.4.1</li>
                                    <li>li-3.4.2</li>
                                </ul>
                            </li>
                            <li>li-3.5</li>
                            <li>
                                <p>li-3.6</p>
                                <ul>
                                    <li>
                                        <p>li-3.6.1</p>
                                        <ul>
                                            <li>li-3.6.1.1</li>
                                            <li>li-3.6.1.2</li>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                            <li>li-3.7</li>
                        </ul>
                    </li>
                    <li>li-4</li>
                    <li>li-5</li>
                </ul>
            </div>";

            //List<HtmlNode> nodes = WKGE.ExtractRelevantHtmlNodesFromHtmlString(htmlInput);

            // expected graph
            KnowledgeGraph expectedKnGraph = new KnowledgeGraph();
            expectedKnGraph.KnGraph.Add(new KnGNode(0, "H2Title", "h2"));
            expectedKnGraph.KnGraph[0].Neighbors.Add(new KnGNode(1, "H3Ttitle", "h3"));
            expectedKnGraph.KnGraph.Add(new KnGNode(1, "H3Title", "h3"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(2,  "li-1", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(3,  "li-2", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(4,  "li-3", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(17, "li-4", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(18, "li-5", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(2, "li-1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(3, "li-2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(4, "li-3", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(5,  "li-3.1", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(6,  "li-3.2", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(7,  "li-3.3", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(8,  "li-3.4", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(11, "li-3.5", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(12, "li-3.6", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(16, "li-3.7", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(5, "li-3.1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(6, "li-3.2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(7, "li-3.3", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(8, "li-3.4", "li"));
            expectedKnGraph.KnGraph[8].Neighbors.Add(new KnGNode(9, "li-3.4.1", "li"));
            expectedKnGraph.KnGraph[8].Neighbors.Add(new KnGNode(10, "li-3.4.2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(9, "li-3.4.1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(10, "li-3.4.2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(11, "li-3.5", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(12, "li-3.6", "li"));
            expectedKnGraph.KnGraph[12].Neighbors.Add(new KnGNode(13, "li-3.6.1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(13, "li-3.6.1", "li"));
            expectedKnGraph.KnGraph[13].Neighbors.Add(new KnGNode(14, "li-3.6.1.1", "li"));
            expectedKnGraph.KnGraph[13].Neighbors.Add(new KnGNode(15, "li-3.6.1.2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(14, "li-3.6.1.1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(15, "li-3.6.1.2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(16, "li-3.7", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(17, "li-4", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(18, "li-5", "li"));

            KnowledgeGraph actualKnGraph = WKGE.ExtractKnGraphFromHtmlInput(htmlInput);

            Assert.AreEqual(expectedKnGraph.KnGraph.Count, actualKnGraph.KnGraph.Count);
            for (int index = 0; index < expectedKnGraph.KnGraph.Count; index++)
            {
                Assert.AreEqual(expectedKnGraph.KnGraph[index].Neighbors.Count, actualKnGraph.KnGraph[index].Neighbors.Count);

                if (expectedKnGraph.KnGraph[index].Neighbors.Count > 0)
                {
                    for (int index2 = 0; index2 < expectedKnGraph.KnGraph[index].Neighbors.Count; index2++)
                    {
                        Assert.AreEqual(expectedKnGraph.KnGraph[index].Neighbors[index2].Index, actualKnGraph.KnGraph[index].Neighbors[index2].Index);
                    }
                }
            }
        }
    }
}
