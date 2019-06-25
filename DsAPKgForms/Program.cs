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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // get the knowlege graph from wikipedia
            KnowledgeGraph knowledgeGraph = WKGE.GetWikipediaPageKnowledgeGraph(WKGE.GetWikipediaListOfAlgorithmsPageUrl());

            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 

            Color color = Color.White;
            for (int index = 0; index < knowledgeGraph.KnGraph.Count; index ++)
            {
                switch (knowledgeGraph.KnGraph[index].HtmlName)
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

                graph.AddNode(knowledgeGraph.KnGraph[index].Label).Attr.FillColor = color;
                knowledgeGraph.KnGraph[index].Neighbors.ForEach(neighbor => graph.AddEdge(knowledgeGraph.KnGraph[index].Label, knowledgeGraph.KnGraph[neighbor.Index].Label));
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
