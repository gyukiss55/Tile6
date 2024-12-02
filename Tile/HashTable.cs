using System;
using System.Collections.Generic;

namespace Utility
{
    public class HashTable<T>
    {
        // Internal dictionary to store the key-value pairs
        private readonly Dictionary<int, T> _data;

        // Constructor
        public HashTable()
        {
            _data = new Dictionary<int, T>();
        }

        // Method to add a value with a unique hash key
        public void Add(int hashKey, T value)
        {
            if (_data.ContainsKey(hashKey))
            {
                throw new ArgumentException($"An item with the key {hashKey} already exists.");
            }

            _data[hashKey] = value;
        }

        // Method to find a value by its hash key
        public T Find(int hashKey)
        {
            if (_data.TryGetValue(hashKey, out T value))
            {
                return value;
            }

            throw new KeyNotFoundException($"No item found with the key {hashKey}.");
        }

        // Method to remove a value by its hash key
        public bool Remove(int hashKey)
        {
            return _data.Remove(hashKey);
        }

        // Method to check if the hash key exists
        public bool ContainsKey(int hashKey)
        {
            return _data.ContainsKey(hashKey);
        }

        // Method to retrieve all items
        public IEnumerable<T> GetAllItems()
        {
            return _data.Values;
        }

        // Method to get the total count of items
        public int Count => _data.Count;
    }

 }
