using System;
using System.Collections.Generic;

public static class LinkedList_Extensions
{
    public static T Find<T>(this LinkedList<T> linkedList,
        Predicate<T> query)
    {
        foreach (T item in linkedList)
        {
            if (query(item))
                return item;
        }
        return default;
    }

    public static bool TryFind<T>(this LinkedList<T> linkedList,
        Predicate<T> query, out T foundItem)
    {
        bool found = false; //return value
        foundItem = default;

        foreach (T item in linkedList)
        {
            if (query(item))
            {
                found = true;
                foundItem = item;
                break;
            }
        }

        return found;
    }

    public static bool TryFindAndRemove<T>(
        this LinkedList<T> linkedList,
        Predicate<T> query, out T foundItem)
    {
        foundItem = default;
        bool found = false; // return value

        if (linkedList.Count <= 0)
            return false; //early exit

        LinkedListNode<T> currentNode = linkedList.First;

        do
        {
            if (query(currentNode.Value))
            {
                found = true;
                foundItem = currentNode.Value;
                linkedList.Remove(currentNode);
                break;
            }
        } while (currentNode.Next != null);

        return found;
    }
}
