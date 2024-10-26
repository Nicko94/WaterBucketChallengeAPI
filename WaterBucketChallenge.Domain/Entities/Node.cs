namespace WaterBucketChallenge.Domain
{
    // Node class for path finding
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Operation { get; set; }

        public Node(int x, int y, string operation)
        {
            X = x;
            Y = y;
            Operation = operation;
        }
    }
}
