using System.Collections.Generic;

namespace RTree
{
    internal partial class RTree // The actual r-tree model lives here
    {
        internal Node Root { get; set; }

        internal RTree()
        {
            this.Root = new Node();
        }

        internal void Insert(Node node, Point point)
        {
            if (node.IsLeaf())
            {
                AddDataPoint(node, point);
                if (node.IsOverflow()) this.HandleOverflow(node);
            }
            else
            {
                var newNode = this.ChooseSubtree(node, point);
                this.Insert(newNode, point);
                UpdateMBR(newNode);
            }
        }

        internal void HandleOverflow(Node node)
        {
            var (node1, node2) = this.Split(node);
            if (node.IsRoot())
            {
                var newNode = new Node();
                AddChild(newNode, node1);
                AddChild(newNode, node2);
                this.Root = newNode;
                UpdateMBR(newNode);
            }
            else
            {
                var parentNode = node.ParentNode;
                parentNode.ChildNodes.Remove(node);
                AddChild(parentNode, node1);
                AddChild(parentNode, node2);
                if (parentNode.IsOverflow()) this.HandleOverflow(parentNode);
                UpdateMBR(parentNode);
            }
        }

        internal Node ChooseSubtree(Node node, Point point)
        {
            if (node.IsLeaf()) return node;
            int min = int.MaxValue;
            Node bestChild = new Node();
            foreach (var child in node.ChildNodes)
            {
                int newPerim = IncreasePerim(child, point);
                if (min > newPerim)
                {
                    min = newPerim;
                    bestChild = child;
                }
            }
            return bestChild;
        }

        internal int Query(Node node, Rectangle query)
        {
            int num = 0;
            if (node.IsLeaf())
            {
                foreach (var point in node.DataPoints)
                    if (IsCovered(point, query))
                        num++;
                return num;
            }
            else
            {
                foreach (var child in node.ChildNodes)
                    if (IsIntersect(child, query))
                        num += this.Query(child, query);
                return num;
            }
        }

        internal (Node, Node) Split(Node node)
        {
            Node bestS1 = new Node();
            Node bestS2 = new Node();
            int bestPerimeter = int.MaxValue;
            if (node.IsLeaf())
            {
                var tempX = new List<Point>(node.DataPoints);
                var tempY = new List<Point>(node.DataPoints);
                tempX.Sort((p, q) => p.X.CompareTo(q.X));
                tempY.Sort((p, q) => p.Y.CompareTo(q.Y));
                int m = node.DataPoints.Count;
                var divides = new List<Point>[]{ tempX, tempY };
                foreach (var divide in divides)
                {
                    int init = (int)System.Math.Ceiling(0.4 * Constants.B);
                    int max = m - init;
                    for (int i = init; i < max; i++)
                    {
                        var s1 = new Node();
                        var subList = new List<Point>();
                        for (int l = 0; l < i; l++) subList.Add(divide[i]);
                        s1.DataPoints = subList;
                        UpdateMBR(s1);

                        var s2 = new Node();
                        subList = new List<Point>();
                        for (int l = i; l < divide.Count; l++) subList.Add(divide[i]);
                        s2.DataPoints = subList;
                        UpdateMBR(s2);

                        if (bestPerimeter > s1.Perimeter() + s2.Perimeter())
                        {
                            bestPerimeter = s1.Perimeter() + s2.Perimeter();
                            bestS1 = s1;
                            bestS2 = s2;
                        }
                    }
                }
            }
            else
            {
                int m = node.ChildNodes.Count;
                var tempX1 = new List<Node>(node.ChildNodes);
                var tempY1 = new List<Node>(node.ChildNodes);
                var tempX2 = new List<Node>(node.ChildNodes);
                var tempY2 = new List<Node>(node.ChildNodes);
                
                tempX1.Sort((p, q) => p.MBR.X1.CompareTo(q.MBR.X1));
                tempY1.Sort((p, q) => p.MBR.Y1.CompareTo(q.MBR.Y1));
                tempX2.Sort((p, q) => p.MBR.X2.CompareTo(q.MBR.X2));
                tempY2.Sort((p, q) => p.MBR.Y2.CompareTo(q.MBR.Y2));

                var divides = new List<Node>[] { tempX1, tempX2, tempY1, tempY2 };

                foreach (var divide in divides)
                {
                    int init = (int)System.Math.Ceiling(0.4 * Constants.B);
                    int max = m - init;
                    for (int i = init; i < max; i++)
                    {
                        var s1 = new Node();
                        var subList = new List<Node>();
                        for (int l = 0; l < i; l++) subList.Add(divide[i]);
                        s1.ChildNodes = subList;
                        UpdateMBR(s1);
                        
                        var s2 = new Node();
                        subList = new List<Node>();
                        for (int l = i; l < divide.Count; l++) subList.Add(divide[i]);
                        s2.ChildNodes = subList;
                        UpdateMBR(s2);

                        if (bestPerimeter > s1.Perimeter() + s2.Perimeter())
                        {
                            bestPerimeter = s1.Perimeter() + s2.Perimeter();
                            bestS1 = s1;
                            bestS2 = s2;
                        }
                    }
                }
            }

            foreach (var child in bestS1.ChildNodes)
                child.ParentNode = bestS1;
            foreach (var child in bestS2.ChildNodes)
                child.ParentNode = bestS2;

            return (bestS1, bestS2);
        }
    }
}
