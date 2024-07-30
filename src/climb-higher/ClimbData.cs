using SQLite;
using System.ComponentModel;

namespace climb_higher;
[Table("climbData")]
/*
 * Definition of variables in the database.
 */
public class ClimbData 
{
    [PrimaryKey, AutoIncrement, Column("_id")]
    public int Id { get; set; }
    public string routeType { get; set; }
    public string grade { get; set; }
    public int tries { get; set; }
    public string color { get; set; }
    public string walltype { get; set; }
    public bool isIndoor { get; set; }
    public string notes { get; set; }
    public string title { get; set; }
    public TimeSpan timeLength { get; set; }
    
    public string stringOfTimes {  get; set; }
    /*
     * This way we can print the records in a resonable way.
     */

    public override string ToString()
    {
        return string.Format("Climb {0}: {1} {2} {3}", Id, title, grade, color);
    }

    // Properties to hold the best, worst, and avg times of an individual climb
    public TimeSpan BestTime { get; set; }
    public TimeSpan AvgTime { get; set; }
    public TimeSpan WorstTime { get; set; }

    /// <summary>
    /// findBestTime() finds the best time of a climb
    /// </summary>
    /// <returns> The best time of a climb </returns>
    public TimeSpan findBestTime()
    {
        if (String.IsNullOrEmpty(stringOfTimes))
        {
            return TimeSpan.Zero;
        }
        String[] arrOfTimes = stringOfTimes.Split(',');
        if (arrOfTimes.Length < 1)
        {
            return TimeSpan.Zero;
        }
        if (arrOfTimes.Length == 1)
        {
            // s format example: 02:14.3450000
            String str = arrOfTimes[0];
            String[] listTimeSplit = (str.Split(':')); // {02, 14.3450000}
            String[] splitSecsMillisecs = (listTimeSplit[1].Split(".")); // {14, 3450000}

            int mins = int.Parse(listTimeSplit[0]);
            int secs = int.Parse(splitSecsMillisecs[0]);
            int millisecs = int.Parse(splitSecsMillisecs[1]);

            return new TimeSpan(0, 0, mins, secs, millisecs);
        }
        else
        {

            TimeSpan bestTime = TimeSpan.MaxValue;
            for (int i = 0; i < arrOfTimes.Length; i++)
            {
                String str = arrOfTimes[i];
                // s format example: 02:14.3450000
                String[] listTimeSplit = (str.Split(':')); // {02, 14.3450000}
                String[] splitSecsMillisecs = (listTimeSplit[1].Split(".")); // {14, 3450000}

                int mins = int.Parse(listTimeSplit[0]);
                int secs = int.Parse(splitSecsMillisecs[0]);
                int millisecs = int.Parse(splitSecsMillisecs[1]);

                TimeSpan currTime = new TimeSpan(0, 0, mins, secs, millisecs);

                int timeComp = TimeSpan.Compare(currTime, bestTime);

                if (timeComp == -1)
                {
                    bestTime = currTime;
                }

            }
            return bestTime;
        }
    }

    /// <summary>
    /// findAvgTime() finds the avg time of a climb
    /// </summary>
    /// <returns>The average time of a climb </returns>
    public TimeSpan findAvgTime()
    {
        if (String.IsNullOrEmpty(stringOfTimes))
        {
            return TimeSpan.Zero;
        }
        String[] arrOfTimes = stringOfTimes.Split(',');
        if (arrOfTimes.Length < 1)
        {
            return TimeSpan.Zero;
        }
        if (arrOfTimes.Length == 1)
        {
            // s format example: 02:14.3450000
            String str = arrOfTimes[0];
            String[] listTimeSplit = (str.Split(':')); // {02, 14.3450000}
            String[] splitSecsMillisecs = (listTimeSplit[1].Split(".")); // {14, 3450000}

            int mins = int.Parse(listTimeSplit[0]);
            int secs = int.Parse(splitSecsMillisecs[0]);
            int millisecs = int.Parse(splitSecsMillisecs[1]);

            return new TimeSpan(0, 0, mins, secs, millisecs);
        }
        else
        {
            List<TimeSpan> listTimes = new List<TimeSpan>();
            for (int i = 0; i < arrOfTimes.Length; i++)
            {
                String str = arrOfTimes[i];
                // s format example: 02:14.3450000
                String[] listTimeSplit = (str.Split(':')); // {02, 14.3450000}
                String[] splitSecsMillisecs = (listTimeSplit[1].Split(".")); // {14, 3450000}

                int mins = int.Parse(listTimeSplit[0]);
                int secs = int.Parse(splitSecsMillisecs[0]);
                int millisecs = int.Parse(splitSecsMillisecs[1]);

                TimeSpan currTime = new TimeSpan(0, 0, mins, secs, millisecs);
                listTimes.Add(currTime);

            }
            double doubleAverageTicks = listTimes.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
            return TimeSpan.FromTicks(longAverageTicks);
        }
    }

    /// <summary>
    /// findWorstTime() finds the worst time of a climb
    /// </summary>
    /// <returns> The worst time of a climb </returns>
    public TimeSpan findWorstTime()
    {
        if (String.IsNullOrEmpty(stringOfTimes))
        {
            return TimeSpan.Zero;
        }
        String[] arrOfTimes = stringOfTimes.Split(',');
        if (arrOfTimes.Length < 1)
        {
            return TimeSpan.Zero;
        }
        if (arrOfTimes.Length == 1)
        {
            // s format example: 02:14.3450000
            String str = arrOfTimes[0];
            String[] listTimeSplit = (str.Split(':')); // {02, 14.3450000}
            String[] splitSecsMillisecs = (listTimeSplit[1].Split(".")); // {14, 3450000}

            int mins = int.Parse(listTimeSplit[0]);
            int secs = int.Parse(splitSecsMillisecs[0]);
            int millisecs = int.Parse(splitSecsMillisecs[1]);

            return new TimeSpan(0, 0, mins, secs, millisecs);
        }
        else
        {

            TimeSpan worstTime = TimeSpan.MinValue;
            for (int i = 0; i < arrOfTimes.Length; i++)
            {
                String str = arrOfTimes[i];
                // s format example: 02:14.3450000
                String[] listTimeSplit = (str.Split(':')); // {02, 14.3450000}
                String[] splitSecsMillisecs = (listTimeSplit[1].Split(".")); // {14, 3450000}

                int mins = int.Parse(listTimeSplit[0]);
                int secs = int.Parse(splitSecsMillisecs[0]);
                int millisecs = int.Parse(splitSecsMillisecs[1]);

                TimeSpan currTime = new TimeSpan(0, 0, mins, secs, millisecs);

                int timeComp = TimeSpan.Compare(currTime, worstTime);

                if (timeComp == 1)
                {
                    worstTime = currTime;
                }

            }
            return worstTime;
        }
    }
}