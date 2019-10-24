using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace GraphSharpDemo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    public class Node<T>
        {
            // Private member-variables
            private T data;
            private NodeList<T> neighbors = null;

            public Node()
            {
            }

            public Node(T data)
                : this(data, null)
            {
            }

            public Node(T data, NodeList<T> neighbors)
            {
                this.data = data;
                this.neighbors = neighbors;
            }

            public T Value
            {
                get { return data; }
                set { data = value; }
            }

            protected NodeList<T> Neighbors
            {
                get { return neighbors; }
                set { neighbors = value; }
            }
        }


        public class NodeList<T> : Collection<Node<T>>
        {
            public NodeList()
                : base()
            {
            }

            public NodeList(int initialSize)
            {
                // Add the specified number of items
                for (int i = 0; i < initialSize; i++)
                    base.Items.Add(default(Node<T>));
            }

            public Node<T> FindByValue(T value)
            {
                // search the list for the value
                foreach (Node<T> node in Items)
                    if (node.Value.Equals(value))
                        return node;

                // if we reached here, we didn't find a matching node
                return null;
            }
        }

        public class BinaryNode<T> : Node<T>
        {
            public BinaryNode()
                : base()
            {
            }

            public BinaryNode(T data)
                : base(data, null)
            {
            }

            public BinaryNode(T data, BinaryNode<T> left, BinaryNode<T> right)
            {
                base.Value = data;
                NodeList<T> children = new NodeList<T>(2);
                children[0] = left;
                children[1] = right;

                base.Neighbors = children;
            }

            public BinaryNode<T> Left
            {
                get
                {
                    if (base.Neighbors == null)
                        return null;
                    else
                        return (BinaryNode<T>)base.Neighbors[0];
                }
                set
                {
                    if (base.Neighbors == null)
                        base.Neighbors = new NodeList<T>(2);

                    base.Neighbors[0] = value;
                }
            }

            public BinaryNode<T> Right
            {
                get
                {
                    if (base.Neighbors == null)
                        return null;
                    else
                        return (BinaryNode<T>)base.Neighbors[1];
                }
                set
                {
                    if (base.Neighbors == null)
                        base.Neighbors = new NodeList<T>(2);

                    base.Neighbors[1] = value;
                }
            }
        }

        public class BinarySearchTree<T>
        {
            Comparer comparer = new Comparer(new CultureInfo("es-ES", false));
            public int count = 0;
            private BinaryNode<T> root;

            public BinarySearchTree()
            {
                root = null;
            }

            public virtual void Clear()
            {
                root = null;
            }

            public BinaryNode<T> Root
            {
                get { return root; }
                set { root = value; }
            }

            public void PreorderTraversal(BinaryNode<int> current)
            {
                if (current != null)
                {
                    // Output the value of the current node
                    //Console.WriteLine(current.Value);

                    // Recursively print the left and right children
                    PreorderTraversal(current.Left);
                    PreorderTraversal(current.Right);
                }
            }

            public void InorderTraversal(BinaryNode<int> current)
            {
                if (current != null)
                {
                    // Output the value of the current node
                    // Recursively print the left and right children
                    PreorderTraversal(current.Left);
                   // Console.WriteLine(current.Value);
                    PreorderTraversal(current.Right);
                }
            }
            public bool Contains(T data)
            {
                // search the tree for a node that contains data
                BinaryNode<T> current = root;
                int result;
                while (current != null)
                {
                    result = comparer.Compare(current.Value, data);
                    if (result == 0)
                        // we found data
                        return true;
                    else if (result > 0)
                        // current.Value > data, search current's left subtree
                        current = current.Left;
                    else if (result < 0)
                        // current.Value < data, search current's right subtree
                        current = current.Right;
                }

                return false;       // didn't find data
            }

            public virtual void Add(T data)
            {
                // create a new Node instance
                BinaryNode<T> n = new BinaryNode<T>(data);
                int result;

                // now, insert n into the tree
                // trace down the tree until we hit a NULL
                BinaryNode<T> current = root, parent = null;
                while (current != null)
                {
                    result = comparer.Compare(current.Value, data);
                    if (result == 0)
                        // they are equal - attempting to enter a duplicate - do nothing
                        return;
                    else if (result > 0)
                    {
                        // current.Value > data, must add n to current's left subtree
                        parent = current;
                        current = current.Left;
                    }
                    else if (result < 0)
                    {
                        // current.Value < data, must add n to current's right subtree
                        parent = current;
                        current = current.Right;
                    }
                }

                // We're ready to add the node!
                count++;       
                if (parent == null)
                    // the tree was empty, make n the root
                    root = n;
          
                else
                {
                    result = comparer.Compare(parent.Value, data);
                    if (result > 0)
                        // parent.Value > data, therefore n must be added to the left subtree
                        parent.Left = n;
                    else
                        // parent.Value < data, therefore n must be added to the right subtree
                        parent.Right = n;
                }
            }

  
            public bool Remove(T data)
            {
                // first make sure there exist some items in this tree
                if (root == null)
                    return false;       // no items to remove

                // Now, try to find data in the tree
                BinaryNode<T> current = root, parent = null;
                int result = comparer.Compare(current.Value, data);
                while (result != 0)
                {
                    if (result > 0)
                    {
                        // current.Value > data, if data exists it's in the left subtree
                        parent = current;
                        current = current.Left;
                    }
                    else if (result < 0)
                    {
                        // current.Value < data, if data exists it's in the right subtree
                        parent = current;
                        current = current.Right;
                    }

                    // If current == null, then we didn't find the item to remove
                    if (current == null)
                        return false;
                    else
                        result = comparer.Compare(current.Value, data);
                }

                // At this point, we've found the node to remove
                count--;

                // We now need to "rethread" the tree
                // CASE 1: If current has no right child, then current's left child becomes
                //         the node pointed to by the parent
                if (current.Right == null)
                {
                    if (parent == null)
                        root = current.Left;
                    else
                    {
                        result = comparer.Compare(parent.Value, current.Value);
                        if (result > 0)
                            // parent.Value > current.Value, so make current's left child a left child of parent
                            parent.Left = current.Left;
                        else if (result < 0)
                            // parent.Value < current.Value, so make current's left child a right child of parent
                            parent.Right = current.Left;
                    }
                }
                // CASE 2: If current's right child has no left child, then current's right child
                //         replaces current in the tree
                else if (current.Right.Left == null)
                {
                    current.Right.Left = current.Left;

                    if (parent == null)
                        root = current.Right;
                    else
                    {
                        result = comparer.Compare(parent.Value, current.Value);
                        if (result > 0)
                            // parent.Value > current.Value, so make current's right child a left child of parent
                            parent.Left = current.Right;
                        else if (result < 0)
                            // parent.Value < current.Value, so make current's right child a right child of parent
                            parent.Right = current.Right;
                    }
                }
                // CASE 3: If current's right child has a left child, replace current with current's
                //          right child's left-most descendent
                else
                {
                    // We first need to find the right node's left-most child
                    BinaryNode<T> leftmost = current.Right.Left, lmParent = current.Right;
                    while (leftmost.Left != null)
                    {
                        lmParent = leftmost;
                        leftmost = leftmost.Left;
                    }

                    // the parent's left subtree becomes the leftmost's right subtree
                    lmParent.Left = leftmost.Right;

                    // assign leftmost's left and right to current's left and right children
                    leftmost.Left = current.Left;
                    leftmost.Right = current.Right;

                    if (parent == null)
                        root = leftmost;
                    else
                    {
                        result = comparer.Compare(parent.Value, current.Value);
                        if (result > 0)
                            // parent.Value > current.Value, so make leftmost a left child of parent
                            parent.Left = leftmost;
                        else if (result < 0)
                            // parent.Value < current.Value, so make leftmost a right child of parent
                            parent.Right = leftmost;
                    }
                }

                return true;
            }

        }

    }

     //   private List<List<string>> l;

     //private int HashFunction(string s)
     //   {
     //       long x = Convert.ToInt64(s);
     //       long x1 = x / 10000;
     //       long x2 = x % 10000;
     //       int sum = Convert.ToInt16(x1 + x2);
     //       return sum % 10;
     //   }

   //else if (text == "Hash Table")
            //{
                
            //        existingVertices = new List<PocVertex>();
            //        Graph = new PocGraph(true);
            //        l = new List<List<string>>(10);

            //        for (int i = 0; i < 10; i++)
            //        {
            //            List<string> sublist = new List<string>();
            //            l.Add(sublist);
                  
            //        }

            //        int o = 0;
            //        foreach (var list in l)
            //        {
            //            existingVertices.Add(new PocVertex((o++).ToString(), true));

            //        }

            //        foreach (PocVertex vertex in existingVertices)
            //            Graph.AddVertex(vertex);

            //        for (int r = 0; r < 9; r++)
            //            AddNewGraphEdge(existingVertices[r], existingVertices[r + 1]);

               
                
            //        l[HashFunction(x.ToString())].Add(x.ToString());

            //        foreach (var list in l)
            //        {
            //            foreach (var str in list)
            //            {
            //                existingVertices.Add(new PocVertex((str).ToString(), true));
            //            }
            //        }

            //        foreach (PocVertex vertex in existingVertices)
            //            Graph.AddVertex(vertex);

            //        for (int i = 0; i < 10; i++)
            //        {
            //            int k = i;
            //            for (int j = 0; j < l[i].Count; j++)
            //            {
            //                AddNewGraphEdge(existingVertices[k], existingVertices[j]);
            //            }
            //        }
                
            //}
           



////////
///// 
