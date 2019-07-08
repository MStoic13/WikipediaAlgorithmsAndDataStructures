using System.Collections.Generic;

namespace KnowledgeExtractor
{
    public class KnowledgeGraph
    {
        public List<KnowledgeGraphNode> KnGraph { get; set; }

        public KnowledgeGraph()
        {
            this.KnGraph = new List<KnowledgeGraphNode>();
        }
    }
}
