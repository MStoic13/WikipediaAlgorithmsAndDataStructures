using KnowledgeExtractor;
using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using static KnowledgeExtractor.Utilities;
using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace DsAPKgForms
{
    static class Program
    {
        private static Dictionary<OriginalGraphType, Dictionary<string, Color>> GraphNodeColors = new Dictionary<OriginalGraphType, Dictionary<string, Color>>()
        {
            // white
            { OriginalGraphType.Unknown, new Dictionary<string, Color>()
                {
                    { "h2", new Color(255, 255, 255) },
                    { "h3", new Color(255, 255, 255) },
                    { "h4", new Color(255, 255, 255) },
                    { "li", new Color(255, 255, 255) }
                } },
            // shades of red
            { OriginalGraphType.AlgorithmsKnGraph, new Dictionary<string, Color>()
                {
                    { "h2", new Color(255, 0, 0) },
                    { "h3", new Color(255, 140, 140) },
                    { "h4", new Color(255, 199, 199) },
                    { "li", new Color(255, 228, 228) }
                } },
            // shades of blue
            { OriginalGraphType.DataStructuresKnGraph, new Dictionary<string, Color>()
                {
                    { "h2", new Color(0, 9, 255) },
                    { "h3", new Color(142, 146, 255) },
                    { "h4", new Color(199, 201, 255) },
                    { "li", new Color(228, 229, 255) }
                } }
        };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // get the knowlege graph from all wikipedia Uris in WKGE
            KnowledgeGraph knowledgeGraph = WKGE.ExtractKnGraphFromUris(WKGE.WikipediaPagesToParse);

            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Graph graph = new Graph("graph");

            //add the graph content for list of algorithms 
            for (int index = 0; index < knowledgeGraph.KnGraph.Count; index++)
            {
                KnGNode node = knowledgeGraph.KnGraph[index];

                // add node and set its color
                graph.AddNode(node.Label).Attr.FillColor = GraphNodeColors[node.OriginalGraphType][node.HtmlName];

                // add all neighbors as edges
                node.Neighbors.ForEach(neighbor =>
                    graph.AddEdge(node.Label, knowledgeGraph.KnGraph[neighbor.Index].Label));
            }
            
            // use the MDS rendering method
            viewer.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.MDS;

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
