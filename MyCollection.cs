using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsLibrary;

namespace MyCollectionLibrary
{
    public class MyCollection<T> : MyHashTable<T>, ICollection<T>, IEnumerable<T> where T : IInit, ICloneable, new()
    {
        // Конструкторы
        public MyCollection() : base() { }

        public MyCollection(int length) : base(length)
        {
            for (int i = 0; i < length; i++)
            {
                T item = new T();
                item.RandomInit();
                AddItem(item);
            }
        }

        public MyCollection(MyCollection<T> c) : base(c.Capacity)
        {
            foreach (T item in c)
            {
                AddItem((T)item.Clone());
            }
        }

        // Индексатор
        public T this[T item]
        {
            get
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));
                int index = FindItem(item);
                if (index == -1)
                    throw new InvalidOperationException("Элемент не найден в коллекции!");
                return table[index];
            }
            set
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));
                int index = FindItem(item);
                if (index != -1)
                {
                    Remove(item);
                    Add(value);
                }
                else
                    throw new InvalidOperationException("Элемент не найден в коллекции!");
            }
        }

        // Свойства
        public bool IsReadOnly => false;

        // Методы
        public void Add(T item)
        {
            AddItem(item);
        }
        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
            {
                table[i] = default;
                wasDeleted[i] = false;
            }
            count = 0;
        }
        public bool Contains(T item)
        {
            return base.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < count)
                throw new ArgumentException("В конечном массиве меньше элементов чем в исходной коллекции!");

            int j = arrayIndex;
            for (int i = 0; i < Capacity; i++)
            {
                if (table[i] != null)
                {
                    array[j++] = table[i];
                }
            }
        }
        public bool Remove(T item)
        {
            return RemoveData(item);
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Capacity; i++)
            {
                if (table[i] != null)
                {
                    yield return table[i];
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
