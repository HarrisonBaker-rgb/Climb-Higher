using SQLite;

namespace climb_higher;
/// <summary>
/// Object to represent time that gets put in a database of times
/// Likely would have been better to just use TimeSpan, although
/// TimeSpan is a bit mroe difficult to get the hang of at first,
/// but these time objects can be converted to a TimeSpan.
/// </summary>
[Table("StopwatchTracker")]
    public class Time
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; }

        public long Mins {  get; set; }
        public long Secs {  get; set; }
        public long Millisecs {  get; set; }
        public string timeString
        { get { return this.toString(); } }

        public String toString()
        {
            return Mins.ToString("D2") + ":" + Secs.ToString("D2") + "." + Millisecs.ToString("D3");
                
        }

        public TimeSpan toTimeSpan()
        {
            return new TimeSpan(0, 0, (int)Mins, (int)Secs, (int)Millisecs);
        }

    }
