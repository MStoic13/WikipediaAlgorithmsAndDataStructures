using KnowledgeExtractor;
using Microsoft.Msagl.Drawing;
using System;

using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace DsAPKgForms
{
    static class Program
    {
        private static Color ListOfAlgosH2Color = new Color(255, 0, 0);
        private static Color ListOfAlgosH3Color = new Color(255, 140, 140);
        private static Color ListOfAlgosH4Color = new Color(255, 199, 199);
        private static Color ListOfAlgosLiColor = new Color(255, 228, 228);

        private static Color ListOfDataStructuresH2Color = new Color(0, 9, 255);
        private static Color ListOfDataStructuresH3Color = new Color(142, 146, 255);
        private static Color ListOfDataStructuresH4Color = new Color(199, 201, 255);
        private static Color ListOfDataStructuresLiColor = new Color(228, 229, 255);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // get the knowlege graph from wikipedia
            KnowledgeGraph listOfAlgorithmsKnGraph = WKGE.GetWikipediaPageKnowledgeGraph(WKGE.GetWikipediaListOfAlgorithmsPageUrl());
            KnowledgeGraph listOfDataStructuresKnGraph = WKGE.GetWikipediaPageKnowledgeGraph(WKGE.GetWikipediaListOfDataStructuresPageUrl());

            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Graph graph = new Graph("graph");
            
            //add the graph content for list of algorithms 
            Color color = Color.White;
            for (int index = 0; index < listOfAlgorithmsKnGraph.KnGraph.Count; index ++)
            {
                switch (listOfAlgorithmsKnGraph.KnGraph[index].HtmlName)
                {
                    case "h2":
                        color = ListOfAlgosH2Color;
                        break;
                    case "h3":
                        color = ListOfAlgosH3Color;
                        break;
                    case "h4":
                        color = ListOfAlgosH4Color;
                        break;
                    case "li":
                        color = ListOfAlgosLiColor;
                        break;
                }

                graph.AddNode(listOfAlgorithmsKnGraph.KnGraph[index].Label).Attr.FillColor = color;
                listOfAlgorithmsKnGraph.KnGraph[index].Neighbors.ForEach(neighbor => 
                    graph.AddEdge(listOfAlgorithmsKnGraph.KnGraph[index].Label, listOfAlgorithmsKnGraph.KnGraph[neighbor.Index].Label));
            }

            // add the graph content for list of data structures
            for (int index = 0; index < listOfDataStructuresKnGraph.KnGraph.Count; index ++)
            {
                switch (listOfDataStructuresKnGraph.KnGraph[index].HtmlName)
                {
                    case "h2":
                        color = ListOfDataStructuresH2Color;
                        break;
                    case "h3":
                        color = ListOfDataStructuresH3Color;
                        break;
                    case "h4":
                        color = ListOfDataStructuresH4Color;
                        break;
                    case "li":
                        color = ListOfDataStructuresLiColor;
                        break;
                }

                graph.AddNode(listOfDataStructuresKnGraph.KnGraph[index].Label).Attr.FillColor = color;
                listOfDataStructuresKnGraph.KnGraph[index].Neighbors.ForEach(neighbor =>
                    graph.AddEdge(listOfDataStructuresKnGraph.KnGraph[index].Label, listOfDataStructuresKnGraph.KnGraph[neighbor.Index].Label));
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
    }
}
