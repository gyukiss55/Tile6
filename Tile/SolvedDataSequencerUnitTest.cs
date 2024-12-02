using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tile;

namespace UnitTest
{
    internal class SolvedDataSequencerUnitTest
    {
        static public void Execute()
        {
            bool failed = false;
            Tile.SolvedDataSequencer sequencer = new Tile.SolvedDataSequencer();

            int counter = 0;
            int cycleNr = 0;

            do
            {
                cycleNr++;
                JsonStringTokenizer.SolvedHeader header = sequencer.GetNextHeaders();
                int key = Tile.SolvedDataSequencer.HashKey(header);
                if (!sequencer.ContainsKey(key))
                {
                    sequencer.Add(key, header);
                    if (!sequencer.ContainsKey(key))
                    {
                        Console.WriteLine("ASSERT 1");
                        failed = true;
                    }
                    counter = sequencer.Count();
                }
                if (cycleNr > 366 * 7 * 4)
                {
                    Console.WriteLine("ASSERT 2");
                    failed = true;
                }

            } while (counter < 366 * 7);

            for (int i = 0; i < 366 * 7; i++)
            {
                JsonStringTokenizer.SolvedHeader header = sequencer.GetNextHeaders();
                int key = Tile.SolvedDataSequencer.HashKey(header);
                if (!sequencer.ContainsKey(key))
                {
                    Console.WriteLine("ASSERT 3");
                    failed = true;
                    break;
                }

            }
            if (!failed)
            {
                Console.WriteLine($"SolvedDataSequencerUnitTest Done {sequencer.Count()} - {366 * 7}");
            }
        }
    }
}
