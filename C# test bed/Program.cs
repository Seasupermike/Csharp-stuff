using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Catalog.TestClass.Test();
        }
    }
}

namespace Catalog
{
    public class TestClass
    {
        public static void Test()
        {
            bool[] bools = [false, true, false];
            Catalog<bool> test = new(bools);
            try
            {
                Console.WriteLine(test[0, 4]);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            
        }
    }
    
    public interface ICatalog<T> : IEnumerable<T>, ICollection<T>
    {
        int Capacity { get; }
        void AddRange(IEnumerable<T> Values);
        void RemoveAt(int Index);
        T RemoveAndGet(int Index);
        int IndexOf(T Taarget);
        T[] ToArray();
        List<T> ToList();
        T GetRandom();
        public void EnsureMinimumCapacity(int NeededCapacity);
    }

    public class Catalog<T> : ICatalog<T>, IList<T>
    {
        private T[] Items;
        public int Capacity => Items.Length;
        public int Count
        {
            get;
            private set
            {
                field = value;
            }
        }
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
            } else
            {
                Items = new T[Collection.Count()];
            }
            int i = 0;
            foreach (T Item in Collection)
            {
                Items[i] = Item;
                i++;
            }
            Count = i;
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

        public void AddRange(IEnumerable<T> Values)
        {
            if (IsReadOnly) throw new ReadOnlyException($"CustomList<{typeof(T)}> is read only.");
            EnsureMinimumCapacity(Count + Values.Count());

            foreach (T Item in Values)
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
            return new List<T>(this);
        }

        // Makes a copy of the list. If you have objects in the list the copy will have the same refrences.
        public Catalog<T> Clone(bool isReadOnly = false) => new(this, isReadOnly);

        public void CopyTo(T[] DestinationArray, int StartingIndex)
        {
            Items.CopyTo(DestinationArray, StartingIndex);
        }

        public T GetRandom()
        {
            if (Count == 0) throw new InvalidOperationException($"Catalog<{typeof(T)}> contains no elements.");
            return Items[Random.Shared.Next(Count)];
        }

        public override string ToString()
        {
            if (Count == 0) return $"Catalog<{typeof(T)}>(0)";

            StringBuilder sb = new StringBuilder($"Catalog<{typeof(T)}>({Count}) {"{"} .");
            switch (typeof(T).ToString())
            {
                case "System.String":
                    for (int i = 0; i < Count; i++)
                    {
                        sb.Append($"\"{Items[i]}\".");
                        if (i < Count - 1)
                        {
                            sb.Append(", .");
                        }
                    }
                    break;
                case "System.Char":
                    for (int i = 0; i < Count; i++)
                    {
                        sb.Append($"'{Items[i]}'.");
                        if (i < Count - 1)
                        {
                            sb.Append(", .");
                        }
                    }
                    break;
                default:
                    for (int i = 0; i < Count; i++)
                    {
                        sb.Append(Items[i]);
                        if (i < Count - 1)
                        {
                            sb.Append(", .");
                        }
                    }
                    break;
            }

            sb.Append(" }.");
            return sb.ToString();
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
            for (int i = 0; i < Count; i++)
            {
                yield return Items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Catalog<T> Combine(Catalog<T> CatalogA, Catalog<T> CatalogB)
        {
            Catalog<T> Result = CatalogA.Clone();
            Result.AddRange(CatalogB);
            return Result;
        }

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
}