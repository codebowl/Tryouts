using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    /// <summary>
    /// BinaryTree tryouts
    /// </summary>
    public class Sample3
    {
        public static void Do()
        {
            Tree tree = new Tree();
            Node root = null;

            foreach (var value in new[] { 4, 2, 6, 3 })
            {
                root = tree.Insert(root, value);
            }

            tree.Traverse(root);

            Console.ReadLine();
        }

        class Node
        {
            public int Value { get; set; }
            public Node Right { get; set; }
            public Node Left { get; set; }
            public int Depth { get; set; }

            public override string ToString()
            {
                return $"V: {GetString(this)}  L: {GetString(Left)}  R: {GetString(Right)}";

                string GetString(Node node)
                {
                    if (node != null)
                    {
                        return node.Value.ToString();
                    }

                    return string.Empty;
                }
            }
        }

        class Tree
        {
            public Node Insert(Node root, int value)
            {
                if (root == null)
                {
                    root = new Node();
                    root.Value = value;
                    return root;
                }

                if (value < root.Value)
                {
                    root.Left = Insert(root.Left, value);
                }
                else
                {
                    root.Right = Insert(root.Right, value);
                }

                return root;
            }

            public void Traverse(Node root)
            {
                if (root == null) return;

                Console.WriteLine(root.Value);
                Traverse(root.Left);
                Traverse(root.Right);
            }
        }

        class Treex
        {
            public Node Insert(Node root, int value, int depth = 0)
            {
                if (root == null)
                {
                    root = new Node
                    {
                        Value = value,
                        Depth = depth
                    };
                }
                else if (value < root.Value)
                {
                    root.Left = Insert(root.Left, value, depth + 1);
                }
                else
                {
                    root.Right = Insert(root.Right, value, depth + 1);
                }

                return root;
            }

            public void Traverse(Node root)
            {
                if (root == null) return;

                Console.WriteLine("V: " + root.Value + " D: " + root.Depth);
                Traverse(root.Left);
                Traverse(root.Right);
            }
        }

        class BinaryTree<T>
        {
            private readonly IComparer<T> _comparer;
            private BinaryTree<T> Right;
            private BinaryTree<T> Left;

            private T Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    HasValue = true;
                }
            }

            private bool HasValue;
            private T _value;

            public BinaryTree(IComparer<T> comparer)
            {
                _comparer = comparer;
            }

            public void Load(BinaryTree<T> tree, T[] values, int index)
            {
                if (values.Length <= index) return;

                if (tree.HasValue)
                {
                    var result = _comparer.Compare(tree.Value, values[index]);
                    if (result > 0)
                    {
                        tree.Left = tree.Left ?? new BinaryTree<T>(_comparer) { Value = values[index] };
                        Load(tree, values, index + 1);
                    }
                    else
                    {
                        tree.Right = tree.Right ?? new BinaryTree<T>(_comparer) { Value = values[index] };
                        Load(tree, values, index + 1);
                    }

                }
                else
                {
                    Value = values[index];
                    index++;
                    if (values.Length > index)
                    {
                        var result = _comparer.Compare(Value, values[index]);

                        if (result < 0)
                        {
                            Right = new BinaryTree<T>(_comparer) { Value = values[index] };
                            Load(tree, values, index + 1);
                        }
                        else
                        {
                            Left = new BinaryTree<T>(_comparer) { Value = values[index] };
                            Load(tree, values, index + 1);
                        }
                    }
                }
            }

            public override string ToString()
            {
                return $"V: {GetString(this)}  L: {GetString(Left)}  R: {GetString(Right)}";

                string GetString(BinaryTree<T> node)
                {
                    if (node != null && node.HasValue)
                    {
                        return node.Value.ToString();
                    }

                    return string.Empty;
                }
            }
        }
    }
}