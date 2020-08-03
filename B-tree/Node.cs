namespace B_tree
{
    /// <summary>
    /// Class node is for each node
    /// </summary>
    public class Node
    {
        public Node[] nodes;
        public int[] keys;
        public int lastIndex;
        public Node parentNode;
        public int parentIndex;
        public Node(int maximumKeys)
        {
            nodes = null;
            parentNode = null;
            parentIndex = -1;
            keys = new int[maximumKeys];
            lastIndex = -1;
        }
    }
}
