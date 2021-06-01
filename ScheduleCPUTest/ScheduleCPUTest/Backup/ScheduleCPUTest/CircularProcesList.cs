using System.Collections.Generic;

/// <summary> 
/// This class implements a circular list using the LinkedList class
/// Note that elements in a LinkedList with n elements are numbered 0 .. (n-1)
/// </version>

public class CircularProcesList
{
    private LinkedList<IProcess> list;

    /// <summary> 
    /// Creates a new LinkedList to contain Testproces-ses
    /// </summary>
    public CircularProcesList()
    {
        this.list = new LinkedList<IProcess>();
    }

    /// <summary>
    /// Check whether the queue is empty
    /// </summary>
    public bool Empty
    {
        get
        {
            return list.Count == 0;
        }
    }

    /// <summary> 
    /// Retrieves the next TestProces from the list
    /// </summary>
    public IProcess Next
    {
        get
        {
            LinkedListNode<IProcess> first = this.list.First;
            if (first != null)
            {
                IProcess nextElement = first.Value;
                this.list.RemoveFirst();
                return nextElement;
            }
            else
            { 
                return null; 
            }
        }

    }

    /// <summary> 
    /// Adds a TestProces to the list
    /// </summary>
    /// <param name="t"> The Testproces to be added</param>
    public void AddItem(IProcess t)
    {
        list.AddLast(t);
    }

    /// <summary> 
    /// Deletes a TestProces from the list
    /// </summary>
    /// <param name="t"> The TestProces to be deleted</param>
    public void deleteItem(IProcess t)
    {
        list.Remove(t);
    }
}