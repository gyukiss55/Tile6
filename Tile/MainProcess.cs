
using System.Text.Json;
using static Tile.JsonStringTokenizer;

namespace Tile
{
    internal class MainProcess
    {
        static public void ExecuteSolver(bool multiThread)
        {
            string jsonFilePath = "SolvedUnion.json";
            string jsonFilePathTmp = "SolvedUnion.json.tmp";
            string fileContent = "";
            if (File.Exists(jsonFilePath))
            {
                fileContent = File.ReadAllText(jsonFilePath);
            }
            JsonStringTokenizer.SolvedUnion solvedUnion = new JsonStringTokenizer.SolvedUnion();
            SolvedDataSequencer sequencer = new SolvedDataSequencer();

            if (fileContent.Length > 0)
            {
                Console.WriteLine("File Content:");
                Console.WriteLine(fileContent);

                JsonStringTokenizer tokenizer = new JsonStringTokenizer();

                // Tokenize the JSON string
                solvedUnion = tokenizer.TokenizeSolvedUnion(fileContent);

                foreach (SolvedObject d in solvedUnion.solvedObjectList)
                {
                    int key = SolvedDataSequencer.HashKey(d.solvedHeader);
                    if (!sequencer.ContainsKey(key))
                    {
                        sequencer.Add(key, d.solvedHeader);
                    }
                }
             }

            do {
                SolvedHeader header = sequencer.GetNextHeaders();
                int key = Tile.SolvedDataSequencer.HashKey(header);
                if (!sequencer.ContainsKey(key))
                {
                    Tile6 tile6 = new Tile6();
                    if (multiThread)
                        tile6.Execute(header.month, header.dayMonth, header.dayWeek); // int month, int mday, int wday (1,1,1) => jan, 1, Sunday - (12,31,7) => dec, 31, Saturday
                    else 
                        tile6.ExecuteAllShape(header.month, header.month, header.dayWeek);
                    Console.WriteLine("Done");
                    sequencer.Add(key, header);
                    JsonStringTokenizer.SolvedObject solvedObject = new JsonStringTokenizer.SolvedObject();
                    solvedObject.solvedHeader = header;
                    int ix = 0;
                    foreach (var solvedItem in tile6.solvedItemStack)
                    {
                        solvedItem.solveIndex = ++ix;
                        solvedObject.solvedItemList.Add(solvedItem);
                    }
                    solvedUnion.solvedObjectList.Add(solvedObject);

                    if (File.Exists(jsonFilePathTmp))
                    {
                        File.Delete(jsonFilePathTmp);
                    }
                    if (File.Exists(jsonFilePath))
                    {
                        File.Move(jsonFilePath, jsonFilePathTmp);
                    }

                    string jsonString = JsonSerializer.Serialize(solvedUnion);
                    Console.WriteLine(jsonString);

                    File.WriteAllText(jsonFilePath, jsonString, System.Text.Encoding.UTF8);
                }

                Console.WriteLine($"sequencer.Count() {sequencer.Count()} / {366 * 7}");
            } while (sequencer.Count() < 366 * 7);
        }

        static public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Options: single thread");
                Console.WriteLine("  -s <month> <monthDay> <weekDay>");
                Console.WriteLine("Options: multi thread");
                Console.WriteLine("  -m <month> <monthDay> <weekDay>");

                ExecuteSolver(true);

            }
            else if (args.Length == 1) {
                if (args[0] == "-jsonTest")
                {
                    Console.WriteLine("jsonTest begin");
                    UnitTest.JsonUnitTest.Execute();
                    Console.WriteLine("jsonTest done");
                }
                if (args[0] == "-HashTableTest")
                {
                    Console.WriteLine("HashTableTest begin");
                    UnitTest.HashTableUnitTest.Execute();
                    Console.WriteLine("HashTableTest done");
                }
                if (args[0] == "-SequencerTest")
                {
                    Console.WriteLine("SolvedDataSequencerUnitTest begin");
                    UnitTest.SolvedDataSequencerUnitTest.Execute();
                    Console.WriteLine("SolvedDataSequencerUnitTest done");
                }
            } else if (args.Length == 4) {
                if (args[0].ToLower() == "-m") // multi thread:
                {
                    ExecuteSolver(true);

                }
                if (args[0].ToLower() == "-s") // single thread:
                {
                    ExecuteSolver(false);
                }
            }
        }
    }
}
