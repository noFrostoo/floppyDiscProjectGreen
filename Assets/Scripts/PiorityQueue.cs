using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FloppyDiscProjectGreen
{
namespace DataStructures
{
public class PiorityQueueHeap<T> where T : IComparable<T>
{
    List<T> items;
    int pos;
    int size;
    public PiorityQueueHeap(List<T> initialItems)
    {
        foreach(var item in initialItems)
            Enqueue(item);

    }

    public void Enqueue (T itemToInsert)
    {
        items.Add(itemToInsert);
        pos = size;
        size += 1;

        int parentPos =  GetParent(pos);
        while(items[pos].CompareTo(items[parentPos]) == -1)
        {
            Swap(pos, parentPos);
            pos = parentPos;
            parentPos = GetParent(pos);
        }
    }

    public T Dequeue()
    {
        if(size == 0)
            return default(T);
        // else if(size == 1)
        // {
        //     DataNode<T> item = items[0];
        //     items.RemoveAt(0);
        //     size--;
        //     return item.data;
        // }
        else 
        {
            T min = items[0];
            size--;
            items[0] = items[size];
            items.RemoveAt(size);
            MinHeapify(pos);
            return min;
        }
    }

    private void MinHeapify(int pos)
    {
        int leftC = GetLeftChild(pos);
        int rightC = GetRightChild(pos);

        int lowestPiority = pos;
        if ( leftC < size && items[lowestPiority].CompareTo(items[leftC]) >= 0)
            lowestPiority = leftC;
        if ( rightC < size && items[lowestPiority].CompareTo(items[rightC]) >= 0)
            lowestPiority = rightC;
        
        if( lowestPiority != pos)
        {
            Swap(pos, lowestPiority);
            MinHeapify(lowestPiority);
        }

        // while(!IsLeaf(pos))
        // {
        //     if(GreaterThanAnyChild(pos))
        //     {
        //         if(items[GetRightChild(pos)] > items[GetLeftChild(pos)])
        //         {
        //             Swap(items[pos], items[GetLeftChild(pos)]);
        //             pos = GetLeftChild(pos);
        //         }else
        //         {
        //             Swap(items[pos], items[GetRightChild(pos)]);
        //             pos = GetRightChild(pos);
        //         }
        //     }
        // }
    }

    private bool IsLeaf(int pos)
    {
        return pos <= (size-1/2) && pos <= size;
    }

    private bool GreaterThanAnyChild(int pos)
    {
        return items[pos].CompareTo(items[GetRightChild(pos)]) == 1 || items[pos].CompareTo(items[GetLeftChild(pos)]) == 1;
    }
    private int GetParent(int itemPos)
    {
        if(itemPos == 0)
            return 0;
        return (itemPos - 1)/2;

    }

    private int GetLeftChild(int itemPos)
    {
        if(itemPos == 0)
            return 1;
        int leftChildPos = 2*itemPos + 1;
        // if(leftChildPos >= size)
        //     throw new IndexOutOfRangeException();
        return leftChildPos;
    }

    private int GetRightChild(int itemPos)
    {
        if(itemPos == 0)
            return 1;
        int leftChildPos = 2*itemPos + 2;
        // if(leftChildPos >= size)
        //     throw new IndexOutOfRangeException();
        return leftChildPos;
    }

    private void Swap(int pos1, int pos2)
    {
        T tempData = items[pos2];

        items[pos2] = items[pos1];
        items[pos1] = tempData;

    }

}


}
}
