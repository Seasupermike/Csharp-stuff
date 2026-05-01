using System.Collections;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Collections
{
    public class TestClass
    {
        public static void Test()
        {
           
        }
    }

    public interface ICatalog<T> : ICollection<T>, IList<T>
    {
        int Capacity { get; }
        void AddRange(IEnumerable<T> Values);
        void RemoveAt(int Index);
        T RemoveAndGet(int Index);
        int IndexOf(T Target);
        T[] ToArray();
        List<T> ToList();
        T GetRandom();
        public void EnsureMinimumCapacity(int NeededCapacity);
    }

    public class Catalog<T> : ICatalog<T>
    {
        private T[] Items;
        public int Capacity => Items.Length;
        public int Count { get; set; }
        public bool IsReadOnly { get; }

        public Catalog(int Capacity = 0, bool IsReadOnly = false)
        {
            Items = new T[Capacity < 0 ? 0 : Capacity];
            this.IsReadOnly = IsReadOnly;
        }

        public Catalog(IEnumerable<T> Collection, bool IsReadOnly = false)
        {
            if (Collection.TryGetNonEnumeratedCount(out int count))
            {
                Items = new T[count];
                int i = 0;
                foreach (T item in Collection)
                {
                    Items[i] = item;
                    i++;
                }
            } else
            {
                Items = new T[1];
                int i = 0;
                foreach (T item in Collection)
                {
                    EnsureMinimumCapacity(i + 1);
                    Items[i] = item;
                    i++;
                }
            }
            this.IsReadOnly = IsReadOnly;
        }

        public T this[int Index]
        {
            get
            {
                if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
                if (Index >= Count) throw new IndexOutOfRangeException("Index must be less than Count.");
                return Items[Index];
            }
            set
            {
                if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
                if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
                if (Index >= Count) throw new IndexOutOfRangeException("Index must be less than Count.");
                Items[Index] = value;
            }
        }

        public T[] this[int StartIndex, int EndIndex]
        {
            get
            {
                if (StartIndex < 0) throw new IndexOutOfRangeException("StartIndex must be greater than or equal to 0.");
                if (StartIndex >= Count) throw new IndexOutOfRangeException("StartIndex must be less than Count.");
                if (EndIndex < 0) throw new IndexOutOfRangeException("EndIndex must be greater than or equal to 0.");
                if (EndIndex > Count) throw new IndexOutOfRangeException("EndIndex must be less than or equal to Count.");
                T[] Result = new T[EndIndex - StartIndex + 1];
                int Index = 0;
                for (int i = StartIndex; i < EndIndex; i++)
                {
                    Items[Index] = Items[i];
                    Index++;
                }

                return Result;
            }
        }

        public void Add(T Item)
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            EnsureMinimumCapacity(Count + 1);
            Items[Count] = Item;
            Count++;
        }

        public void AddRange(IEnumerable<T> Collection)
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            if (Collection.TryGetNonEnumeratedCount(out int count))
            {
                EnsureMinimumCapacity(Count + count);
            }
            else
            {
                EnsureMinimumCapacity(Count + Collection.Count());
            }

            foreach (T Item in Collection)
            {
                Items[Count] = Item;
                Count++;
            }
        }

        public void Insert(int Index, T Value)
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
            if (Index >= Count) throw new IndexOutOfRangeException("Index must be less than or equal to Count.");
            EnsureMinimumCapacity(Count + 1);
            for (int i = Count; i > Index; i--)
            {
                Items[i] = Items[i - 1];
            }

            Items[Index] = Value;
            Count++;
        }

        public void RemoveAt(int Index)
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
            if (Index >= Count) throw new IndexOutOfRangeException("Index must be less than Count.");
            for (; Index < Count - 1; Index++)
            {
                Items[Index] = Items[Index + 1];
            }
            Items[Count] = default!;
            Count--;
        }

        public T RemoveAndGet(int Index)
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
            if (Index >= Count) throw new IndexOutOfRangeException("Index must be less than Count.");
            T Result = Items[Index];
            for (; Index < Count - 1; Index++)
            {
                Items[Index] = Items[Index + 1];
            }
            Items[Count] = default!;
            Count--;
            return Result;
        }

        public bool Remove(T Target)
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            for (int i = 0; i < Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(this[i], Target))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            Items = new T[Items.Length];
            Count = 0;
        }

        public int IndexOf(T Target)
        {
            for (int i = 0; i < Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(Items[i], Target))
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(T Target)
        {
            for (int i = 0; i < Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(Items[i], Target))
                {
                    return true;
                }
            }

            return false;
        }

        public T[] ToArray()
        {
            T[] Result = new T[Count];
            Array.Copy(Items, Result, Count);
            return Result;
        }

        public List<T> ToList()
        {
            List<T> Result = new List<T>(Capacity);
            foreach (T Item in Items)
            {
                Result.Add(Item);
            }
            return Result;
        }

        public Catalog<T> Clone(bool isReadOnly = false) => new(this, isReadOnly);

        public void CopyTo(T[] DestinationArray, int StartingIndex)
        {
            try
            {
                Items.CopyTo(DestinationArray, StartingIndex);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>Gets random item in Catalog</summary>
        public T GetRandom()
        {
            if (Count == 0) throw new InvalidOperationException($"Catalog<{typeof(T)}> contains no elements.");
            return Items[Random.Shared.Next(Count)];
        }

        /// <summary>Returns a represation of the Catalog</summary>
        public override string ToString()
        {
            if (Count == 0) return $"Catalog<{typeof(T)}>(0)";

            StringBuilder sb = new StringBuilder($"Catalog<{typeof(T)}>({Count}) {"{"} ");
            switch (typeof(T).ToString())
            {
                case "System.String":
                    for (int i = 0; i < Count; i++)
                    {
                        T item = Items[i];

                        if (item is null)
                        {
                            sb.Append($"null");
                        }
                        else
                        {
                            sb.Append($"\"{item}\"");
                        }

                        if (i < Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    break;
                case "System.Char":
                    for (int i = 0; i < Count; i++)
                    {
                        T item = Items[i];

                        if (item is null)
                        {
                            sb.Append($"null");
                        }
                        else
                        {
                            sb.Append($"'{Items[i]}'");
                        }

                        if (i < Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    break;
                case "System.Object":
                    for (int i = 0; i < Count; i++)
                    {
                        T item = Items[i];
                        if (item is null)
                        {
                            sb.Append("null");
                            if (i < Count - 1)
                            {
                                sb.Append(", ");
                            }
                            continue;
                        }

                        string type = item!.GetType().ToString();


                        if (type == "System.String")
                        {
                            sb.Append($"\"{item}\"");
                        }
                        else if (type == "System.Char")
                        {
                            sb.Append($"'{item}'");
                        }
                        else
                        {
                            sb.Append(item);

                        }

                        if (i < Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    break;
                default:
                    for (int i = 0; i < Count; i++)
                    {
                        if (Items[i] is null)
                        {
                            sb.Append("null");
                        }
                        else
                        {
                            sb.Append(Items[i]!.ToString());
                        }

                        if (i < Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    break;
            }
            return sb.Append(" }").ToString();
        }

        public void EnsureMinimumCapacity(int NeededCapacity)
        {
            if (NeededCapacity >= Items.Length)
            {
                Array.Resize(ref Items, (NeededCapacity) * 2);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in Items) yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool EqualContents(Catalog<T> CatalogA, Catalog<T> CatalogB)
        {
            if (CatalogA.Count != CatalogB.Count) return false;
            for (int i = 0; i < CatalogA.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(CatalogA[i], CatalogB[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Selection<T> : IEnumerable<T> where T : class
    {
        private T[] _Items;
        private Dictionary<Type, int> _ItemsLocation;
        public int Count { get; private set; }

        public int Current
        {
            get;
            set
            {
                if (value < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
                if (value >= Count) throw new IndexOutOfRangeException("Index must be less than Count.");
                field = value;
            }
        }
        public T Selected => _Items[Current];

        public Selection(int Capacity = 0)
        {
            Capacity = Capacity < 0 ? 0 : Capacity;
            _Items = new T[Capacity];
            _ItemsLocation = new(Capacity);

        }

        public T this[int Index]
        {
            get
            {
                if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
                if (Index >= Count) throw new IndexOutOfRangeException("Index must be less than Count.");
                return _Items[Index];
            }
            set
            {
                if (Index < 0) throw new IndexOutOfRangeException("Index must be greater than or equal to 0.");
                if (Index >= Count) throw new IndexOutOfRangeException("Index must be less than Count.");
                _Items[Index] = value;
                _ItemsLocation[value.GetType()] = Index;
            }
        }

        public bool TryAdd(T item)
        {
            if (_ItemsLocation.ContainsKey(item.GetType())) return false;
            _ItemsLocation.Add(item.GetType(), Count);
            if (Count + 1 >= _Items.Length)
            {
                Array.Resize(ref _Items, (Count + 1) * 2);
            }
            _Items[Count] = item;
            Count++;
            return true;
        }

        public void SetCurrentToRandom()
        {
            Current = Random.Shared.Next(0, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in _Items) yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class LinkedList<T> : IEnumerable<ListNode<T>>
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

        public ListNode()
        {
            Value = default;
        }

        public ListNode(T inital)
        {
            Value = inital;
        }

        public ListNode(ListNode<T> node)
        {
            Value = default;
            Next = node;
        }

        public ListNode(T inital, ListNode<T> node)
        {
            Value = inital;
            Next = node;
        }
    }
}
