using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using GraphSharp.Controls;
using GraphSharpDemo.CSKicksCollection.Trees;

namespace GraphSharpDemo
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    namespace CSKicksCollection.Trees
    {
        
        /// <summary>
        /// A Binary Tree node that holds an element and references to other tree nodes
        /// </summary>
        public class BinaryTreeNode<T>
            where T : IComparable
        {
            private T value;
            private BinaryTreeNode<T> leftChild;
            private BinaryTreeNode<T> rightChild;
            private BinaryTreeNode<T> parent;
            private BinaryTree<T> tree;

            /// <summary>
            /// The value stored at the node
            /// </summary>
            public virtual T Value
            {
                get { return value; }
                set { this.value = value; }
            }

            /// <summary>
            /// Gets or sets the left child node
            /// </summary>
            public virtual BinaryTreeNode<T> LeftChild
            {
                get { return leftChild; }
                set { leftChild = value; }
            }
            
            /// <summary>
            /// Gets or sets the right child node
            /// </summary>
            public virtual BinaryTreeNode<T> RightChild
            {
                get { return rightChild; }
                set { rightChild = value; }
            }

            /// <summary>
            /// Gets or sets the parent node
            /// </summary>
            public virtual BinaryTreeNode<T> Parent
            {
                get { return parent; }
                set { parent = value; }
            }

            /// <summary>
            /// Gets or sets the Binary Tree the node belongs to
            /// </summary>
            public virtual BinaryTree<T> Tree
            {
                get { return tree; }
                set { tree = value; }
            }

            /// <summary>
            /// Gets whether the node is a leaf (has no children)
            /// </summary>
            public virtual bool IsLeaf
            {
                get { return this.ChildCount == 0; }
            }

            /// <summary>
            /// Gets whether the node is internal (has children nodes)
            /// </summary>
            public virtual bool IsInternal
            {
                get { return this.ChildCount > 0; }
            }

            /// <summary>
            /// Gets whether the node is the left child of its parent
            /// </summary>
            public virtual bool IsLeftChild
            {
                get { return this.Parent != null && this.Parent.LeftChild == this; }
            }

            /// <summary>
            /// Gets whether the node is the right child of its parent
            /// </summary>
            public virtual bool IsRightChild
            {
                get { return this.Parent != null && this.Parent.RightChild == this; }
            }

            /// <summary>
            /// Gets the number of children this node has
            /// </summary>
            public virtual int ChildCount
            {
                get
                {
                    int count = 0;

                    if (this.LeftChild != null)
                        count++;

                    if (this.RightChild != null)
                        count++;

                    return count;
                }
            }

            /// <summary>
            /// Gets whether the node has a left child node
            /// </summary>
            public virtual bool HasLeftChild
            {
                get { return (this.LeftChild != null); }
            }

            /// <summary>
            /// Gets whether the node has a right child node
            /// </summary>
            public virtual bool HasRightChild
            {
                get { return (this.RightChild != null); }
            }

            /// <summary>
            /// Create a new instance of a Binary Tree node
            /// </summary>
            public BinaryTreeNode(T value)
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Binary Tree data structure
        /// </summary>
        public class BinaryTree<T> : ICollection<T>
            where T : IComparable
        {
            /// <summary>
            /// Specifies the mode of scanning through the tree
            /// </summary>
            public enum TraversalMode
            {
                InOrder = 0,
                PostOrder,
                PreOrder
            }

            private BinaryTreeNode<T> head;
            private Comparison<IComparable> comparer = CompareElements;
            private int size;
            private TraversalMode traversalMode = TraversalMode.InOrder;

            /// <summary>
            /// Gets or sets the root of the tree (the top-most node)
            /// </summary>
            public virtual BinaryTreeNode<T> Root
            {
                get { return head; }
                set { head = value; }
            }

            /// <summary>
            /// Gets whether the tree is read-only
            /// </summary>
            public virtual bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            /// Gets the number of elements stored in the tree
            /// </summary>
            public virtual int Count
            {
                get { return size; }
            }

            /// <summary>
            /// Gets or sets the traversal mode of the tree
            /// </summary>
            public virtual TraversalMode TraversalOrder
            {
                get { return traversalMode; }
                set { traversalMode = value; }
            }

            /// <summary>
            /// Creates a new instance of a Binary Tree
            /// </summary>
            public BinaryTree()
            {
                head = null;
                size = 0;
            }

            /// <summary>
            /// Adds a new element to the tree
            /// </summary>
            public virtual void Add(T value)
            {
                BinaryTreeNode<T> node = new BinaryTreeNode<T>(value);
                this.Add(node);
            }

            /// <summary>
            /// Adds a node to the tree
            /// </summary>
            public virtual void Add(BinaryTreeNode<T> node)
            {
                if (this.head == null) //first element being added
                {
                    this.head = node; //set node as root of the tree
                    node.Tree = this;
                    size++;
                }
                else
                {
                    if (node.Parent == null)
                        node.Parent = head; //start at head

                    //Node is inserted on the left side if it is smaller or equal to the parent
                    bool insertLeftSide = comparer((IComparable) node.Value, (IComparable) node.Parent.Value) <= 0;

                    if (insertLeftSide) //insert on the left
                    {
                        if (node.Parent.LeftChild == null)
                        {
                            node.Parent.LeftChild = node; //insert in left
                            size++;
                            node.Tree = this; //assign node to this binary tree
                        }
                        else
                        {
                            node.Parent = node.Parent.LeftChild; //scan down to left child
                            this.Add(node); //recursive call
                        }
                    }
                    else //insert on the right
                    {
                        if (node.Parent.RightChild == null)
                        {
                            node.Parent.RightChild = node; //insert in right
                            size++;
                            node.Tree = this; //assign node to this binary tree
                        }
                        else
                        {
                            node.Parent = node.Parent.RightChild;
                            this.Add(node);
                        }
                    }
                }
            }

            /// <summary>
            /// Returns the first node in the tree with the parameter value.
            /// </summary>
            public virtual BinaryTreeNode<T> Find(T value)
            {
                BinaryTreeNode<T> node = this.head; //start at head
                while (node != null)
                {
                    if (node.Value.Equals(value)) //parameter value found
                        return node;
                    else
                    {
                        //Search left if the value is smaller than the current node
                        bool searchLeft = comparer((IComparable) value, (IComparable) node.Value) < 0;

                        if (searchLeft)
                            node = node.LeftChild; //search left
                        else
                            node = node.RightChild; //search right
                    }
                }

                return null; //not found
            }

            /// <summary>
            /// Returns whether a value is stored in the tree
            /// </summary>
            public virtual bool Contains(T value)
            {
                return (this.Find(value) != null);
            }

            /// <summary>
            /// Removes a value from the tree and returns whether the removal was successful.
            /// </summary>
            public virtual bool Remove(T value)
            {
                BinaryTreeNode<T> removeNode = Find(value);

                return this.Remove(removeNode);
            }

            /// <summary>
            /// Removes a node from the tree and returns whether the removal was successful.
            /// </summary>>
            public virtual bool Remove(BinaryTreeNode<T> removeNode)
            {
                if (removeNode == null || removeNode.Tree != this)
                    return false; //value doesn't exist or not of this tree

                //Note whether the node to be removed is the root of the tree
                bool wasHead = (removeNode == head);

                if (this.Count == 1)
                {
                    this.head = null; //only element was the root
                    removeNode.Tree = null;

                    size--; //decrease total element count
                }
                else if (removeNode.IsLeaf) //Case 1: No Children
                {
                    //Remove node from its parent
                    if (removeNode.IsLeftChild)
                        removeNode.Parent.LeftChild = null;
                    else
                        removeNode.Parent.RightChild = null;

                    removeNode.Tree = null;
                    removeNode.Parent = null;

                    size--; //decrease total element count
                }
                else if (removeNode.ChildCount == 1) //Case 2: One Child
                {
                    if (removeNode.HasLeftChild)
                    {
                        //Put left child node in place of the node to be removed
                        removeNode.LeftChild.Parent = removeNode.Parent; //update parent

                        if (wasHead)
                        try
                        {
                            this.Root = removeNode.LeftChild; //update root reference if needed
                        }
                        catch (NullReferenceException)
                        {

                        }
                        if (removeNode.IsLeftChild) //update the parent's child reference
                             try
                            {
                                removeNode.Parent.LeftChild = removeNode.LeftChild;
                            }
                            catch (NullReferenceException)
                            {
                                
                            }
                        else
                            try
                            {
                                removeNode.Parent.RightChild = removeNode.LeftChild;
                            }
                            catch (NullReferenceException)
                            {
                                
                            }
                    }
                    else //Has right child
                    {
                        //Put left node in place of the node to be removed
                        removeNode.RightChild.Parent = removeNode.Parent; //update parent

                        if (wasHead)
                            this.Root = removeNode.RightChild; //update root reference if needed

                        if (removeNode.IsLeftChild) //update the parent's child reference
                            removeNode.Parent.LeftChild = removeNode.RightChild;
                        else
                        {
                            try
                            {
                                removeNode.Parent.RightChild = removeNode.RightChild;

                            }
                            catch (NullReferenceException)
                            {
                                
                            }
                        }
                    }

                    removeNode.Tree = null;
                    removeNode.Parent = null;
                    removeNode.LeftChild = null;
                    removeNode.RightChild = null;

                    size--; //decrease total element count
                }
                else //Case 3: Two Children
                {
                    //Find inorder predecessor (right-most node in left subtree)
                    BinaryTreeNode<T> successorNode = removeNode.LeftChild;
                    while (successorNode.RightChild != null)
                    {
                        successorNode = successorNode.RightChild;
                    }

                    removeNode.Value = successorNode.Value; //replace value

                    this.Remove(successorNode); //recursively remove the inorder predecessor
                }


                return true;
            }

            /// <summary>
            /// Removes all the elements stored in the tree
            /// </summary>
            public virtual void Clear()
            {
                //Remove children first, then parent
                //(Post-order traversal)
                IEnumerator<T> enumerator = GetPostOrderEnumerator();
                while (enumerator.MoveNext())
                {
                    this.Remove(enumerator.Current);
                }
                enumerator.Dispose();
            }

            /// <summary>
            /// Returns the height of the entire tree
            /// </summary>
            public virtual int GetHeight()
            {
                return this.GetHeight(this.Root);
            }

            /// <summary>
            /// Returns the height of the subtree rooted at the parameter value
            /// </summary>
            public virtual int GetHeight(T value)
            {
                //Find the value's node in tree
                BinaryTreeNode<T> valueNode = this.Find(value);
                if (value != null)
                    return this.GetHeight(valueNode);
                else
                    return 0;
            }

            /// <summary>
            /// Returns the height of the subtree rooted at the parameter node
            /// </summary>
            public virtual int GetHeight(BinaryTreeNode<T> startNode)
            {
                if (startNode == null)
                    return 0;
                else
                    return 1 + Math.Max(GetHeight(startNode.LeftChild), GetHeight(startNode.RightChild));
            }

            /// <summary>
            /// Returns the depth of a subtree rooted at the parameter value
            /// </summary>
            public virtual int GetDepth(T value)
            {
                BinaryTreeNode<T> node = this.Find(value);
                return this.GetDepth(node);
            }

            /// <summary>
            /// Returns the depth of a subtree rooted at the parameter node
            /// </summary>
            public virtual int GetDepth(BinaryTreeNode<T> startNode)
            {
                int depth = 0;

                if (startNode == null)
                    return depth;

                BinaryTreeNode<T> parentNode = startNode.Parent; //start a node above
                while (parentNode != null)
                {
                    depth++;
                    parentNode = parentNode.Parent; //scan up towards the root
                }

                return depth;
            }

            /// <summary>
            /// Returns an enumerator to scan through the elements stored in tree.
            /// The enumerator uses the traversal set in the TraversalMode property.
            /// </summary>
            public virtual IEnumerator<T> GetEnumerator()
            {
                switch (this.TraversalOrder)
                {
                    case TraversalMode.InOrder:
                        return GetInOrderEnumerator();
                    case TraversalMode.PostOrder:
                        return GetPostOrderEnumerator();
                    case TraversalMode.PreOrder:
                        return GetPreOrderEnumerator();
                    default:
                        return GetInOrderEnumerator();
                }
            }

            /// <summary>
            /// Returns an enumerator to scan through the elements stored in tree.
            /// The enumerator uses the traversal set in the TraversalMode property.
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that visits node in the order: left child, parent, right child
            /// </summary>
            public virtual IEnumerator<T> GetInOrderEnumerator()
            {
                return new BinaryTreeInOrderEnumerator(this);
            }

            /// <summary>
            /// Returns an enumerator that visits node in the order: left child, right child, parent
            /// </summary>
            public virtual IEnumerator<T> GetPostOrderEnumerator()
            {
                return new BinaryTreePostOrderEnumerator(this);
            }

            /// <summary>
            /// Returns an enumerator that visits node in the order: parent, left child, right child
            /// </summary>
            public virtual IEnumerator<T> GetPreOrderEnumerator()
            {
                return new BinaryTreePreOrderEnumerator(this);
            }

            /// <summary>
            /// Copies the elements in the tree to an array using the traversal mode specified.
            /// </summary>
            public virtual void CopyTo(T[] array)
            {
                this.CopyTo(array, 0);
            }

            /// <summary>
            /// Copies the elements in the tree to an array using the traversal mode specified.
            /// </summary>
            public virtual void CopyTo(T[] array, int startIndex)
            {
                IEnumerator<T> enumerator = this.GetEnumerator();

                for (int i = startIndex; i < array.Length; i++)
                {
                    if (enumerator.MoveNext())
                        array[i] = enumerator.Current;
                    else
                        break;
                }
            }

            /// <summary>
            /// Compares two elements to determine their positions within the tree.
            /// </summary>
            public static int CompareElements(IComparable x, IComparable y)
            {
                return x.CompareTo(y);
            }

            /// <summary>
            /// Returns an inorder-traversal enumerator for the tree values
            /// </summary>
            internal class BinaryTreeInOrderEnumerator : IEnumerator<T>
            {
                private BinaryTreeNode<T> current;
                private BinaryTree<T> tree;
                internal Queue<BinaryTreeNode<T>> traverseQueue;

                public BinaryTreeInOrderEnumerator(BinaryTree<T> tree)
                {
                    this.tree = tree;

                    //Build queue
                    traverseQueue = new Queue<BinaryTreeNode<T>>();
                    visitNode(this.tree.Root);
                }

                private void visitNode(BinaryTreeNode<T> node)
                {
                    if (node == null)
                        return;
                    else
                    {
                        visitNode(node.LeftChild);
                        traverseQueue.Enqueue(node);
                        visitNode(node.RightChild);
                    }
                }

                public T Current
                {
                    get { return current.Value; }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }

                public void Dispose()
                {
                    current = null;
                    tree = null;
                }

                public void Reset()
                {
                    current = null;
                }

                public bool MoveNext()
                {
                    if (traverseQueue.Count > 0)
                        current = traverseQueue.Dequeue();
                    else
                        current = null;

                    return (current != null);
                }
            }

            /// <summary>
            /// Returns a postorder-traversal enumerator for the tree values
            /// </summary>
            internal class BinaryTreePostOrderEnumerator : IEnumerator<T>
            {
                private BinaryTreeNode<T> current;
                private BinaryTree<T> tree;
                internal Queue<BinaryTreeNode<T>> traverseQueue;

                public BinaryTreePostOrderEnumerator(BinaryTree<T> tree)
                {
                    this.tree = tree;

                    //Build queue
                    traverseQueue = new Queue<BinaryTreeNode<T>>();
                    visitNode(this.tree.Root);
                }

                private void visitNode(BinaryTreeNode<T> node)
                {
                    if (node == null)
                        return;
                    else
                    {
                        visitNode(node.LeftChild);
                        visitNode(node.RightChild);
                        traverseQueue.Enqueue(node);
                    }
                }

                public T Current
                {
                    get { return current.Value; }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }

                public void Dispose()
                {
                    current = null;
                    tree = null;
                }

                public void Reset()
                {
                    current = null;
                }

                public bool MoveNext()
                {
                    if (traverseQueue.Count > 0)
                        current = traverseQueue.Dequeue();
                    else
                        current = null;

                    return (current != null);
                }
            }

            /// <summary>
            /// Returns an preorder-traversal enumerator for the tree values
            /// </summary>
            internal class BinaryTreePreOrderEnumerator : IEnumerator<T>
            {
                private BinaryTreeNode<T> current;
                private BinaryTree<T> tree;
                internal Queue<BinaryTreeNode<T>> traverseQueue;

                public BinaryTreePreOrderEnumerator(BinaryTree<T> tree)
                {
                    this.tree = tree;

                    //Build queue
                    traverseQueue = new Queue<BinaryTreeNode<T>>();
                    visitNode(this.tree.Root);
                }

                private void visitNode(BinaryTreeNode<T> node)
                {
                    if (node == null)
                        return;
                    else
                    {
                        traverseQueue.Enqueue(node);
                        visitNode(node.LeftChild);
                        visitNode(node.RightChild);
                    }
                }

                public T Current
                {
                    get { return current.Value; }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }

                public void Dispose()
                {
                    current = null;
                    tree = null;
                }

                public void Reset()
                {
                    current = null;
                }

                public bool MoveNext()
                {
                    if (traverseQueue.Count > 0)
                        current = traverseQueue.Dequeue();
                    else
                        current = null;

                    return (current != null);
                }
            }
        }
    }

    public class PocGraphLayout : GraphLayout<PocVertex, PocEdge, PocGraph> { }

   public class AVLTreeNode<T> : BinaryTreeNode<T>
       where T : IComparable
    {
        public AVLTreeNode(T value)
            : base(value)
        {
        }

        public new AVLTreeNode<T> LeftChild
        {
            get
            {
                return (AVLTreeNode<T>)base.LeftChild;
            }
            set
            {
                base.LeftChild = value;
            }
        }

        public new AVLTreeNode<T> RightChild
        {
            get
            {
                return (AVLTreeNode<T>)base.RightChild;
            }
            set
            {
                base.RightChild = value;
            }
        }

        public new AVLTreeNode<T> Parent
        {
            get
            {
                return (AVLTreeNode<T>)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }
    }


    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Data

        public  List<PocVertex> existingVertices;
        private string layoutAlgorithmType;
        public  PocGraph graph;
        private List<String> layoutAlgorithmTypes = new List<string>();
        private int count;
        #endregion

        /// <summary>
        /// AVL Tree data structure
        /// </summary>
      public  class AVLTree<T> : BinaryTree<T>
            where T : IComparable
        {
            /// <summary>
            /// Returns the AVL Node of the tree
            /// </summary>
            public new AVLTreeNode<T> Root
            {
                get { return (AVLTreeNode<T>)base.Root; }
                set { base.Root = value; }
            }

            /// <summary>
            /// Returns the AVL Node corresponding to the given value
            /// </summary>
            public new AVLTreeNode<T> Find(T value)
            {
                return (AVLTreeNode<T>)base.Find(value);
            }

            public void preorder(AVLTreeNode<T> current)
            {
                if (current != null)
                {
                    // existingVertices.Add(new PocVertex((current.data + 1).ToString(), true));
                    //Console.WriteLine(current.Value);
                    preorder(current.LeftChild);
                    preorder(current.RightChild);
                }
            }
            /// <summary>
            /// Insert a value in the tree and rebalance the tree if necessary.
            /// </summary>
            public override void Add(T value)
            {
                AVLTreeNode<T> node = new AVLTreeNode<T>(value);

                base.Add(node); //add normally

                //Balance every node going up, starting with the parent
                AVLTreeNode<T> parentNode = node.Parent;

                while (parentNode != null)
                {
                    int balance = this.getBalance(parentNode);
                    if (Math.Abs(balance) == 2) //-2 or 2 is unbalanced
                    {
                        //Rebalance tree
                        this.balanceAt(parentNode, balance);
                    }

                    parentNode = parentNode.Parent; //keep going up
                }
            }

            /// <summary>
            /// Removes a given value from the tree and rebalances the tree if necessary.
            /// </summary>
            public override bool Remove(T value)
            {
                AVLTreeNode<T> valueNode = this.Find(value);
                return this.Remove(valueNode);
            }

            /// <summary>
            /// Wrapper method for removing a node within the tree
            /// </summary>
            protected new bool Remove(BinaryTreeNode<T> removeNode)
            {
                return this.Remove((AVLTreeNode<T>)removeNode);
            }

            /// <summary>
            /// Removes a given node from the tree and rebalances the tree if necessary.
            /// </summary>
            public bool Remove(AVLTreeNode<T> valueNode)
            {
                //Save reference to the parent node to be removed
                AVLTreeNode<T> parentNode = valueNode.Parent;

                //Remove the node as usual
                bool removed = base.Remove(valueNode);

                if (!removed)
                    return false; //removing failed, no need to rebalance
                else
                {
                    //Balance going up the tree
                    while (parentNode != null)
                    {
                        int balance = this.getBalance(parentNode);

                        if (Math.Abs(balance) == 1) //1, -1
                            break; //height hasn't changed, can stop
                        else if (Math.Abs(balance) == 2) //2, -2
                        {
                            //Rebalance tree
                            this.balanceAt(parentNode, balance);
                        }

                        parentNode = parentNode.Parent;
                    }

                    return true;
                }
            }

            /// <summary>
            /// Balances an AVL Tree node
            /// </summary>
            protected virtual void balanceAt(AVLTreeNode<T> node, int balance)
            {
                if (balance == 2) //right outweighs
                {
                    int rightBalance = getBalance(node.RightChild);

                    if (rightBalance == 1 || rightBalance == 0)
                    {
                        //Left rotation needed
                        rotateLeft(node);
                    }
                    else if (rightBalance == -1)
                    {
                        //Right rotation needed
                        rotateRight(node.RightChild);

                        //Left rotation needed
                        rotateLeft(node);
                    }
                }
                else if (balance == -2) //left outweighs
                {
                    int leftBalance = getBalance(node.LeftChild);
                    if (leftBalance == 1)
                    {
                        //Left rotation needed
                        rotateLeft(node.LeftChild);

                        //Right rotation needed
                        rotateRight(node);
                    }
                    else if (leftBalance == -1 || leftBalance == 0)
                    {
                        //Right rotation needed
                        rotateRight(node);
                    }
                }
            }

            /// <summary>
            /// Determines the balance of a given node
            /// </summary>
            protected virtual int getBalance(AVLTreeNode<T> root)
            {
                //Balance = right child's height - left child's height
                return this.GetHeight(root.RightChild) - this.GetHeight(root.LeftChild);
            }

            /// <summary>
            /// Rotates a node to the left within an AVL Tree
            /// </summary>
            protected virtual void rotateLeft(AVLTreeNode<T> root)
            {
                if (root == null)
                    return;

                AVLTreeNode<T> pivot = root.RightChild;

                if (pivot == null)
                    return;
                else
                {
                    AVLTreeNode<T> rootParent = root.Parent; //original parent of root node
                    bool isLeftChild = (rootParent != null) && rootParent.LeftChild == root; //whether the root was the parent's left node
                    bool makeTreeRoot = root.Tree.Root == root; //whether the root was the root of the entire tree

                    //Rotate
                    root.RightChild = pivot.LeftChild;
                    pivot.LeftChild = root;

                    //Update parents
                    root.Parent = pivot;
                    pivot.Parent = rootParent;

                    if (root.RightChild != null)
                        root.RightChild.Parent = root;

                    //Update the entire tree's Root if necessary
                    if (makeTreeRoot)
                        pivot.Tree.Root = pivot;

                    //Update the original parent's child node
                    if (isLeftChild)
                        rootParent.LeftChild = pivot;
                    else
                        if (rootParent != null)
                            rootParent.RightChild = pivot;
                }
            }

            /// <summary>
            /// Rotates a node to the right within an AVL Tree
            /// </summary>
            protected virtual void rotateRight(AVLTreeNode<T> root)
            {
                if (root == null)
                    return;

                AVLTreeNode<T> pivot = root.LeftChild;

                if (pivot == null)
                    return;
                else
                {
                    AVLTreeNode<T> rootParent = root.Parent; //original parent of root node
                    bool isLeftChild = (rootParent != null) && rootParent.LeftChild == root; //whether the root was the parent's left node
                    bool makeTreeRoot = root.Tree.Root == root; //whether the root was the root of the entire tree

                    //Rotate
                    root.LeftChild = pivot.RightChild;
                    pivot.RightChild = root;

                    //Update parents
                    root.Parent = pivot;
                    pivot.Parent = rootParent;

                    if (root.LeftChild != null)
                        root.LeftChild.Parent = root;

                    //Update the entire tree's Root if necessary
                    if (makeTreeRoot)
                        pivot.Tree.Root = pivot;

                    //Update the original parent's child node
                    if (isLeftChild)
                        rootParent.LeftChild = pivot;
                    else
                        if (rootParent != null)
                            rootParent.RightChild = pivot;
                }
            }

        }

        #region Ctor
        public int u = 0;
        public List<int> list;
        public Dictionary<int, int> map;

      public AVLTree<int> t0 = new AVLTree<int>();
         
        public List<List<int>> G =new List<List<int>>(100); 
        
      public void init()
      {
          try { existingVertices.Clear();}
          catch (System.NullReferenceException) { }
          try { map.Clear(); }
          catch (System.NullReferenceException) { }
          try { list.Clear(); }
          catch (System.NullReferenceException) { }
          try { graph.Clear(); }
          catch (System.NullReferenceException) { }
          existingVertices = new List<PocVertex>();
          map = new Dictionary<int, int>();
          list = new List<int>();
          Graph = new PocGraph(true);
          u = 0;
         
      }

        public string preoreder="";
      public void DisplayTree()
      {
          PreOrderDisplayTree(t0.Root);
      }
      private void PreOrderDisplayTree(AVLTreeNode<int> current)
      {
          if (current != null)
          {
              list.Add(Convert.ToInt32(current.Value));
              try
              {
                  map.Add(Convert.ToInt32(current.Value), 0);

              }
              catch (System.ArgumentException)
              {
                  
              }
              preoreder += current.Value.ToString()+" -> ";

              PreOrderDisplayTree(current.LeftChild);
              PreOrderDisplayTree(current.RightChild);

          }
      }

      public void fixitupbaby()
      {
          list.Sort();
          for (int i = 0; i < list.Count; i++)
          {
              if (map[Convert.ToInt32(list[i])] == 0) map[Convert.ToInt32(list[i])] = u++;
              existingVertices.Add(new PocVertex((list[i]).ToString(), true));
          }
          foreach (PocVertex vertex in existingVertices)
              Graph.AddVertex(vertex);
      }




      public void pre(AVLTreeNode<int> current)
      {
          if (current != null)
          {
              if (current.LeftChild != null)
                  AddNewGraphEdge(existingVertices[map[Convert.ToInt32(current.Value)]], existingVertices[map[Convert.ToInt32(current.LeftChild.Value)]]);
              if (current.RightChild != null)
                  AddNewGraphEdge(existingVertices[map[Convert.ToInt32(current.Value)]], existingVertices[map[Convert.ToInt32(current.RightChild.Value)]]);
              pre(current.LeftChild);
              pre(current.RightChild);
          }
      }
    
      public void usepre()
      {
          if (t0.Root == null)
          {
              return;
          }
          if (t0.Root != null)
          {
              pre(t0.Root);
          }
      }



      public BinarySearchTree<int> t = new BinarySearchTree<int>();

      public void DisplayTree1()
      {
          PreOrderDisplayTree1(t.Root);
      }
      private void PreOrderDisplayTree1(BinaryNode<int> current)
      {
          if (current != null)
          {
              // existingVertices.Add(new PocVertex((current.Value).ToString(), true));
              list.Add(Convert.ToInt32(current.Value));
              map.Add(Convert.ToInt32(current.Value), 0);
              preoreder += current.Value.ToString() + " -> ";

              PreOrderDisplayTree1(current.Left);
              PreOrderDisplayTree1(current.Right);

          }
      }

      public void fixitupbaby1()
      {
          list.Sort();
          for (int i = 0; i < list.Count; i++)
          {
              if (map[Convert.ToInt32(list[i])] == 0) map[Convert.ToInt32(list[i])] = u++;
              existingVertices.Add(new PocVertex((list[i]).ToString(), true));
          }
          foreach (PocVertex vertex in existingVertices)
              Graph.AddVertex(vertex);
      }




      public void pre1(BinaryNode<int> current)
      {
          if (current != null)
          {
              // MessageBox.Show((current.Value).ToString());
              if (current.Left != null)
                  AddNewGraphEdge(existingVertices[map[Convert.ToInt32(current.Value)]], existingVertices[map[Convert.ToInt32(current.Left.Value)]]);
              if (current.Right != null)
                  AddNewGraphEdge(existingVertices[map[Convert.ToInt32(current.Value)]], existingVertices[map[Convert.ToInt32(current.Right.Value)]]);

              pre1(current.Left);
              pre1(current.Right);
          }
      }
      public void usepre1()
      {
          if (t.Root == null)
          {
              return;
          }
          if (t.Root != null)
          {
              pre1(t.Root);
          }
      }

   
      public void fixitupbaby2()
      {
          u = 0;
          try
          {
              for (int i = 0; i < map.Count; i++)
                  map[i] = 0;
          }
          catch (System.OutOfMemoryException)
          {              
          }
   
        
          
          for (int i = 0; i < list.Count; i++)
          {
              if ((map[list[i]] == 0)) map[list[i]] = u++;
              existingVertices.Add(new PocVertex((list[i]).ToString(), true));
              
          }

          foreach (PocVertex vertex in existingVertices)
              Graph.AddVertex(vertex);
      }

      public void pre2()
      {
          for (int i = 0; i < list.Count; i += 2)
              try
              {
                  for (int j = i+1 ; j < i+3; j++)
                      if (j < list.Count)
                          AddNewGraphEdge(existingVertices[map[list[i]]], existingVertices[map[list[j]]]);
              }
              catch (IndexOutOfRangeException)
              {                            
              }
                                  
      }

        public bool[] visited = new bool[100];
        public long[] val = new long[100];
        public string path = "";
     public void DFS(int node)
      {
          //MessageBox.Show(val[node].ToString());
         path += val[node].ToString()+" -> ";
          visited[node] = true;
          for (int i = 0; i < G[node].Count; ++i)
              if (!visited[G[node][i]])
                  DFS(G[node][i]);
      }


        public void BFS(int x)
        {         
            Queue<int> q=new Queue<int>();
            q.Enqueue(x);
            //MessageBox.Show(val[x].ToString());
            path += val[x].ToString()+" -> ";
            while (q.Count!=0)
            {
                int u = q.Peek();
                q.Dequeue();
                visited[u] = true;
                for (int i = 0; i < G[u].Count(); i++)
                {
                    int qq = G[u][i];
                    if (!visited[qq])
                    {
                        q.Enqueue(qq);
                      //  MessageBox.Show(val[qq].ToString());
                        path += val[qq].ToString() + " -> ";
                        visited[qq] = true;              
                    }
                }
            }
        }
        public int Shortest_BFS(int start, int Goal)
        {
            Queue<Tuple<int, int>> Q = new Queue<Tuple<int, int>>();
            var t = Tuple.Create(start, 0);
            Q.Enqueue(t);
            visited[start] = true;
            while (Q.Count()!=0)
            {
                Tuple<int, int> x = Q.Peek();
                Q.Dequeue();
                if (x.Item1 == Goal) return x.Item2;
                for (int i = 0; i < G[x.Item1].Count(); i++)
                    if (!visited[G[x.Item1][i]])
                    {
                        visited[G[x.Item1][i]] = true;
                        var t2 = Tuple.Create(G[x.Item1][i], x.Item2 + 1);
                        Q.Enqueue(t2);
                    }
            }
            return -1; // if there is no path between start and Goal
        }


        public  MainWindowViewModel()
        {
            LayoutAlgorithmType = "EfficientSugiyama";


            //layoutAlgorithmTypes.Add("BoundedFR");
            //layoutAlgorithmTypes.Add("Circular");
            //layoutAlgorithmTypes.Add("CompoundFDP");
            //layoutAlgorithmTypes.Add("ISOM");      

        }
        #endregion

        public bool first = true;
        public bool firstGraph = true;
        int i, j;

      
        public void insert(int x ,int y, string text)
        {
            if (first && text != "Graph")
            {
                list.Add(x);
                if(text=="Binary Tree")
                    map.Add(x, 0);           
                else if (text == "Binary Search Tree")
                t.Add(x);
                else if (text == "AVL")
                t0.Add(x);

                first = false;
                MessageBox.Show("Great ! Now insert the second value to start having fun ");
                 goto end;
            }
            if (text == "AVL")
            {
                LayoutAlgorithmType = "EfficientSugiyama";
                init();
                t0.Add(x);
                DisplayTree();
                fixitupbaby();
                usepre();
            }
            else if (text == "Binary Search Tree")
            {
                LayoutAlgorithmType = "EfficientSugiyama";
                init();
                t.Add(x);
                DisplayTree1();
                fixitupbaby1();
                usepre1(); 
            }
            else if (text == "Binary Tree")
            {
                LayoutAlgorithmType = "EfficientSugiyama";
                if (list.Contains(x))
                    MessageBox.Show("This node is already in the tree !");
                else
                {
                        existingVertices = new List<PocVertex>();
                        Graph = new PocGraph(true);              
                        list.Add(x);
                        try
                        {
                            map.Add(x, 0);
                        }
                        catch (System.ArgumentException)
                        {
                        }
                        catch (System.OutOfMemoryException)
                        {
                        }
                        fixitupbaby2();
                        pre2();
                    }

                
            }
            else if (text == "Linked List")
            {
                LayoutAlgorithmType = "EfficientSugiyama";
                existingVertices = new List<PocVertex>();
                Graph = new PocGraph(true);
                list.Add(x);
                list.Sort();
                for (int i = 0; i < list.Count; i++)
                    existingVertices.Add(new PocVertex((list[i]).ToString(), true));

                foreach (PocVertex vertex in existingVertices)
                    Graph.AddVertex(vertex);

                for(int i=0;i<list.Count-1;i++)
                    AddNewGraphEdge(existingVertices[i], existingVertices[i+1]);

            }
            else if (text == "Stack")
            {
                LayoutAlgorithmType = "EfficientSugiyama";
                existingVertices = new List<PocVertex>();
                Graph = new PocGraph(true);
                list.Add(x);
                for (int i = list.Count - 1; i >= 0; i--)
                    existingVertices.Add(new PocVertex((list[i]).ToString(), true));

                foreach (PocVertex vertex in existingVertices)
                    Graph.AddVertex(vertex);

                for (int i = 0; i < list.Count - 1; i++)
                    AddNewGraphEdge(existingVertices[i], existingVertices[i + 1]);

            }
            else if (text == "Queue")
            {
                LayoutAlgorithmType = "EfficientSugiyama";
                existingVertices = new List<PocVertex>();
                Graph = new PocGraph(true);
                list.Add(x);
                for (int i = 0; i < list.Count; i++)
                    existingVertices.Add(new PocVertex((list[i]).ToString(), true));

                foreach (PocVertex vertex in existingVertices)
                    Graph.AddVertex(vertex);

                for (int i = 0; i < list.Count - 1; i++)
                    AddNewGraphEdge(existingVertices[i], existingVertices[i + 1]);

            }
            else if (text == "Graph")
            {
                LayoutAlgorithmType = "CompoundFDP";
               
                if (firstGraph)
                {
                    i = 0;
                  //  G=new List<List<int>>(100);
                    for (int u = 0; u < 100; u++)
                    {
                        visited[u] = false;
                        List<int> sublist = new List<int>();
                        G.Add(sublist);
                    }
                    existingVertices = new List<PocVertex>();
                    Graph = new PocGraph(true);
                    firstGraph = false;
                }
                if (!list.Contains(x))
                {
                    list.Add(x);
                    map.Add(x, i++);
                    existingVertices.Add(new PocVertex(x.ToString(), true));

                }
                if (!list.Contains(y))
                {
                    list.Add(y);
                    map.Add(y, i++);
                    existingVertices.Add(new PocVertex(y.ToString(), true));
                }
                G[map[x]].Add(map[y]);
                val[map[x]] = x;
                val[map[y]] = y;

                foreach (PocVertex vertex in existingVertices)
                    Graph.AddVertex(vertex);

                AddNewGraphEdge(existingVertices[map[x]], existingVertices[map[y]]);


            }

            end:
            NotifyPropertyChanged("Graph");

        }
        public void Delete(int x , string text)
        {
            bool found = false;
            if (text == "AVL")
            {
                init();
                t0.Remove(x);
                DisplayTree();
                fixitupbaby();
                usepre();
            }
           else if (text == "Binary Search Tree")
           {
               init();
               t.Remove(x);
               DisplayTree1();
               fixitupbaby1();
               usepre1();
           }
           else if (text == "Binary Tree")
           {

               for (int i = 0; i < list.Count; i++)
               {
                   if (list[i] == x)
                   {
                       found = true;
                       break;
                   }
               }
               if(!found)
                   MessageBox.Show("Not Found !");
               else
               {

                   u = 0;
                   existingVertices = new List<PocVertex>();
                   Graph = new PocGraph(true);
                   list.Remove(x);
                   map.Remove(x);
                   fixitupbaby2();
                   pre2();
               }
           }
            else if (text == "Linked List")
            {
           
                    existingVertices = new List<PocVertex>();
                    Graph = new PocGraph(true);
                    list.Remove(x);
                    list.Sort();
                    for (int i = 0; i < list.Count; i++)
                        existingVertices.Add(new PocVertex((list[i]).ToString(), true));

                    foreach (PocVertex vertex in existingVertices)
                        Graph.AddVertex(vertex);

                    for (int i = 0; i < list.Count - 1; i++)
                        AddNewGraphEdge(existingVertices[i], existingVertices[i + 1]);

                
            }
            else if (text == "Stack")
            {

                existingVertices = new List<PocVertex>();
                Graph = new PocGraph(true);
                list.RemoveAt(list.Count-1);
                for (int i = list.Count - 1; i >= 0; i--)
                    existingVertices.Add(new PocVertex((list[i]).ToString(), true));

                foreach (PocVertex vertex in existingVertices)
                    Graph.AddVertex(vertex);

                for (int i = 0; i < list.Count - 1; i++)
                    AddNewGraphEdge(existingVertices[i], existingVertices[i + 1]);

            }
            else if (text == "Queue")
            {
                existingVertices = new List<PocVertex>();
                Graph = new PocGraph(true);
                list.RemoveAt(0);
                for (int i = 0; i < list.Count; i++)
                    existingVertices.Add(new PocVertex((list[i]).ToString(), true));

                foreach (PocVertex vertex in existingVertices)
                    Graph.AddVertex(vertex);

                for (int i = 0; i < list.Count - 1; i++)
                    AddNewGraphEdge(existingVertices[i], existingVertices[i + 1]);
            }

            NotifyPropertyChanged("Graph");

        }

         public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs 
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.   
                    //OAEP padding is only available on Microsoft Windows XP or 
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

         public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs 
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.   
                    //OAEP padding is only available on Microsoft Windows XP or 
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }

        #region Private Methods
        public  PocEdge AddNewGraphEdge(PocVertex from, PocVertex to)
        {
            string edgeString = string.Format("{0}-{1} Connected", from.ID, to.ID);

            PocEdge newEdge = new PocEdge(edgeString, from, to);
            Graph.AddEdge(newEdge);
            return newEdge;
        }


        #endregion

        #region Public Properties

        public List<String> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }


        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }



        public  PocGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                MainWindowViewModel a;
                
                NotifyPropertyChanged("Graph");
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
