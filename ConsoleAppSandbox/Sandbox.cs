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

            Console.WriteLine(Utilities.FormatKnGraphLabelsForPrinting(knowledgeGraph));
            Console.WriteLine();
            Console.WriteLine(Utilities.FormatKnGraphIndexAndLabelsForPrinting(knowledgeGraph));
            Console.WriteLine();
            Console.WriteLine(Utilities.FormatKnGraphIndexesForPrinting(knowledgeGraph));

            Console.ReadKey();
        }
    }
}
