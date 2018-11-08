using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test;

namespace Test
{
    class Node : IComparable
    {
        public char Character { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(char Character, int Frequency, Node Left, Node Right)
        {
            this.Character = Character;
            this.Frequency = Frequency;
            this.Left = Left;
            this.Right = Right;
        }


        public int CompareTo(Object obj)
        {

            Node f = (Node)obj;

            return f.Frequency.CompareTo(this.Frequency);

            //if (f.Frequency > Frequency)
            //    return 1;
            //else
            //    if (f.Frequency < Frequency)
            //        return -1;
            //    else
            //        if (f.Frequency == Frequency)
            //            return 0;
            //        else
            //            throw new Exception("you cannot compare");
        }
    }
}


class Huffman
{
    private Node HT;                               //Huffman tree to create codes and decode
    private Dictionary<char, string> D = new Dictionary<char, string>();            //dictionary to encode text

    private string message;                                                     //stores the text as a string
    string bits = "";                                                          //text converted to 0s and 1s

    //constructor
    public Huffman(string S)
    {
        message = S;

        Build(AnalyzeText());       //invokes the Buld();
        CreateCodes(HT, bits);      //invokes the createCodes()
    }


    //returns the frequency of each character in the given text(invoked by Huffman)
    private int[] AnalyzeText()
    {
        //array that contains the frequency of letters in the text
        int[] Container = new int[255];

        //aloop that goes through the text to coun the number of frequencies
        foreach (char m in message)
        {
            Container[(int)m]++;
        }

        //returns the array the frequency of each letters 
        return Container;
    }

    //build a huffman tree 
    private void Build(int[] F)
    {
        //creates nodes left and right
      Node left, Right;

        //creates priority queue
        PriorityQueue<Node> PQ = new PriorityQueue<Node>(255);

        //goes through the array and creates a node for each letter
        for (int i = 32; i < F.Length; i++)
        {
            if (F[i] > 0)            //the conditions is to go through the array and obtain the letters with the frequency of atleast 1
            {
                PQ.Add(new Node((char)i, F[i], null, null));
            }
        }

        //it checks wether the priority queue has only one node 
        if (PQ.Size() == 1)
        {
            HT = PQ.Front();
        }

        //builds a binary tree 
        else
        {
            while (PQ.Size() > 1)
            {
                left = PQ.Front();
                PQ.Remove();
                Right = PQ.Front();
                PQ.Remove();
                PQ.Add(new Node('/', left.Frequency + Right.Frequency, left, Right));
            }
            HT = PQ.Front();
        }


    }

    //creates the code of 0s and 1s for each character by transversing the Huffman tree(invoked by Huffman)
    private void CreateCodes(Node HT, string bits)
    {
        //creates a new node and assigns it to the root of the root of the priority queue
        Node current = HT;

        //traverses through the binary tree
        if (current.Left != null && current.Right != null)
        {
            CreateCodes(current.Left, bits + "0");
            CreateCodes(current.Right, bits + "1");
        }

        else
        {
            D.Add(current.Character, bits);

        }

    }

    //encode the given text and return a string of 0s and 1s
    public string Encode(string S)
    {
        string encode = "";

        foreach (char item in S)
        {
            char m = (char)item;
            foreach (KeyValuePair<char, string> val in D)
            {
                if (m == val.Key)
                    encode = encode + val.Value;
            }
        }



        return encode;
    }

    //Decode the given string of 0s and 1s and return original text
    public string Decode(string S)
    {


        Node current = HT;
        string decode = "";

        foreach (char ch in S)
        {
            if (current.Left == null)
            {
                decode += current.Character;
                current = HT;
            }
            else
            {
                if (ch == '0')
                    current = current.Left;
                else
                    current = current.Right;
            }
            if (current.Left == null)
            {
                decode += current.Character;
                current = HT;
            }
        }

        return decode;
    }
}

// Common interface for ALL non-linear data structures

public interface IContainer<T>
{
    void MakeEmpty();  // Reset an instance to empty
    bool Empty();      // Test if an instance is empty
    int Size();        // Return the number of items in an instance
}

//-----------------------------------------------------------------------------

public interface IPriorityQueue<T> : IContainer<T> where T : IComparable
{
    void Add(T item);  // Add an item to a priority queue
    void Remove();     // Remove the item with the highest priority
    T Front();         // Return the item with the highest priority
}

//-------------------------------------------------------------------------

// Priority Queue
// Implementation:  Binary heap

public class PriorityQueue<T> : IPriorityQueue<T> where T : IComparable
{
    private int capacity;  // Maximum number of items in a priority queue
    private T[] A;         // Array of items
    private int count;     // Number of items in a priority queue

    public PriorityQueue(int size)
    {
        capacity = size;
        A = new T[size + 1];  // Indexing begins at 1
        count = 0;
    }

    // Percolate up from position i in a priority queue

    private void PercolateUp(int i)
    // (Worst case) time complexity: O(log n)
    {
        int child = i, parent;

        while (child > 1)
        {
            parent = child / 2;
            if (A[child].CompareTo(A[parent]) > 0)
            // If child has a higher priority than parent
            {
                // Swap parent and child
                T item = A[child];
                A[child] = A[parent];
                A[parent] = item;
                child = parent;  // Move up child index to parent index
            }
            else
                // Item is in its proper position
                return;
        }
    }

    public void Add(T item)
    // Time complexity: O(log n)
    {
        if (count < capacity)
        {
            A[++count] = item;  // Place item at the next available position
            PercolateUp(count);
        }
    }

    // Percolate down from position i in a priority queue

    private void PercolateDown(int i)
    // Time complexity: O(log n)
    {
        int parent = i, child;

        while (2 * parent <= count)
        // while parent has at least one child
        {
            // Select the child with the highest priority
            child = 2 * parent;    // Left child index
            if (child < count)  // Right child also exists
                if (A[child + 1].CompareTo(A[child]) > 0)
                    // Right child has a higher priority than left child
                    child++;

            if (A[child].CompareTo(A[parent]) > 0)
            // If child has a higher priority than parent
            {
                // Swap parent and child
                T item = A[child];
                A[child] = A[parent];
                A[parent] = item;
                parent = child;  // Move down parent index to child index
            }
            else
                // Item is in its proper place
                return;
        }
    }

    public void Remove()
    // Time complexity: O(log n)
    {
        if (!Empty())
        {
            // Remove item with highest priority (root) and
            // replace it with the last item
            A[1] = A[count--];

            // Percolate down the new root item
            PercolateDown(1);
        }
    }

    public T Front()
    // Time complexity: O(1)
    {
        if (!Empty())
        {
            return A[1];  // Return the root item (highest priority)
        }
        else
            return default(T);
    }

    // Create a binary heap
    // Percolate down from the last parent to the root (first parent)

    private void BuildHeap()
    // Time complexity: O(n)
    {
        int i;
        for (i = count / 2; i >= 1; i--)
        {
            PercolateDown(i);
        }
    }

    // Sorts and returns the InputArray

    public void HeapSort(T[] inputArray)
    // Time complexity: O(n log n)
    {
        int i;

        capacity = count = inputArray.Length;

        // Copy input array to A (indexed from 1)
        for (i = capacity - 1; i >= 0; i--)
        {
            A[i + 1] = inputArray[i];
        }

        // Create a binary heap
        BuildHeap();

        // Remove the next item and place it into the input (output) array
        for (i = 0; i < capacity; i++)
        {
            inputArray[i] = Front();
            Remove();
        }
    }

    public void MakeEmpty()
    // Time complexity: O(1)
    {
        count = 0;
    }

    public bool Empty()
    // Time complexity: O(1)
    {
        return count == 0;
    }

    public int Size()
    // Time complexity: O(1)
    {
        return count;
    }
}

//-------------------------------------------------------------------------

// Used by class PriorityQueue<T>
// Implements IComparable and overrides ToString (from Object)

public class PriorityClass : IComparable
{
    private int priorityValue;
    private String name;

    public PriorityClass(int priority, String name)
    {
        this.name = name;
        priorityValue = priority;
    }

    public int CompareTo(Object obj)
    {
        PriorityClass other = (PriorityClass)obj;   // Explicit cast
        return priorityValue - other.priorityValue;
    }

    public override string ToString()
    {
        return name + " with priority " + priorityValue;
    }
}

//-----------------------------------------------------------------------------

class Program
{
    static void Main(string[] args)
    {
        //asks the user to enter a text
        Console.Write("Enter a word or a text:  ");
        string input = Convert.ToString(Console.ReadLine());

        Huffman insta = new Huffman(input);

        //encodes the text
        Console.WriteLine(insta.Encode(input));

        //decodes
        Console.Write(insta.Decode(insta.Encode(input)));

        Console.ReadLine();
    }

}