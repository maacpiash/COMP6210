using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace RTree
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please specify two file paths like this: dotnet run [dataset-file] [query-file]");
                return;
            }
            string datasetFilePath = args[0];
            string queryFilePath = args[1];

            // Read dataset

            string[] dataLines = File.ReadAllLines(datasetFilePath);
            if (dataLines.Length < 2)
            {
                Console.WriteLine("Not enough data in the file specified for dataset. Quitting…");
                return;
            }

            int max = int.Parse(dataLines[0]);
            List<Point> points = new List<Point>();
            for (int i = 1; i < dataLines.Length; i++)
            {
                points.Add(new Point(dataLines[i]));
            }

            // Read query

            string[] queryLines = File.ReadAllLines(queryFilePath);
            if (queryLines.Length < 1)
            {
                Console.WriteLine("Not enough data in the file specified for query. Quitting…");
                return;
            }

            List<Rectangle> queries = new List<Rectangle>();

            for (int i = 0; i < queryLines.Length; i++)
                queries.Add(new Rectangle(queryLines[i]));

            List<int> seqResults = new List<int>();
            List<int> rtResults = new List<int>();

            // Build R-Tree

            Console.WriteLine("Building tree");
            var tree = new RTree
            {
                Root = new Node
                {
                    DataPoints = points
                }
            };
            Console.WriteLine("Build complete.");

            // Sequential Query

            var stopwatch = Stopwatch.StartNew();

            foreach (var query in queries)
                seqResults.Add(SequentialQuery(points, query));

            stopwatch.Stop();
            Console.WriteLine($"Total time for sequential queries: {stopwatch.Elapsed.Milliseconds} ms.");

            File.WriteAllLines("./seqResults.txt", seqResults.Select(n => n.ToString()));

            // R-Tree query

            stopwatch = Stopwatch.StartNew();

            foreach (var query in queries)
                rtResults.Add(tree.Query(tree.Root, query));

            stopwatch.Stop();
            Console.WriteLine($"Total time for R-Tree queries: {stopwatch.Elapsed.Milliseconds} ms.");

            File.WriteAllLines("./rtResults.txt", rtResults.Select(n => n.ToString()));
        }

        static int SequentialQuery(List<Point> points, Rectangle query)
        {
            int result = 0;
            foreach (var point in points)
                if (RTree.IsCovered(point, query)) result++;
            return result;
        }
    }

    internal class Constants { internal const double B = 4.0; }
}
