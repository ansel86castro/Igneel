using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Collections
{
    public static class Utils
    {
        public static LinkedListNode<T> GetLinkedNode<T>(this LinkedList<T>list, Predicate<T>condition)
        {
            LinkedListNode<T> node = null;
            LinkedListNode<T> current = list.First;
            while (current != null)
            {
                if (condition(current.Value))
                {
                    node = current;
                    break;
                }
                current = current.Next;
            }
            return node;
        }
    }
}
