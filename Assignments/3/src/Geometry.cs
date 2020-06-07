namespace RTree
{
    internal class Point
    {
        internal int id { get; set; }
        internal int X { get; set; }
        internal int Y { get; set; }

        internal Point(string line)
        {
            string[] values = line.Split(' ');
            if (values.Length < 3)
                throw new System.Exception("There must be three values in the line.");
            this.id = int.Parse(values[0]);
            this.X = int.Parse(values[1]);
            this.Y = int.Parse(values[2]);
        }
    }

    internal class Rectangle
    {
        internal int X1 { get; set; }
        internal int Y1 { get; set; }
        internal int X2 { get; set; }
        internal int Y2 { get; set; }

        internal Rectangle()
        {
            this.X1 = -1;
            this.X2 = -1;
            this.Y1 = -1;
            this.Y2 = -1;
        }

        internal Rectangle(string line)
        {
            string[] values = line.Split(' ');
            if (values.Length < 4)
                throw new System.Exception("There must be four values in the line.");
            this.X1 = int.Parse(values[0]);
            this.Y1 = int.Parse(values[1]);
            this.X2 = int.Parse(values[2]);
            this.Y2 = int.Parse(values[3]);
        }
    }
}
