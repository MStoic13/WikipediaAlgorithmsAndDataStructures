using KnowledgeExtractor;
using System;

using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace ConsoleAppSandbox
{
    class Sandbox
    {
        static void Main(string[] args)
        {
            KnowledgeGraph knowledgeGraph = WKGE.GetWikipediaPageKnowledgeGraph(WKGE.GetWikipediaListOfAlgorithmsPageUrl());

            Console.WriteLine(knowledgeGraph.FormatGraphWithNamesForPrinting());
            Console.WriteLine(knowledgeGraph.FormatGraphIntsForPrinting());

            Console.ReadKey();
        }
    }
}
