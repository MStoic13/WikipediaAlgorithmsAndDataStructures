using KnowledgeExtractor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ExtractKnGraphFromHtmlInputOnePageTest()
        {
            // expected graph
            KnowledgeGraph expectedKnGraph = new KnowledgeGraph();
            expectedKnGraph.KnGraph.Add(new KnGNode(0, Utilities.OriginalGraphType.Unknown, "H2Title", "h2", new Uri("about:blank")));
            expectedKnGraph.KnGraph[0].Neighbors.Add(new KnGNode(1, Utilities.OriginalGraphType.Unknown, "H3Ttitle", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(1, Utilities.OriginalGraphType.Unknown, "H3Title", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(2, Utilities.OriginalGraphType.Unknown,  "li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(3, Utilities.OriginalGraphType.Unknown,  "li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(4, Utilities.OriginalGraphType.Unknown, "li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(17, Utilities.OriginalGraphType.Unknown, "li-4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(18, Utilities.OriginalGraphType.Unknown, "li-5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(2, Utilities.OriginalGraphType.Unknown, "li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(3, Utilities.OriginalGraphType.Unknown, "li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(4, Utilities.OriginalGraphType.Unknown, "li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(5, Utilities.OriginalGraphType.Unknown,  "li-3.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(6, Utilities.OriginalGraphType.Unknown,  "li-3.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(7, Utilities.OriginalGraphType.Unknown,  "li-3.3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(8, Utilities.OriginalGraphType.Unknown, "li-3.4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(11, Utilities.OriginalGraphType.Unknown, "li-3.5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(12, Utilities.OriginalGraphType.Unknown, "li-3.6", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(16, Utilities.OriginalGraphType.Unknown, "li-3.7", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(5, Utilities.OriginalGraphType.Unknown, "li-3.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(6, Utilities.OriginalGraphType.Unknown, "li-3.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(7, Utilities.OriginalGraphType.Unknown, "li-3.3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(8, Utilities.OriginalGraphType.Unknown, "li-3.4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[8].Neighbors.Add(new KnGNode(9, Utilities.OriginalGraphType.Unknown, "li-3.4.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[8].Neighbors.Add(new KnGNode(10, Utilities.OriginalGraphType.Unknown, "li-3.4.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(9, Utilities.OriginalGraphType.Unknown, "li-3.4.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(10, Utilities.OriginalGraphType.Unknown, "li-3.4.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(11, Utilities.OriginalGraphType.Unknown, "li-3.5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(12, Utilities.OriginalGraphType.Unknown, "li-3.6", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[12].Neighbors.Add(new KnGNode(13, Utilities.OriginalGraphType.Unknown, "li-3.6.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(13, Utilities.OriginalGraphType.Unknown, "li-3.6.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[13].Neighbors.Add(new KnGNode(14, Utilities.OriginalGraphType.Unknown, "li-3.6.1.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[13].Neighbors.Add(new KnGNode(15, Utilities.OriginalGraphType.Unknown, "li-3.6.1.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(14, Utilities.OriginalGraphType.Unknown, "li-3.6.1.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(15, Utilities.OriginalGraphType.Unknown, "li-3.6.1.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(16, Utilities.OriginalGraphType.Unknown, "li-3.7", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(17, Utilities.OriginalGraphType.Unknown, "li-4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(18, Utilities.OriginalGraphType.Unknown, "li-5", "li", new Uri("about:blank")));
            
            KnowledgeGraph actualKnGraph = WKGE.ExtractKnGraphFromHtmlFile(filePath: "./TestInputs/page1.html");

            AssertKnowledgegraphsAreEqual(expectedKnGraph, actualKnGraph);
        }

        [TestMethod]
        public void ExtractKnGraphFromHtmlInputThreePagesTest()
        {
            // html input files
            List<string> htmlFiles = new List<string>()
            {
                "./TestInputs/page1.html",
                "./TestInputs/page2.html",
                "./TestInputs/page3.html"
            };

            // expected graph
            KnowledgeGraph expectedKnGraph = new KnowledgeGraph();
            // graph from page 1
            expectedKnGraph.KnGraph.Add(new KnGNode(0, Utilities.OriginalGraphType.Unknown, "H2Title", "h2", new Uri("about:blank")));
            expectedKnGraph.KnGraph[0].Neighbors.Add(new KnGNode(1, Utilities.OriginalGraphType.Unknown, "H3Ttitle", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(1, Utilities.OriginalGraphType.Unknown, "H3Title", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(2, Utilities.OriginalGraphType.Unknown, "li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(3, Utilities.OriginalGraphType.Unknown, "li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(4, Utilities.OriginalGraphType.Unknown, "li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(17, Utilities.OriginalGraphType.Unknown, "li-4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[1].Neighbors.Add(new KnGNode(18, Utilities.OriginalGraphType.Unknown, "li-5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(2, Utilities.OriginalGraphType.Unknown, "li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(3, Utilities.OriginalGraphType.Unknown, "li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(4, Utilities.OriginalGraphType.Unknown, "li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(5, Utilities.OriginalGraphType.Unknown, "li-3.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(6, Utilities.OriginalGraphType.Unknown, "li-3.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(7, Utilities.OriginalGraphType.Unknown, "li-3.3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(8, Utilities.OriginalGraphType.Unknown, "li-3.4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(11, Utilities.OriginalGraphType.Unknown, "li-3.5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(12, Utilities.OriginalGraphType.Unknown, "li-3.6", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[4].Neighbors.Add(new KnGNode(16, Utilities.OriginalGraphType.Unknown, "li-3.7", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(5, Utilities.OriginalGraphType.Unknown, "li-3.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(6, Utilities.OriginalGraphType.Unknown, "li-3.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(7, Utilities.OriginalGraphType.Unknown, "li-3.3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(8, Utilities.OriginalGraphType.Unknown, "li-3.4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[8].Neighbors.Add(new KnGNode(9, Utilities.OriginalGraphType.Unknown, "li-3.4.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[8].Neighbors.Add(new KnGNode(10, Utilities.OriginalGraphType.Unknown, "li-3.4.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(9, Utilities.OriginalGraphType.Unknown, "li-3.4.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(10, Utilities.OriginalGraphType.Unknown, "li-3.4.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(11, Utilities.OriginalGraphType.Unknown, "li-3.5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(12, Utilities.OriginalGraphType.Unknown, "li-3.6", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[12].Neighbors.Add(new KnGNode(13, Utilities.OriginalGraphType.Unknown, "li-3.6.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(13, Utilities.OriginalGraphType.Unknown, "li-3.6.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[13].Neighbors.Add(new KnGNode(14, Utilities.OriginalGraphType.Unknown, "li-3.6.1.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[13].Neighbors.Add(new KnGNode(15, Utilities.OriginalGraphType.Unknown, "li-3.6.1.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(14, Utilities.OriginalGraphType.Unknown, "li-3.6.1.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(15, Utilities.OriginalGraphType.Unknown, "li-3.6.1.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(16, Utilities.OriginalGraphType.Unknown, "li-3.7", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(17, Utilities.OriginalGraphType.Unknown, "li-4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(18, Utilities.OriginalGraphType.Unknown, "li-5", "li", new Uri("about:blank")));
            //graph from page 2
            expectedKnGraph.KnGraph.Add(new KnGNode(19, Utilities.OriginalGraphType.Unknown, "H2Title2", "h2", new Uri("about:blank")));
            expectedKnGraph.KnGraph[19].Neighbors.Add(new KnGNode(20, Utilities.OriginalGraphType.Unknown, "H3Title2", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(20, Utilities.OriginalGraphType.Unknown, "H3Title2", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(21, Utilities.OriginalGraphType.Unknown, "page2li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(22, Utilities.OriginalGraphType.Unknown, "page2li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(23, Utilities.OriginalGraphType.Unknown, "page2li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(24, Utilities.OriginalGraphType.Unknown, "page2li-4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(21, Utilities.OriginalGraphType.Unknown, "page2li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(22, Utilities.OriginalGraphType.Unknown, "page2li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(23, Utilities.OriginalGraphType.Unknown, "page2li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(24, Utilities.OriginalGraphType.Unknown, "page2li-4", "li", new Uri("about:blank")));
            // graph from page 3
            expectedKnGraph.KnGraph.Add(new KnGNode(25, Utilities.OriginalGraphType.Unknown, "H2Title3", "h2", new Uri("about:blank")));
            expectedKnGraph.KnGraph[25].Neighbors.Add(new KnGNode(26, Utilities.OriginalGraphType.Unknown, "H3Title3", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(26, Utilities.OriginalGraphType.Unknown, "H3Title3", "h3", new Uri("about:blank")));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(27, Utilities.OriginalGraphType.Unknown, "page3li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(28, Utilities.OriginalGraphType.Unknown, "page3li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(29, Utilities.OriginalGraphType.Unknown, "page3li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(35, Utilities.OriginalGraphType.Unknown, "page3li-4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(36, Utilities.OriginalGraphType.Unknown, "page3li-5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(27, Utilities.OriginalGraphType.Unknown, "page3li-1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(28, Utilities.OriginalGraphType.Unknown, "page3li-2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(29, Utilities.OriginalGraphType.Unknown, "page3li-3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(35, Utilities.OriginalGraphType.Unknown, "page3li-4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(36, Utilities.OriginalGraphType.Unknown, "page3li-5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(30, Utilities.OriginalGraphType.Unknown, "page3li-3.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(31, Utilities.OriginalGraphType.Unknown, "page3li-3.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(32, Utilities.OriginalGraphType.Unknown, "page3li-3.3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(33, Utilities.OriginalGraphType.Unknown, "page3li-3.4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(34, Utilities.OriginalGraphType.Unknown, "page3li-3.5", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(30, Utilities.OriginalGraphType.Unknown, "page3li-2.1.1", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(31, Utilities.OriginalGraphType.Unknown, "page3li-2.1.2", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(32, Utilities.OriginalGraphType.Unknown, "page3li-2.1.3", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(33, Utilities.OriginalGraphType.Unknown, "page3li-2.1.4", "li", new Uri("about:blank")));
            expectedKnGraph.KnGraph.Add(new KnGNode(34, Utilities.OriginalGraphType.Unknown, "page3li-2.1.5", "li", new Uri("about:blank")));

            // compute actual graph from files
            KnowledgeGraph actualKnGraph = WKGE.ExtractKnGraphFromHtmlFiles(htmlFiles);

            AssertKnowledgegraphsAreEqual(expectedKnGraph, actualKnGraph);
        }

        private void AssertKnowledgegraphsAreEqual(KnowledgeGraph expectedKnGraph, KnowledgeGraph actualKnGraph)
        {
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
