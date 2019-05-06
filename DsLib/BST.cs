using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsLib
{
    public delegate void MyAction<T>(T item);

    public class BST<T> where T : IComparable<T>// we want that we can compare between 2 data on a node
                                         // so we declare that the data<T> is IComparable!
    {
         Node root;// the base
        public int Count { get; set; }
        #region Add to the Tree
        public void Add(T item)
        {
            Count++;
            Node newNode = new Node(item);
            if (root == null)
            {// when the tree is empty so we create the first root
                root = newNode;
                return;
            }
            Node tmp = root;// always go in front of the parent, and we need to stop when he null,
                            // because he reach to the end of the root.
            Node parent = null;// create a node that always step back of the temp and when temp us null 
                               // so the parent is the last node in the root.

            while (tmp != null)
            {
                parent = tmp;// when the temp is not a null so we need to promote the parent
                if (newNode.data.CompareTo(tmp.data) < 0) tmp = tmp.left;// promote the temp to next node
                                                                         //check the data becouse we need to now where to countiue to left or right
                                                                         // left if the data small, right if the data is bigger
                else tmp = tmp.right;//promote the temp to the next node, acoording to the condition
            }
            // now after that the temp is null so the parnt is the last node in this root
            // soo we need to ask him about the data to know where to conect him to the root which dirction
            if (newNode.data.CompareTo(parent.data) < 0) // add to left
                parent.left = newNode;
            else
                parent.right = newNode;// ad to the right
        }
        #endregion

        #region Remove From Tree 

        public bool IsEmpty()
        {
            return root == null;
        }

        public void RemoveFromTree(T item)
        {// i need 2 node to track my steps and to now how to connect them after
            if (root == null) return;
            Node temp = root;
            Node parent = null;

            
            while (!temp.data.Equals(item))//find the node that i need to remove
            {
                parent = temp;
                // my moment is according to the data that i looking for
                if (temp.data.CompareTo(item) < 0) temp = temp.right;
                else temp = temp.left;
            }

            // 3 cases that i have after i founded the node that i need to remove
            //1- leaf - without child ,2 - only one child left or right,
            //3 - 2 childes

            //1
            if (temp.right == null && temp.left == null)
            {
                if (parent == null)
                {
                    root = null;
                    return;
                }
                if (parent.left == temp) parent.left = null;
                else parent.right = null;
            }
            //2
            else if (temp.right == null && !(temp.left == null) || !(temp.right == null) && temp.left == null)
            {//we need to check where he, and connect him to the
             // grandfather.
                if (parent == null)
                {
                    if (temp.right == null) root = temp.left;
                    else root = temp.right;
                    return;
                }
                if (!(temp.right == null))
                {
                    if (parent.left == temp)
                        parent.left = temp.right;
                    else parent.right = temp.right;
                }
                else //  the left node
                {
                    if (parent.left == temp)
                        parent.left = temp.left;
                    else parent.right = temp.left;
                }
            }

            //3
            else
            {// we need 3 node to remember all the time,1- parent(the one we what to remove)
             //2- temp(the one we what to take from him the data) 3-(the one before the temp
             // to disconnect the temp after we taking the data to parent)
                Node stepbBehindTemp = temp;
                parent = temp;
                temp = temp.right;
                while (temp.left != null)
                {
                    stepbBehindTemp = temp;
                    temp = temp.left;
                }

                parent.data = temp.data;
                if (parent.right == temp) parent.right = null;
                else stepbBehindTemp.left = null;//disconnect the node from the tree
            }
        }

        #endregion

        #region Scan Methods (InOrder,PostOrder,PreOrder)
        public void ScanInOrder(MyAction<T> act)//passing delegate to bring to the user the option
                                                 // to choose what he want to do with all the node in the tree
        {
            ScanInOrder(root, act);

        }
        public Node GetRoot() { return root; }
        //this fun run all the node that in the tree.
        public void ScanInOrder(Node n, MyAction<T> act)//the scan is inOrder(left,parent,right)
        {
            if (n == null) return;// stop point of the recursion

            ScanInOrder(n.left, act);
            act(n.data);// action is now doing invoke
            ScanInOrder(n.right, act);

        }

        private void ScanPreOrder(Node n, MyAction<T> act)// the scan is PreOrder(parent,left,right)
        {

            if (n == null) return;// stop point of the recursion
            act(n.data);
            ScanPreOrder(n.left, act);
            ScanPreOrder(n.right, act);
        }

        private void ScanPostOrder(Node n, MyAction<T> act)//scan is PostOrder(left,right,parent)
        {

            if (n == null) return;// stop point of the recursion
            ScanPreOrder(n.left, act);
            ScanPreOrder(n.right, act);
            act(n.data);
        }
        #endregion

        #region GetDepth
        public int GetDepth()// get the levels of the root
        {
            return GetDepth(root);
        }

        private int GetDepth(Node n)
        {
            if (n == null) return 0;
            int leftDepth = GetDepth(n.left);
            int rightDepth = GetDepth(n.right);
            return Math.Max(leftDepth, rightDepth) + 1;
        }
        #endregion

        #region Get The numbers Of Leaves
        public int GetLeaves()// get the levels of the root
        {
            return GetLeaves(root);
        }

        private int GetLeaves(Node n)
        {
            if (n == null) return 0;
            if (n.left == null && n.right == null) return 1;
            return GetLeaves(n.left) + GetLeaves(n.right);
        }

        #endregion

        #region BinarySearchTree
        public bool BinarySearchTree(T item)
        {
            return BinarySearchTree(root, item);
        }
        public bool Find(out T findItem, T itemForSearch) {
            Node tmp = root;
            findItem = default(T);
            while (tmp != null)
            {
                if (tmp.data.CompareTo(itemForSearch) == 0)
                {
                    findItem = tmp.data;
                    return true;
                }
                else if (itemForSearch.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right; 
            }
            return false;
        }
        public void FindExactOrClose(out T findItem, T itemForSearch)
        {
            
            Node tmp = root;
            findItem = default(T);
            while (tmp != null)
            {
                if (tmp.data.CompareTo(itemForSearch) == 0)
                {
                    findItem = tmp.data;
                    return;
                }

                else if (itemForSearch.CompareTo(tmp.data) < 0)
                {
                    findItem = tmp.data;
                    tmp = tmp.left;
                    
                }
                else tmp = tmp.right;
            }
           
        }
        private bool BinarySearchTree(Node node, T item)
        {
            if (node == null) return false;
            if (item.Equals(node.data)) return true;
            if (item.CompareTo(node.data) > 0)
                return BinarySearchTree(node.right, item);
            else return BinarySearchTree(node.left, item);
        }
        #endregion

        public class Node
        {
            public T data;
            public Node left;
            public Node right;

            public Node(T data)
            {
                this.data = data;
            }
        }
    }
}

