using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tile.JsonStringTokenizer;

namespace Tile
{
    internal class Tile6 : Tile6Base
    {

        List<Pair<int>> pairsMonthList = new List<Pair<int>>();
        List<Pair<int>> pairsWeekDayList = new List<Pair<int>>();
        List<Pair<int>> pairsDayList = new List<Pair<int>>();
        ShapePossiblePosition shapePossiblePosition = new ShapePossiblePosition (0);
        List<ShapePossiblePosition> shapePossiblePositionList = new List<ShapePossiblePosition>();
        public ConcurrentStack<JsonStringTokenizer.SolvedItem> solvedItemStack = new ConcurrentStack<JsonStringTokenizer.SolvedItem>();

        const int ThreadNumber = 11;
        public int month { get; set; }
        public int mday { get; set; }
        public int wday { get; set; }

        void InitTable(int month, int day, int weekDay)
        {
            InitTableBase();

            pairsMonthList.Add(new Pair<int>(0, 4)); // 1
            pairsMonthList.Add(new Pair<int>(0, 3)); // 2
            pairsMonthList.Add(new Pair<int>(1, 2)); // 3
            pairsMonthList.Add(new Pair<int>(1, 1)); // 4
            pairsMonthList.Add(new Pair<int>(2, 0)); // 5
            pairsMonthList.Add(new Pair<int>(3, 0)); // 6
            
            pairsMonthList.Add(new Pair<int>(5, 0)); // 7
            pairsMonthList.Add(new Pair<int>(6, 0)); // 8
            pairsMonthList.Add(new Pair<int>(6, 1)); // 9
            pairsMonthList.Add(new Pair<int>(7, 2)); // 10
            pairsMonthList.Add(new Pair<int>(7, 3)); // 11
            pairsMonthList.Add(new Pair<int>(8, 4)); // 12

            pairsWeekDayList.Add(new Pair<int>(2, 1));
            pairsWeekDayList.Add(new Pair<int>(3, 1));
            pairsWeekDayList.Add(new Pair<int>(4, 1));
            pairsWeekDayList.Add(new Pair<int>(5, 1));
            pairsWeekDayList.Add(new Pair<int>(3, 2));
            pairsWeekDayList.Add(new Pair<int>(4, 2));
            pairsWeekDayList.Add(new Pair<int>(5, 2));

            pairsDayList.Add(new Pair<int>(1, 3));
            pairsDayList.Add(new Pair<int>(2, 3));
            pairsDayList.Add(new Pair<int>(3, 3));
            pairsDayList.Add(new Pair<int>(4, 3));
            pairsDayList.Add(new Pair<int>(5, 3));
            pairsDayList.Add(new Pair<int>(6, 3));

            pairsDayList.Add(new Pair<int>(1, 4));
            pairsDayList.Add(new Pair<int>(2, 4));
            pairsDayList.Add(new Pair<int>(3, 4));
            pairsDayList.Add(new Pair<int>(4, 4));
            pairsDayList.Add(new Pair<int>(5, 4));
            pairsDayList.Add(new Pair<int>(6, 4));
            pairsDayList.Add(new Pair<int>(7, 4));

            pairsDayList.Add(new Pair<int>(1, 5));
            pairsDayList.Add(new Pair<int>(2, 5));
            pairsDayList.Add(new Pair<int>(3, 5));
            pairsDayList.Add(new Pair<int>(4, 5));
            pairsDayList.Add(new Pair<int>(5, 5));
            pairsDayList.Add(new Pair<int>(6, 5));

            pairsDayList.Add(new Pair<int>(2, 6));
            pairsDayList.Add(new Pair<int>(3, 6));
            pairsDayList.Add(new Pair<int>(4, 6));
            pairsDayList.Add(new Pair<int>(5, 6));
            pairsDayList.Add(new Pair<int>(6, 6));

            pairsDayList.Add(new Pair<int>(2, 7));
            pairsDayList.Add(new Pair<int>(3, 7));
            pairsDayList.Add(new Pair<int>(4, 7));
            pairsDayList.Add(new Pair<int>(5, 7));

            pairsDayList.Add(new Pair<int>(3, 8));
            pairsDayList.Add(new Pair<int>(4, 8));
            pairsDayList.Add(new Pair<int>(5, 8));

            var pairMonth = pairsMonthList[month - 1];
            var pairDay = pairsDayList[day - 1];
            var pairWeekDay = pairsWeekDayList[weekDay - 1];

            SetTable(0xe, pairMonth);
            SetTable(0xe, pairDay);
            SetTable(0xe, pairWeekDay);

        }

        public void ExecuteAllShape(int monthIn, int mdayIn, int wdayIn)
        {
            InitTable(monthIn, mdayIn, wdayIn);
            //DumpTable();
            bool result = PlaceAllShapes();
            //Console.WriteLine($"Execute:{result}");
            long elapsedTotalLocal = elapsedTotal + timerTest.Check();
            Console.WriteLine($"Timer total: {elapsedTotalLocal}");

            DumpTable();
        }

        public bool CalculateOneShape()
        {
            InitTableBase();
            return true;
        }

        public void ExecuteOneShape(int shapeIndex)
        {
            //DumpTable();

            //List<ShapePosition> spList = new List<ShapePosition>();
            //List<List<Pair<int>>> tList = new List<List<Pair<int>>>();
            List<ShapePosition> spList = shapePossiblePosition.shapePositionList;
            List<List<Pair<int>>> tList = shapePossiblePosition.territoryList;

            bool result = PlaceOneShape(shapeIndex, ref spList, ref tList);
            //Console.WriteLine($"Execute:{result}");

            //long elapsedTotalLocal = elapsedTotal + timerTest.Check();
            //Console.WriteLine($"Timer total: {elapsedTotalLocal}");

            //DumpTable();
            
        }


        static async Task PossiblePositionCall(Tile6 tile)
        {
            //Console.WriteLine($"MultiTaskCall is:{shapePossiblePosition.shapeIndex}");
            tile.ExecuteOneShape(tile.shapePossiblePosition.shapeIndex);
        }

        async Task CalculatePossiblePosition()
        {
            TimerTest timerTest = new TimerTest();

            Tile6[] tile6Array = new Tile6[11];
            List<Task> taskList = new List<Task>();
            var shapeIndeces = Enumerable.Range(1, 11);

            foreach (var shapeIndex in shapeIndeces)
            {
                int currentSi = shapeIndex;
                tile6Array[shapeIndex - 1] = new Tile6();
                tile6Array[shapeIndex - 1].InitTableBase();
                tile6Array[shapeIndex - 1].tile6ID = shapeIndex;
                tile6Array[shapeIndex - 1].shapePossiblePosition = new ShapePossiblePosition(shapeIndex);
            }

            Parallel.ForEach(shapeIndeces, shapeIndex =>
            {
                taskList.Add(PossiblePositionCall(tile6Array[shapeIndex - 1]));
            });

            foreach (Tile6 tile in tile6Array)
            {
                shapePossiblePositionList.Add(tile.shapePossiblePosition);
            }
            foreach (Tile6 tile in tile6Array)  {
                 shapePossiblePositionList[tile.shapePossiblePosition.shapeIndex - 1] = tile.shapePossiblePosition;
            }

            Console.WriteLine($"ShapePossiblePosition time:{timerTest.Check()}");
        }
 
        public void PlaceAllShapesCall(List<ShapePossiblePosition> sppList, ConcurrentStack<JsonStringTokenizer.SolvedItem> solvedItemInStack)
        {
            Console.WriteLine($"Start:{stepIndex}");
            TimerTest timerTest = new TimerTest();
            //Tile6 tileThread = new Tile6();
            InitTable(month, mday, wday);
            //tileThread.DumpTable();
            bool result = PlaceAllShapes(stepIndex, ref sppList, solvedItemInStack);
            //Console.WriteLine($"Execute:{result}");
            if (result)
            {
                Console.WriteLine($"Done step index: {stepIndex}");
                long elapsedTotalLocal = timerTest.Check();
                Console.WriteLine($"Timer total: {elapsedTotalLocal}");
                //DumpTable();
            }
            Console.WriteLine($"Finished:{stepIndex}");

        }
        public void PlaceAllShapesCall(Tile6 subTile, ref List<ShapePossiblePosition> sppList, ConcurrentStack<JsonStringTokenizer.SolvedItem> solvedItemInStack)
        {
            subTile.PlaceAllShapesCall(sppList, solvedItemInStack);
        }

        public void Execute(int monthIn, int mdayIn, int wdayIn)
        {
            month = monthIn;
            mday = mdayIn;
            wday = wdayIn;

            Console.WriteLine($"Solve: {month},{mday},{wday}");
            //HashUtility.TestGenerateHashListPair();

            //const int threadNums = ShapeNumber;
            TimerTest timerTest = new TimerTest();
            CalculatePossiblePosition ();

            //Thread[] threads = new Thread[ThreadNumber];

            List<ShapePossiblePosition> sppList = new List<ShapePossiblePosition>();
            sppList = shapePossiblePositionList;

            List<Tile6> tileList = new List<Tile6>(); 

            int stepIndex = 0;
            foreach (var shapePosition in shapePossiblePositionList[0].shapePositionList)
            {
                Tile6 tile6Task = new Tile6();
                tile6Task.stepIndex = stepIndex;
                tile6Task.tile6ID = stepIndex;
                tile6Task.month = month;
                tile6Task.mday = mday;
                tile6Task.wday = wday;

                tileList.Add(tile6Task);

                stepIndex++;
            }

            List<Task> taskList = new List<Task>();
            var rangeTile = Enumerable.Range(0, shapePossiblePositionList[0].shapePositionList.Count - 1);
            Parallel.ForEach(rangeTile, indexTile => {
                PlaceAllShapesCall(tileList[indexTile], ref sppList, solvedItemStack);
            });

            long ms = timerTest.Check();
            elapsedTotal += ms;

            if (elapsedTotal % 1000 == 0)
            {
                Console.WriteLine($"stepIndex: {stepIndex}");
                Console.WriteLine($"Timer: {elapsedTotal}");
            }
        }
    }
}
