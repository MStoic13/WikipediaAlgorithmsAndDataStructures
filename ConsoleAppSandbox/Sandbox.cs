using HtmlAgilityPack;
using KnowledgeExtractor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WKGE = KnowledgeExtractor.WikipediaKnowledgeGraphExtractor;

namespace ConsoleAppSandbox
{
    class Sandbox
    {
        static void Main(string[] args)
        {
            KnowledgeGraph knowledgeGraph = WKGE.ExtractKnGraphFromUris(WKGE.WikipediaPagesToParse);

            // Console.WriteLine(Utilities.FormatKnGraphIndexAndLabelsForPrinting(knowledgeGraph));
            // Console.WriteLine();

            // Console.WriteLine(Utilities.FormatKnGraphLabelsForPrinting(knowledgeGraph));
            // Console.WriteLine();

            // Console.WriteLine(Utilities.FormatKnGraphIndexesForPrinting(knowledgeGraph));

            // Utilities.DownloadAllPagesInKnGraph();

            // Utilities.SaveDSWordCountsToJsonFile(knowledgeGraph);

            Console.ReadKey();
        }
    }
}
