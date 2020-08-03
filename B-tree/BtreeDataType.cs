using System;

namespace B_tree
{
    public class BtreeDataType
    {
        public int order;
        public int maximumKeys;
        public Node root;
        public BtreeDataType()
        {
            root = null;
        }
        /// <summary>
        /// Method to insert values to the corresponding BtreeDataType
        /// </summary>
        /// <param name="insertionValue"></param>
        /// <param name="index"></param>
        /// <param name="currentNode"></param>
        /// <param name="leftNode"></param>
        /// <param name="rightNode"></param>
        public void InsertRecursion(int insertionValue, int index, Node currentNode, Node leftNode, Node rightNode)
        {
            int tempKey;
            Node newRightNode = null, tempNode = null;
            int medianIndex;
            //This if condition handles when existing root node is exploded(when the existing root node is full and splitted to create a new root node)
            if (currentNode == null)
            {
                currentNode = new Node(maximumKeys);
                root = currentNode;
                currentNode.keys[0] = insertionValue;
                currentNode.lastIndex = 0;
                currentNode.nodes = new Node[order];
                currentNode.nodes[0] = leftNode;
                currentNode.nodes[1] = rightNode;
                leftNode.parentNode = currentNode;
                rightNode.parentNode = currentNode;
                leftNode.parentIndex = 0;
                rightNode.parentIndex = 1;
            }
            //This if condition handles when current node has last index as the maximum possible index based on the order/maximumkeys)
            else if (currentNode.lastIndex == maximumKeys - 1)
            {
                //This if condition handles when the index where value to be inserted is same as last index of current node which is same as the maximumkeys-1)

                if (index == currentNode.lastIndex)
                {
                    #region Finding median for the current node
                    int median = (order) / 2;
                    if ((order) % 2 == 0)
                    {
                        medianIndex = median - 1;
                    }
                    else
                    {
                        medianIndex = median;
                    }
                    #endregion
                    newRightNode = new Node(maximumKeys);
                    newRightNode.parentNode = currentNode.parentNode;
                    if (currentNode.parentIndex != -1)
                    {
                        newRightNode.parentIndex = currentNode.parentIndex + 1;
                    }
                    if (currentNode.nodes != null)
                    {
                        newRightNode.nodes = new Node[order];
                    }
                    //Values of index after median in currentnode is set to newRightNode until last index
                    for (int i = medianIndex + 1, j = 0; i < currentNode.lastIndex; i++, j++)
                    {
                        newRightNode.keys[j] = currentNode.keys[i];
                        if (currentNode.nodes != null)
                        {
                            newRightNode.nodes[j] = currentNode.nodes[i];
                            currentNode.nodes[i] = new Node(maximumKeys);
                        }
                        currentNode.keys[i] = 0;
                        newRightNode.lastIndex++;
                    }
                    newRightNode.lastIndex++;
                    //Check if lastindex value is less than insertion value to set insertionValue and its corresponding node are set at last
                    if (currentNode.keys[currentNode.lastIndex] < insertionValue)
                    {
                        newRightNode.keys[newRightNode.lastIndex] = currentNode.keys[currentNode.lastIndex];
                        newRightNode.keys[newRightNode.lastIndex + 1] = insertionValue;
                        currentNode.keys[currentNode.lastIndex] = 0;
                        if (currentNode.nodes != null)
                        {
                            newRightNode.nodes[newRightNode.lastIndex] = currentNode.nodes[currentNode.lastIndex];
                            newRightNode.nodes[newRightNode.lastIndex + 1] = leftNode;
                            newRightNode.nodes[newRightNode.lastIndex + 2] = rightNode;
                            currentNode.nodes[currentNode.lastIndex] = new Node(maximumKeys);
                        }
                    }
                    //else insertionvalue and its corresponding nodes are set at last before
                    else
                    {
                        newRightNode.keys[newRightNode.lastIndex] = insertionValue;
                        newRightNode.keys[newRightNode.lastIndex + 1] = currentNode.keys[currentNode.lastIndex];
                        currentNode.keys[currentNode.lastIndex] = 0;
                        if (currentNode.nodes != null)
                        {
                            newRightNode.nodes[newRightNode.lastIndex] = leftNode;
                            newRightNode.nodes[newRightNode.lastIndex + 1] = rightNode;
                            newRightNode.nodes[newRightNode.lastIndex + 2] = currentNode.nodes[currentNode.lastIndex];
                            currentNode.nodes[currentNode.lastIndex] = new Node(maximumKeys);
                        }
                    }
                    //current node last index value and in
                    currentNode.lastIndex = medianIndex - 1;
                    newRightNode.lastIndex++;
                    insertionValue = currentNode.keys[medianIndex];
                    currentNode.keys[medianIndex] = 0;
                    if (currentNode.nodes != null)
                    {
                        currentNode.nodes[medianIndex+1] = new Node(maximumKeys);
                    }
                    //set right node parent index parent node
                    leftNode = currentNode;
                    rightNode = newRightNode;
                    InsertRecursion(insertionValue, currentNode.parentIndex, currentNode.parentNode, leftNode, rightNode);
                }
                //This else condition handles when the index where value to be inserted is not the last index of the current node)
                else
                {
                    tempKey = currentNode.keys[index];
                    currentNode.keys[index] = insertionValue;
                    insertionValue = tempKey;
                    if (currentNode.nodes != null)
                    {
                        tempNode = currentNode.nodes[index + 1];
                        currentNode.nodes[index] = leftNode;
                        currentNode.nodes[index + 1] = rightNode;
                    }
                    leftNode = rightNode;
                    rightNode = tempNode;
                    InsertRecursion(insertionValue, index + 1, currentNode, leftNode, rightNode);

                }
            }
            //This else condition handles when a value is inserted when it has space in the node it's trying to insert)
            else
            {
                tempKey = currentNode.keys[index];
                currentNode.keys[index] = insertionValue;
                if (currentNode.nodes != null)
                {
                    tempNode = currentNode.nodes[index + 1];
                    currentNode.nodes[index] = leftNode;
                    leftNode.parentIndex = index;
                    leftNode.parentNode = currentNode;
                    currentNode.nodes[index + 1] = rightNode;
                    rightNode.parentIndex = index + 1;
                    rightNode.parentNode = currentNode;
                    leftNode = rightNode;
                    rightNode = tempNode;
                }
                if (currentNode.lastIndex < index)
                {
                    currentNode.lastIndex = index;
                }
                else
                {
                    InsertRecursion(tempKey, index + 1, currentNode, leftNode, rightNode);
                }
            }
        }

        /// <summary>
        /// Searching the suitable node and suitable index for insertion of the insertionValue
        /// </summary>
        /// <param name="insertionValue"></param>
        /// <param name="index"></param>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        public Tuple<Node, int> SearchForSuitableNodeRecursion(int insertionValue, int index, Node currentNode)
        {
            Tuple<Node, int> returnedValue = new Tuple<Node, int>(currentNode, index);

            //This if condition handles insertionvalue is less than or equal to the curresponding index value
            if (insertionValue <= currentNode.keys[index])
            {
                ///This if condition handles whether child node is there or not.
                ///If yes, Then left node of the particular index will be sent again to find the suitable value
                ///else that particular index and node is suitable for insertion
                if (currentNode.nodes != null)
                {
                    returnedValue = SearchForSuitableNodeRecursion(insertionValue, 0, currentNode.nodes[index]);
                }
            }
            //This if condition checks if the index searched is the last index of the node
            else if (index == currentNode.lastIndex)
            {
                //This if condition checks if the index searched is the last index as per the order/maximumkeys
                if (index == maximumKeys - 1)
                {
                    ///This if condition handles whether child node is there or not.
                    ///If yes,then right node of the particular index will be sent again to find the suitable value
                    ///else that particular index and node is suitable for insertion
                    if (currentNode.nodes != null)
                    {
                        returnedValue = SearchForSuitableNodeRecursion(insertionValue, 0, currentNode.nodes[index + 1]);
                    }
                }
                else
                {
                    ///This if condition handles whether child node is there or not.
                    ///If yes,then right node of the particular index will be sent again to find the suitable value
                    ///else that next index with current node is considered suitable for insertion
                    if (currentNode.nodes != null)
                    {
                        returnedValue = SearchForSuitableNodeRecursion(insertionValue, 0, currentNode.nodes[index + 1]);
                    }
                    else
                    {
                        returnedValue = new Tuple<Node, int>(currentNode, index + 1);
                    }
                }
            }
            else
            {
                //This else used to verify the next index in the same node by calling the same function
                returnedValue = SearchForSuitableNodeRecursion(insertionValue, index + 1, currentNode);
            }

            return returnedValue;
        }

        /// <summary>
        /// Searching a particular value to get the node and index if available.
        /// Else null and 0 is returned
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="index"></param>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        public Tuple<Node, int> SearchBTree(int searchValue, int index, Node currentNode)
        {
            Tuple<Node, int> returnedValue = new Tuple<Node, int>(currentNode, index);
            if (searchValue == currentNode.keys[index])
            {
                return returnedValue;
            }
            if (searchValue < currentNode.keys[index])
            {
                if (currentNode.nodes != null)
                {
                    returnedValue = SearchForSuitableNodeRecursion(searchValue, 0, currentNode.nodes[index]);
                }
            }
            else if (index == currentNode.lastIndex)
            {
                if (currentNode.nodes != null)
                {
                    returnedValue = SearchForSuitableNodeRecursion(searchValue, 0, currentNode.nodes[index + 1]);
                }
                else
                {
                    returnedValue = new Tuple<Node, int>(null, 0);
                }
            }
            else
            {
                returnedValue = SearchForSuitableNodeRecursion(searchValue, index + 1, currentNode);
            }

            return returnedValue;
        }
    }
}
