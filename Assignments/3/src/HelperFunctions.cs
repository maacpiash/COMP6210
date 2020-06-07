using System;
using System.Collections.Generic;
using System.Linq;

namespace RTree
{
    internal partial class RTree // All the static methods live here
    {        
        internal static int IncreasePerim(Node node, Point point) =>
            (Max(node.MBR.X1, node.MBR.X2, point.X) - Min(node.MBR.X1, node.MBR.X2, point.X))
            + (Max(node.MBR.Y1, node.MBR.Y2, point.Y) - Min(node.MBR.Y1, node.MBR.Y2, point.Y))
            + node.Perimeter();

        internal static bool IsCovered(Point point, Rectangle query) =>
            query.X1 <= point.X && point.X <= query.X2 && query.Y1 <= point.Y && point.Y <= query.Y2;

        internal static bool IsIntersect(Node node, Rectangle query)
        {
            double c1x = (node.MBR.X1 + node.MBR.X2) / 2.0;
            double c1y = (node.MBR.Y1 + node.MBR.Y2) / 2.0;
            double w1 = node.MBR.X2 - node.MBR.X1;
            double l1 = node.MBR.Y2 - node.MBR.Y1;

            double c2x = (query.X1 + query.X2) / 2.0;
            double c2y = (query.Y1 + query.Y2) / 2.0;
            double w2 = query.X2 - query.X1;
            double l2 = query.Y2 - query.Y1;

            double Cx = Math.Abs(c1x - c2x);
            double Cy = Math.Abs(c1y - c2y);
            double L = (l1 + l2) / 2.0;
            double W = (w1 + w2) / 2.0;

            return (Cx <= W) && (Cy <= L);
        }

        internal static void AddChild(Node node, Node child)
        {
            node.ChildNodes.Add(child);
            child.ParentNode = node;

            if (child.MBR.X1 < node.MBR.X1)
                node.MBR.X1 = child.MBR.X1;
            if (child.MBR.X2 > node.MBR.X2)
                node.MBR.X2 = child.MBR.X2;

            if (child.MBR.Y1 < node.MBR.Y1)
                node.MBR.Y1 = child.MBR.Y1;
            if (child.MBR.Y2 > node.MBR.Y2)
                node.MBR.Y2 = child.MBR.Y2;
        }

        internal static void AddDataPoint(Node node, Point dataPoint)
        {
            node.DataPoints.Add(dataPoint);

            if (dataPoint.X < node.MBR.X1)
                node.MBR.X1 = dataPoint.X;
            if (dataPoint.X > node.MBR.X2)
                node.MBR.X2 = dataPoint.X;

            if (dataPoint.Y < node.MBR.Y1)
                node.MBR.Y1 = dataPoint.Y;
            if (dataPoint.Y > node.MBR.Y2)
                node.MBR.Y2 = dataPoint.Y;
        }

        internal static void UpdateMBR(Node node)
        {
            List<int> xList, yList;
            if (node.IsLeaf())
            {
                xList = node.DataPoints.Select(p => p.X).ToList();
                yList = node.DataPoints.Select(p => p.Y).ToList();
            }
            else
            {
                xList = new List<int>();
                yList = new List<int>();
                foreach (var childNode in node.ChildNodes)
                {
                    xList.Add(childNode.MBR.X1);
                    xList.Add(childNode.MBR.X2);
                    yList.Add(childNode.MBR.Y1);
                    yList.Add(childNode.MBR.Y2);
                
                }
            }
            node.MBR = new Rectangle
            {
                X1 = xList.Min(),
                X2 = xList.Max(),
                Y1 = yList.Min(),
                Y2 = yList.Max()
            };
        }

        internal static int Max(int a, int b, int c)
        {
            if (a >= b)
                return (a >= c) ? a : c;
            else
                return (b >= c) ? b : c;
        }

        internal static int Min(int a, int b, int c)
        {
            if (a <= b)
                return (a <= c) ? a : c;
            else
                return (b <= c) ? b : c;
        }
    }
}
