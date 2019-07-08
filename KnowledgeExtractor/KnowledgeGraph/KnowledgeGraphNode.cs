using System;
using System.Collections.Generic;
using static KnowledgeExtractor.Utilities;

namespace KnowledgeExtractor
{
    public class KnowledgeGraphNode
    {
        public int Index { get; private set; }

        public OriginalGraphType OriginalGraphType { get; private set; }

        public string Label { get; private set; }

        public string HtmlName { get; private set; }

        public Uri LinkToPage { get; private set; }

        public List<KnowledgeGraphNode> Neighbors { get; set; }

        public KnowledgeGraphNode(int index, OriginalGraphType originalGraphType, string label, string htmlName, Uri linkToPage)
        {
            this.Index = index;
            this.OriginalGraphType = originalGraphType;
            this.Label = label;
            this.HtmlName = htmlName;
            this.LinkToPage = linkToPage;
            this.Neighbors = new List<KnowledgeGraphNode>();
        }

        public KnowledgeGraphNode(KnowledgeGraphNode node)
        {
            this.Index = node.Index;
            this.OriginalGraphType = node.OriginalGraphType;
            this.Label = node.Label;
            this.HtmlName = node.HtmlName;
            this.LinkToPage = node.LinkToPage;
            this.Neighbors = new List<KnowledgeGraphNode>(node.Neighbors);
        }
    }
}
