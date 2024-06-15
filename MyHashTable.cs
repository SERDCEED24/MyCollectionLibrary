using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsLibrary;

namespace MyCollectionLibrary
{
    public class MyHashTable<T> where T : IInit, ICloneable, new()
    {
        // Поля
        protected T[] table;
        protected bool[] wasDeleted;
        protected int count = 0;
        protected double fillRatio;

        // Свойства
        public int Capacity => table.Length;
        public int Count => count;
        public T[] Table => table;

        // Конструктор
        public MyHashTable(int size = 10, double fillRatio = 0.72)
        {
            table = new T[size];
            wasDeleted = new bool[size];
            this.fillRatio = fillRatio;
        }

        //Методы
        public bool Contains(T data)
        {
            return !(FindItem(data) < 0);
        }
        public bool RemoveData(T data)
        {
            int index = FindItem(data);
            if (index < 0)
                return false;
            count--;
            table[index] = default;
            wasDeleted[index] = true;
            return true;
        }
        public void Print()
        {
            int i = 0;
            foreach (T item in table)
            {
                Console.WriteLine($"{i}: {item}");
                i++;
            }
        }
        public void AddItem(T item)
        {
            if ((double)Count / Capacity > fillRatio)
            {
                T[] tempTable = (T[])table.Clone();
                table = new T[tempTable.Length * 2];
                bool[] tempWD = (bool[])wasDeleted.Clone();
                wasDeleted = new bool[tempWD.Length * 2];
                count = 0;
                for (int i = 0; i < tempTable.Length; i++)
                {
                    AddData(tempTable[i]);
                    wasDeleted[i] = tempWD[i];
                }
            }
            AddData(item);
        }
        private int GetIndex(T data)
        {
            return Math.Abs(data.GetHashCode()) % Capacity;
        }
        public void AddData(T data)
        {
            if (data == null)
                return;
            int index = GetIndex(data);
            int current = index;
            if (table[index] != null)
            {
                while (current < table.Length && table[current] != null)
                    current++;
                if (current == table.Length)
                {
                    current = 0;
                    while (current < index && table[current] != null)
                        current++;
                    if (current == index)
                    {
                        throw new Exception("Нет места в таблице!");
                    }
                }
            }
            table[current] = data;
            count++;
        }
        public int FindItem(T data)
        {
            int index = GetIndex(data);
            if (table[index] == null && !wasDeleted[index])
                return -1;
            if (table[index] != null && table[index].Equals(data))
                return index;
            else
            {
                int current = index;
                while (current < table.Length)
                {
                    if (table[current] != null && table[current].Equals(data))
                        return current;
                    current++;
                }
                current = 0;
                while (current < index)
                {
                    if (table[current] != null && table[current].Equals(data))
                        return current;
                    current++;
                }
            }
            return -1;
        }
    }

}
