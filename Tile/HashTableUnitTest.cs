using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace UnitTest
{
    public class HashTableTestData
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }

    internal class HashTableUnitTest
    {
        static public void Execute()
        {
            // Create a new hash table
            HashTable<HashTableTestData> hashTable = new HashTable<HashTableTestData>();

            // Add items
            hashTable.Add(1, new HashTableTestData { Name = "First" });
            hashTable.Add(2, new HashTableTestData { Name = "Second" });
            hashTable.Add(3, new HashTableTestData { Name = "Third" });

            // Find an item
            Console.WriteLine("Find Key 2: " + hashTable.Find(2));

            // Check existence of a key
            Console.WriteLine("Contains Key 3: " + hashTable.ContainsKey(3));

            // Remove an item
            hashTable.Remove(2);
            Console.WriteLine("Removed Key 2");

            // Attempt to find a removed item
            try
            {
                Console.WriteLine(hashTable.Find(2));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            // Get all items
            Console.WriteLine("All Items:");
            foreach (var item in hashTable.GetAllItems())
            {
                Console.WriteLine(item);
            }

            // Display count
            Console.WriteLine("Total Count: " + hashTable.Count);
        }

    }

}
