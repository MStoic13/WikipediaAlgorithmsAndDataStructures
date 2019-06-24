using System.Collections.Generic;

namespace KnowledgeExtractor
{
    public class KnowledgeGraph
    {
        public List<KnGNode> KnGraph { get; set; }

        public KnowledgeGraph()
        {
            this.KnGraph = new List<KnGNode>();
        }
    }
}
