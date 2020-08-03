using System;

namespace B_tree
{
    class Program
    {
        //Main function
        static void Main(string[] args)
        {
            //INstantiated B-tree
            Console.WriteLine("B-Tree! \n Enter the order of the B-tree");
            BtreeDataType btreeDataType = new BtreeDataType();
            //setting order and maximum keys
            btreeDataType.order = Convert.ToInt32(Console.ReadLine());
            btreeDataType.maximumKeys = btreeDataType.order - 1;
            Console.WriteLine("Enter value: ");
            int valueToBeInserted = Convert.ToInt32(Console.ReadLine());
            //Instantiated and initialized root node
            btreeDataType.root = new Node(btreeDataType.maximumKeys);
            btreeDataType.root.keys[0] = valueToBeInserted;
            btreeDataType.root.lastIndex = 0;
            Console.WriteLine("Do you want to enter more values?");
            string isInsert = Console.ReadLine();
            while (isInsert == "Y" || isInsert == "y" || isInsert == "yes" || isInsert == "YES")
            {
                Console.WriteLine("Enter the value to be inserted: ");
                valueToBeInserted = Convert.ToInt32(Console.ReadLine());
                Tuple<Node, int> suitableTuple = btreeDataType.SearchForSuitableNodeRecursion(valueToBeInserted, 0, btreeDataType.root);

                btreeDataType.InsertRecursion(valueToBeInserted, suitableTuple.Item2, suitableTuple.Item1, null, null);
                //Program.Insert(valueToBeInserted, null, null);
                Console.WriteLine("Do you want to enter more values?");
                isInsert = Console.ReadLine();
            }
        }
    }
}
