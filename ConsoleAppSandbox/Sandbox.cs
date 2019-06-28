using KnowledgeExtractor;
using System;
using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace ConsoleAppSandbox
{
    class Sandbox
    {
        static void Main(string[] args)
        {
            KnowledgeGraph knowledgeGraph = WKGE.ExtractKnGraphFromUris(WKGE.WikipediaPagesToParse);

            Console.WriteLine(Utilities.FormatKnGraphIndexAndLabelsForPrinting(knowledgeGraph));
            Console.WriteLine();

            Console.WriteLine(Utilities.FormatKnGraphLabelsForPrinting(knowledgeGraph));
            Console.WriteLine();

            Console.WriteLine(Utilities.FormatKnGraphIndexesForPrinting(knowledgeGraph));

            Console.ReadKey();
        }
    }
}
