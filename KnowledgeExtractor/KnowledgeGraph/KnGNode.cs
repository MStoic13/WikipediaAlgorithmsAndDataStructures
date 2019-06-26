using System;
using System.Collections.Generic;
using static KnowledgeExtractor.Utilities;

namespace KnowledgeExtractor
{
    public class KnGNode
    {
        public int Index { get; private set; }

        public OriginalGraphType OriginalGraph { get; private set; }

        public string Label { get; private set; }

        public string HtmlName { get; private set; }

        public Uri LinkToPage { get; private set; }

        public List<KnGNode> Neighbors { get; set; }

        public KnGNode(int index, string label, string htmlName)
        {
            this.Index = index;
            this.Label = label;
            this.HtmlName = htmlName;
            this.Neighbors = new List<KnGNode>();
        }
    }
}
