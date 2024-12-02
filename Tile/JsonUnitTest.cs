
using System.Text.Json;
using Tile;


namespace UnitTest
{
    internal class JsonUnitTest
    {
        static public void Execute()
        {
            Tile.JsonStringTokenizer.ShapePosition sp1 = new Tile.JsonStringTokenizer.ShapePosition
            {
                index = 1,
                position = new Tile.JsonStringTokenizer.Position { first = 2, second = 3 },
                orientation = 0
            };
            Tile.JsonStringTokenizer.ShapePosition sp2 = new Tile.JsonStringTokenizer.ShapePosition
            {
                index = 3,
                position = new Tile.JsonStringTokenizer.Position { first = 4, second = 6 },
                orientation = 5
            };
            Tile.JsonStringTokenizer.ShapePosition sp3 = new Tile.JsonStringTokenizer.ShapePosition
            {
                index = 11,
                position = new Tile.JsonStringTokenizer.Position { first = 7, second = 8 },
                orientation = 2
            };
            Tile.JsonStringTokenizer.SolvedItem solvedItem = new Tile.JsonStringTokenizer.SolvedItem
            {
                solveIndex = 1,
                shapePositionList = new List<Tile.JsonStringTokenizer.ShapePosition> { sp1, sp2, sp3 }
            };
            Tile.JsonStringTokenizer.SolvedObject solvedObject = new Tile.JsonStringTokenizer.SolvedObject {
                solvedHeader = new Tile.JsonStringTokenizer.SolvedHeader {month = 12, dayMonth = 31, dayWeek = 7 },
                solvedItemList = new List<Tile.JsonStringTokenizer.SolvedItem> { solvedItem } };

            Tile.JsonStringTokenizer.SolvedUnion solvedUnion = new Tile.JsonStringTokenizer.SolvedUnion { solvedObjectList = new List<Tile.JsonStringTokenizer.SolvedObject> { solvedObject } };

            // Serialize the object to JSON
            string jsonString = JsonSerializer.Serialize(solvedUnion);
            Console.WriteLine(jsonString);
            string jsonFilePath = "SolvedUnion.json";
            File.WriteAllText(jsonFilePath, jsonString, System.Text.Encoding.UTF8);

            string fileContent = File.ReadAllText(jsonFilePath);
            Console.WriteLine("File Content:");
            Console.WriteLine(fileContent);

            JsonStringTokenizer tokenizer = new JsonStringTokenizer();

            // Tokenize the JSON string
            Tile.JsonStringTokenizer.SolvedUnion parsedData = tokenizer.TokenizeSolvedUnion(fileContent);

            string jsonString2 = JsonSerializer.Serialize(parsedData);
            Console.WriteLine("Parsed Data:");
            Console.WriteLine(jsonString2);

            string jsonFilePath2 = "SolvedUnion2.json";
            File.WriteAllText(jsonFilePath2, jsonString2, System.Text.Encoding.UTF8);

        }
    }
}
