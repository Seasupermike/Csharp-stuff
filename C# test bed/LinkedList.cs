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
        public readonly Link<T> Head;

        public LinkedList(T inital = default)
        {
            Head = new(inital);
        }

        public Link<T>? this[int Index]
        {
            get
            {
                if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
                Link<T> Current = Head;
                for (int i = 0; i < Index + 1; i++)
                {
                    if (i == Index) return Current;
                }
                return null;
            }
            set
            {

            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (Link<T>? Current = Head; Current is not null; Current = Current.Next)
            {
                yield return Current.Value;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Link<T>
    {
        public T Value;
        public Link<T>? Next;

        public Link(T inital)
        {
            Value = inital;
        }
    }
}