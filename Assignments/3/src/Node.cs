using System.Collections.Generic;

namespace RTree
{
    internal class Node
    {
        internal int id { get; set; }
        internal Rectangle MBR { get; set; }
        internal List<Node> ChildNodes { get; set; }
        internal List<Point> DataPoints { get; set; }
        internal Node ParentNode { get; set; }

        private static double threshold = System.Math.Ceiling(Constants.B / 2.0); // for underflow detection
        
        internal Node()
        {
            this.ChildNodes = new List<Node>();
            this.DataPoints = new List<Point>();
            this.MBR = new Rectangle();
        }

        internal int Perimeter() => (this.MBR.X2 - this.MBR.X1) + (this.MBR.Y2 - this.MBR.Y1);
        
        internal bool IsUnderflow() =>
            this.IsLeaf() ? this.DataPoints.Count < threshold : this.ChildNodes.Count < threshold;

        internal bool IsOverflow() =>
            this.IsLeaf() ? this.DataPoints.Count > Constants.B : this.ChildNodes.Count > Constants.B;

        internal bool IsRoot() => this.ParentNode is null;
        internal bool IsLeaf() => this.ChildNodes.Count == 0;
    }
}
