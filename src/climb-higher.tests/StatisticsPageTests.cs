namespace climb_higher.tests;

/// <summary>
/// Class to store climb statistics for easy access for tests.
/// </summary>
public class ClimbDataStats {
    public int totalClimbs {get; set;}
    public TimeSpan bestTime {get; set;}
    public TimeSpan worstTime {get; set;}
    public TimeSpan averageTime {get; set;}
}

/// <summary>
/// Tests the functionality of the climb_higher.StatisticsPage.OnAppearing()
/// method to ensure correct information is returned for different amounts of
/// climbs entered. 
/// 
/// Each tests' potentional errors are marked with which statistics value is 
/// incorrec to ease testing. 
/// </summary>
public class StatisticsPageTests
{  
    private DateTime startTime, endTime;
    public string grade="grade", walltype="walltypeTest", tries="1", color="colorTest", notes="notesTest", title="titleTest";
    public bool outside=true;
    private string routeType="Boulder";

    /// <summary>
    /// Asserts StatisticsPage() will return 3 for total climbs, 3.0.0 for best
    /// climb, 5.0.0 for worst climb, and 4.0.0 for average time when given list 
    /// of 3 climbs with the following times: 3.0.0, 4.0.0, 5.0.0.
    /// </summary>
    [Test]
    public void multipleEntries()
    {
        List<TimeSpan> climbs = new List<TimeSpan>();

        // Best Time: 3.0.0
        startTime = new DateTime(2010, 1, 1, 8, 0, 0);
        endTime = new DateTime(2010, 1, 1, 11, 0, 0);

        ClimbData result = Button_Clicked();
        TimeSpan best = result.timeLength;
        climbs.Add(result.timeLength);

        endTime = new DateTime(2010, 1, 1, 12, 0, 0);
        result = Button_Clicked();
        climbs.Add(result.timeLength);

        // Worst Time: 5.0.0
        endTime = new DateTime(2010, 1, 1, 13, 0, 0);
        result = Button_Clicked();
        TimeSpan worst = result.timeLength;
        climbs.Add(result.timeLength);

        // Average Time: 4.0.0
        TimeSpan average = new TimeSpan(4, 0, 0);

        ClimbDataStats test = StatisticsPage(climbs);
        Assert.True(3 == test.totalClimbs, "multipleEntries() - totalClimbs failure");
        Assert.True(best == test.bestTime, "multipleEntries() - bestTime failure");
        Assert.True(worst == test.worstTime, "multipleEntries() - worstTime failure");
        Assert.True(average == test.averageTime, "multipleEntries() - averageTime failure");
    }

    /// <summary>
    /// Asserts StatisticsPage() will return 1 for total climbs, 3.0.0 for best
    /// climb, 3.0.0 for worst climb, and 3.0.0 for average time when given list
    /// of 1 climb with the following time: 3.0.0.
    /// </summary>
    [Test]
    public void singularEntry() 
    {
        List<TimeSpan> climbs = new List<TimeSpan>();

        startTime=new DateTime(2010, 1, 1, 8, 0, 0);
        endTime=new DateTime(2010, 1, 1, 11, 0, 0);

        ClimbData result = Button_Clicked();
        TimeSpan time = result.timeLength;
        climbs.Add(result.timeLength);

        ClimbDataStats test = StatisticsPage(climbs);
        Assert.True(1 == test.totalClimbs, "singularEntry() - totalClimbs failure");
        Assert.True(time == test.bestTime, "singularEntry() - bestTime failure");
        Assert.True(time == test.worstTime, "singularEntry() - worstTime failure");
        Assert.True(time == test.averageTime, "singularEntry() - averageTime failure");
    }

    /// <summary>
    /// Asserts StatisticsPage() will return 0 for total climbs, 0.0.0 for best
    /// climb, 0.0.0 for worst climb, and 0.0.0 for average time when given list
    /// of 0 climbs.
    /// </summary>
    [Test]
    public void zeroEntries() 
    {
        List<TimeSpan> climbs = new List<TimeSpan>();
        TimeSpan time = new TimeSpan(0, 0, 0);

        ClimbDataStats test = StatisticsPage(climbs);
        Assert.True(0 == test.totalClimbs, "zeroEntries() - totalClimbs failure");
        Assert.True(time == test.bestTime, "zeroEntries() - bestTime failure");
        Assert.True(time == test.worstTime, "zeroEntries() - worstTime failure");
        Assert.True(time == test.averageTime, "zeroEntries() - averageTime failure");
    }

    /// <summary>
    /// A mock climb_higher.climbDataEntryPage.Button_Clicked() method to create
    /// climbs to add to a list of climbs for statistics testing. 
    /// </summary>
    /// <returns>ClimbData climb</returns>
    public ClimbData Button_Clicked()
    {
        ClimbData climb = null;
        return new ClimbData
        {
            grade = grade,
            tries = int.Parse(tries),
            walltype = walltype,
            color = color,
            notes = notes,
            title = title,
            isOutside = outside,
            routeType = routeType,
            timeLength = endTime - startTime
        };
    }
    
    /// <summary>
    /// A mock climb_higher.StatisticsPage.OnAppearing() method that uses the 
    /// same logic to calculate the total climb and best, worst, and average time.
    /// </summary>
    /// <param name="climbStat">List of climb times</param>
    /// <returns>An object containing information about the statistics</returns>
    public ClimbDataStats StatisticsPage (List<TimeSpan> climbStat) {
        ClimbDataStats stats = null;
        if (climbStat.Any()) {
            double doubleAverageTicks = climbStat.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
            stats = new ClimbDataStats
            {
                totalClimbs = climbStat.Count(),
                bestTime = climbStat.Min(),
                worstTime = climbStat.Max(),
                averageTime = new TimeSpan(longAverageTicks)
            };
        } else {
            stats = new ClimbDataStats
            {
                totalClimbs = 0,
                bestTime = new TimeSpan(0, 0, 0),
                worstTime = new TimeSpan(0, 0, 0),
                averageTime = new TimeSpan(0, 0, 0)
            };
        }

        return stats;
    }
}