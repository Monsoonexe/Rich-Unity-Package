using System;
using System.Collections.Generic;

namespace RichPackage
{
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

        public static bool TryFindAndRemoveNode<T>(
            this LinkedList<T> linkedList,
            Predicate<T> query, out LinkedListNode<T> foundNode)
        {
            foundNode = null; //return values
            LinkedListNode<T> currentNode = linkedList.First;

            while (currentNode != null)
            {
                if (query(currentNode.Value))
                {
                    foundNode = currentNode;
                    linkedList.Remove(foundNode);
                    break;
                }
                currentNode = currentNode.Next; //go to next element
            } 

            return foundNode != null;
        }

        public static bool TryFindAndRemove<T>(
            this LinkedList<T> linkedList,
            Predicate<T> query, out T foundItem)
        {
            foundItem = default;
            bool found = false; // return value
            LinkedListNode<T> currentNode = linkedList.First;

            while (currentNode != null)
            {
                if (query(currentNode.Value))
                {
                    found = true;
                    foundItem = currentNode.Value;
                    linkedList.Remove(currentNode);
                    break;
                }
                currentNode = currentNode.Next; //go to next element
            } 

            return found;
        }
    }
}
