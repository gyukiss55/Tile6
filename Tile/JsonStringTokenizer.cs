using System;
using System.Collections.Generic;
using System.Text.Json;


namespace Tile
{
    public class JsonStringTokenizer
    {
        // Define the structure of the JSON string
        public class Position
        {
            public int first { get; set; }
            public int second { get; set; }
     
        }

        public class ShapePosition
        {
            public int index { get; set; }
            public Position position { get; set; }
            public int orientation { get; set; }
        }

        public class SolvedItem
        {
            public int solveIndex { get; set; }
            public List<ShapePosition> shapePositionList { get; set; } = new List<ShapePosition>();
        }

        public class SolvedHeader
        {
            public int month { get; set; }
            public int dayMonth { get; set; }
            public int dayWeek { get; set; }
        }

        public class SolvedObject
        {
            public SolvedHeader solvedHeader { get; set; }
            public List<SolvedItem> solvedItemList { get; set; } = new List<SolvedItem>();
        }

        public class SolvedUnion
        {
            public string jsonFileID { get; set; } = "SolvedUnion V.1.0.0";
            public List<SolvedObject> solvedObjectList { get; set; } = new List<SolvedObject>();
        }

        // Method to parse and tokenize the JSON string
        public SolvedUnion TokenizeSolvedUnion(string jsonString)
        {
            try
            {
                // Deserialize the JSON string into the RootObject structure
                SolvedUnion parsedObject = JsonSerializer.Deserialize<SolvedUnion>(jsonString);

                if (parsedObject == null)
                {
                    throw new Exception("Failed to parse JSON string.");
                }

                return parsedObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while tokenizing JSON: {ex.Message}");
                throw;
            }
        }
        public SolvedObject TokenizeSolvedObject(string jsonString)
        {
            try
            {
                // Deserialize the JSON string into the RootObject structure
                SolvedObject parsedObject = JsonSerializer.Deserialize<SolvedObject>(jsonString);

                if (parsedObject == null)
                {
                    throw new Exception("Failed to parse JSON string.");
                }

                return parsedObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while tokenizing JSON: {ex.Message}");
                throw;
            }
        }   
        public SolvedItem TokenizeSolvedItem(string jsonString)
        {
            try
            {
                // Deserialize the JSON string into the RootObject structure
                SolvedItem parsedObject = JsonSerializer.Deserialize<SolvedItem>(jsonString);

                if (parsedObject == null)
                {
                    throw new Exception("Failed to parse JSON string.");
                }

                return parsedObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while tokenizing JSON: {ex.Message}");
                throw;
            }
        }
    }
}
