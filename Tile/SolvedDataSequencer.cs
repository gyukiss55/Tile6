using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tile
{
    internal class SolvedDataSequencer
    {

        const int MaxDateNumber = 366 * 7;
        Utility.HashTable<JsonStringTokenizer.SolvedHeader> hashTableHeaders = new Utility.HashTable<JsonStringTokenizer.SolvedHeader>();

        DateTime lastDateTime = DateTime.Today;

        static public int HashKey(JsonStringTokenizer.SolvedHeader header)
        {
            if (header == null)
                return 0;
            else
                return header.month * 1000 + header.dayMonth * 10 + header.dayWeek;
        }

        static public JsonStringTokenizer.SolvedHeader GetHeaderTodayDate()
        {
            JsonStringTokenizer.SolvedHeader ret = new JsonStringTokenizer.SolvedHeader();
            DateTime today = DateTime.Today;
            ret.month = today.Month;
            ret.dayMonth = today.Day;
            ret.dayWeek = (int)today.DayOfWeek;
            return ret;
        }

        static public JsonStringTokenizer.SolvedHeader GetHeaderDate(int year, int month, int day)
        {
            JsonStringTokenizer.SolvedHeader ret = new JsonStringTokenizer.SolvedHeader();
            DateTime today = new DateTime (year, month, day);
            ret.month = today.Month;
            ret.dayMonth = today.Day;
            ret.dayWeek = (int)today.DayOfWeek;
            return ret;
        }
        static bool IsLeapYear(int year)
        {
            // Custom leap year logic
            if (year % 4 == 0)
            {
                if (year % 100 == 0)
                {
                    return year % 400 == 0;
                }
                return true;
            }
            return false;
        }

        static DateTime NextDay (DateTime d)
        {
            DateTime ret = d;
            if (d.Month == 12 && d.Day == 31)
                ret = new DateTime (d.Year + 1, 1, 1);
            else
                ret = d.AddDays(1);
            return ret;
        }

        public JsonStringTokenizer.SolvedHeader GetNextHeaders ()
        {
            DateTime retDateTime = lastDateTime;
            lastDateTime = NextDay(lastDateTime);
            JsonStringTokenizer.SolvedHeader ret = new JsonStringTokenizer.SolvedHeader { month = retDateTime.Month, dayMonth = retDateTime.Day, dayWeek = (int)retDateTime.DayOfWeek + 1 };
            return ret;
        }

        public bool ContainsKey (int key)
        {
            if (hashTableHeaders.ContainsKey(key))
                return true;
            return false;
        }
        public void Add (int key, JsonStringTokenizer.SolvedHeader header)
        {
            hashTableHeaders.Add(key, header);
        }
        public JsonStringTokenizer.SolvedHeader Find (int key)
        {
            return hashTableHeaders.Find(key);
        }

        public int Count ()
        {
            return hashTableHeaders.Count;
        }
    }
}
