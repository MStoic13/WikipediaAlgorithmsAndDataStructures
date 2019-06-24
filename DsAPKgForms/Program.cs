using KnowledgeExtractor;
using System;

using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace DsAPKgForms
{
    static class Program
    {
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
            for (int index = 0; index < knowledgeGraph.KnGraph.Count; index ++)
            {
                graph.AddNode(knowledgeGraph.KnGraph[index].Label);
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
