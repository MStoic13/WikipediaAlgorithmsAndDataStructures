using KnowledgeExtractor;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;
using System.IO;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ExtractKnGraphFromHtmlInputOnePageTest()
        {
            string htmlInput = @"
            <div class="" mw-parser-output"">
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

            // todo: need to finish fixing this. it's getting content root null for some reason in ExtractRelevantHtmlNodesFromHtmlDoc
            // even if the parsed text is correct. so it can reach the file and get its content.
            // KnowledgeGraph actualKnGraph = WKGE.ExtractKnGraphFromHtmlFile(filePath: "./TestInputs/page1.html");
            // this also doesn't work. same error. smth is happening when it's in a file
            // WKGE.ExtractKnGraphFromHtmlInput(File.ReadAllText("./TestInputs/page1.html"));

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

        [TestMethod]
        public void ExtractKnGraphFromHtmlInputThreePagesTest()
        {
            string htmlInput1 = @"
            <div class="" mw-parser-output"">
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

            string htmlInput2 = @"
            <div class="" mw-parser-output"">
                <h2>H2Title2</h2>
                <h3>H3Title2</h3>
                <ul>
                    <li>page2li-1</li>
                    <li>page2li-2</li>
                    <li>page2li-4</li>
                    <li>page2li-5</li>
                </ul>
            </div>";

            string htmlInput3 = @"
            <div class="" mw-parser-output"">
                <h2>H2Title3</h2>
                <h3>H3Title3</h3>
                <ul>
                    <li>page3li-1</li>
                    <li>page3li-2</li>
                    <li>
                        <p>page3li-3</p>
                        <ul>
                            <li>page3li-3.1</li>
                            <li>page3li-3.2</li>
                            <li>page3li-3.3</li>
                            <li>page3li-3.5</li>
                            <li>page3li-3.7</li>
                        </ul>
                    </li>
                    <li>page3li-4</li>
                    <li>page3li-5</li>
                </ul>
            </div>";

            // expected graph
            KnowledgeGraph expectedKnGraph = new KnowledgeGraph();
            expectedKnGraph.KnGraph.Add(new KnGNode(0, "H2Title", "h2"));
            expectedKnGraph.KnGraph[0].Neighbors.Add(new KnGNode(1, "H3Ttitle", "h3"));
            expectedKnGraph.KnGraph.Add(new KnGNode(1, "H3Title", "h3"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(2, "li-1", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(3, "li-2", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(4, "li-3", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(17, "li-4", "li"));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(18, "li-5", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(2, "li-1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(3, "li-2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(4, "li-3", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(5, "li-3.1", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(6, "li-3.2", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(7, "li-3.3", "li"));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(8, "li-3.4", "li"));
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
            // todo: add to this graph the other 2 resulting graphs

            List<string> htmlInputs = new List<string>() { htmlInput1, htmlInput2, htmlInput3 };
            KnowledgeGraph actualKnGraph = WKGE.ExtractKnGraphFromHtmlInputs(htmlInputs);
        }
    }
}
