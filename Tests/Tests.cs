using KnowledgeExtractor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
            //graph from page 2
            expectedKnGraph.KnGraph.Add(new KnGNode(19, "H2Title2", "h2"));
            expectedKnGraph.KnGraph[19].Neighbors.Add(new KnGNode(20, "H3Title2", "h3"));
            expectedKnGraph.KnGraph.Add(new KnGNode(20, "H3Title2", "h3"));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(21, "page2li-1", "li"));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(22, "page2li-2", "li"));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(23, "page2li-3", "li"));
            expectedKnGraph.KnGraph[20].Neighbors.Add(new KnGNode(24, "page2li-4", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(21, "page2li-1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(22, "page2li-2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(23, "page2li-3", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(24, "page2li-4", "li"));
            // graph from page 3
            expectedKnGraph.KnGraph.Add(new KnGNode(25, "H2Title3", "h2"));
            expectedKnGraph.KnGraph[25].Neighbors.Add(new KnGNode(26, "H3Title3", "h3"));
            expectedKnGraph.KnGraph.Add(new KnGNode(26, "H3Title3", "h3"));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(27, "page3li-1", "li"));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(28, "page3li-2", "li"));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(29, "page3li-3", "li"));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(35, "page3li-4", "li"));
            expectedKnGraph.KnGraph[26].Neighbors.Add(new KnGNode(36, "page3li-5", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(27, "page3li-1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(28, "page3li-2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(29, "page3li-3", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(35, "page3li-4", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(36, "page3li-5", "li"));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(30, "page3li-3.1", "li"));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(31, "page3li-3.2", "li"));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(32, "page3li-3.3", "li"));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(33, "page3li-3.4", "li"));
            expectedKnGraph.KnGraph[29].Neighbors.Add(new KnGNode(34, "page3li-3.5", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(30, "page3li-2.1.1", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(31, "page3li-2.1.2", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(32, "page3li-2.1.3", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(33, "page3li-2.1.4", "li"));
            expectedKnGraph.KnGraph.Add(new KnGNode(34, "page3li-2.1.5", "li"));

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
