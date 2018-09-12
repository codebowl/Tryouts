using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    /// <summary>
    /// LinkedList LRU Cache O(1) with reveres
    /// </summary>
    public class Sample5
    {
        class Node
        {
            public int Value { get; set; }
            public Node Next { get; set; }
            public Node Previous { get; set; }

            public override string ToString()
            {
                return $"V: {GetString(this)}  P: {GetString(Previous)}  N: {GetString(Next)}";

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

        private Dictionary<int, Node> items = new Dictionary<int, Node>();
        private Node Head;
        private Node Tail;
        private readonly int _size;

        public Sample5(int size)
        {
            _size = size;
        }

        public void Add(int value)
        {
            if (items.Count == _size)
                RemoveTail();

            var item = new Node();
            item.Value = value;
            items.Add(value, item);

            // first item
            if (Head == null)
            {
                Head = item;
            }
            else
            {
                item.Next = Head;
                Head.Previous = item;
                Head = item;
            }

            if (Tail == null)
                Tail = Head;
        }

        public int Get(int value)
        {
            var node = items[value];
            MoveToHead(node);
            return node.Value;
        }

        void MoveToHead(Node node)
        {
            if (node == Head) return;

            if (Tail == node)
                Tail = node.Previous;

            var previous = node.Previous;
            var next = node.Next;

            if (previous != null) previous.Next = next;
            if (next != null) next.Previous = previous;

            Head.Previous = node;
            node.Next = Head;
            Head = node;
            Head.Previous = null;
        }

        void RemoveTail()
        {
            items.Remove(Tail.Value);
            var newTail = Tail.Previous;
            newTail.Next = null;
            Tail = newTail;
        }

        void Print()
        {
            Console.WriteLine();
            var node = Head;
            while (node != null)
            {
                Console.Write(node.Value + " >> ");
                node = node.Next;
            }

            Console.WriteLine();

            node = Tail;
            while (node != null)
            {
                Console.Write(node.Value + " >> ");
                node = node.Previous;
            }

            Console.WriteLine();
        }

        private void Reverse()
        {
            var movingTail = Tail;
            while (movingTail != Head)
            {
                Swap(Head, movingTail);
            }
        }

        private void Swap(Node movingNode, Node tail)
        {
            // make head.next as the new head
            var newHead = movingNode.Next;
            newHead.Previous = null;
            Head = newHead;

            // push the head after the current tail. if current moving tail isn't the last element, keep the connections
            movingNode.Previous = tail;
            tail.Next = movingNode;
            var tailNext = tail.Next;
            if (tailNext != null) tailNext.Previous = movingNode;
            movingNode.Next = tailNext;

            if (Tail.Next != null)
                Tail = Tail.Next;
        }

        private void Swap(Node node)
        {
            var previous = node.Previous;
            var next = node.Next;

            previous.Previous = next;
            next.Next = previous;
        }

        public static void Do()
        {
            Sample5 sample5 = new Sample5(3);
            Console.WriteLine("Empty");
            //sample5.Print();
            Console.WriteLine("Add 4");
            sample5.Add(4);
            //sample5.Print();
            Console.WriteLine("Add 5");
            sample5.Add(5);
            //sample5.Print();
            Console.WriteLine("Add 6");
            sample5.Add(6);
            sample5.Print();
            Console.WriteLine("Reverse");
            sample5.Reverse();
            sample5.Print();
            Console.WriteLine("Get 4");
            sample5.Get(4);
            sample5.Print();
            Console.WriteLine("Add 7");
            sample5.Add(7);
            sample5.Print();
            sample5.Reverse();
        }
    }
}