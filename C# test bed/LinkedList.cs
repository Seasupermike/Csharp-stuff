using System.Collections;
using System.Collections.Generic;

namespace LinkedList
{
    public class TestClass
    {
        public static void Test()
        {
            
        }
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        public readonly ListNode<T> Head;

        public LinkedList(T inital = default)
        {
            Head = new(inital);
        }

        public ListNode<T>? this[int index]
        {
            get
            {
                if (index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
                int i = 0;
                for (ListNode<T>? Current = Head; Current is not null; Current = Current.Next)
                {
                   if (i == index) return Current;
                }
                return null;
            }
        }

        public IEnumerator<ListNode<T>> GetEnumerator()
        {
            for (ListNode<T>? Current = Head; Current is not null; Current = Current.Next)
            {
                yield return Current;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ListNode<T>
    {
        public T Value;
        public ListNode<T>? Next;

        public ListNode(T inital)
        {
            Value = inital;
        }
    }
}