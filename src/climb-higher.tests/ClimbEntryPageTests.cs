namespace climb_higher.tests;

/// <summary>
/// Mock climb_higher.ClimbData class to be able to create ClimbData entities.
/// </summary>
public class ClimbData {
    public string routeType {get; set;}
    public string grade {get; set;}
    public int tries {get; set;}
    public string walltype {get; set;}
    public string color {get; set;}
    public string notes {get; set;}
    public string title {get; set;}
    public bool isOutside {get; set;}
    public TimeSpan timeLength {get; set; }
    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", 
            title, grade, tries, walltype, color, notes, isOutside, timeLength);
    }
}

/// <summary>
/// Tests the functionality of the current climb_higher.climbDataEntryPage.Button_Clicked()
/// method to ensure users are not able to enter incorrect information. The tests are split
/// between checking valid entries and invalid entries. 
/// </summary>
public class climbDataEntryPageTests
{  
    public DateTime startTime = new DateTime(2010, 1, 1, 8, 0, 0),
         endTime = new DateTime(2010, 1, 1, 11, 0, 0);
    public string grade = "gradeTest", tries = "1", walltype = "walltypeTest",
         color = "colorTest", notes = "notesTest", title = "titleTest";
    public bool outside=true;
    public string routeType="test";

    /// <summary>
    /// Asserts Button_Clicked() will not return null (i.e. will add a value
    /// to ClimbData in climb_higher) when all fields are entered correctly.
    /// </summary>
    [Test]
    public void validEntry_AllFields()
    {
        ClimbData result = Button_Clicked();
        Assert.IsNotNull(result);
    }

    /// <summary>
    /// Asserts Button_Clicked() will not return null (i.e. will add a value
    /// to ClimbData in climb_higher) when all fields are entered correctly
    /// with the user enter no notes. 
    /// </summary>
    [Test]
    public void validEntry_NoNotes()
    {
        notes = null;
        ClimbData result = Button_Clicked();
        Assert.IsNotNull(result);

        notes = "notesTest";
    }

    /// <summary>
    /// Asserts Button_Clicked() will return null (i.e. will not add a value
    /// to ClimbData in climb_higher) when there is a missing required field.
    /// </summary>
    [Test]
    public void invalidEntry_MissingFields() 
    {
        grade = null;
        ClimbData result = Button_Clicked();
        Assert.IsNull(result);

        grade = "gradeTest";
    }

    /// <summary>
    /// Asserts Button_Clicked() will return null (i.e. will not add a value
    /// to ClimbData in climb_higher) when there the entered startTime is 
    /// greater than or equal to the entered endTime.
    /// </summary>
    [Test]
    public void invalidEntry_InvalidTime() 
    {
        startTime = new DateTime(2010, 1, 1, 11, 0, 0);
        endTime = new DateTime(2010, 1, 1, 8, 0, 0);
        ClimbData result = Button_Clicked();
        Assert.IsNull(result);

        startTime = new DateTime(2010, 1, 1, 8, 0, 0);
        endTime = new DateTime(2010, 1, 1, 11, 0, 0);
    }

    /// <summary>
    /// Asserts Button_Clicked() will return null (i.e. will not add a value
    /// to ClimbData in climb_higher) when there is a non-integer valued
    /// entered into the tries field.
    /// </summary>
    [Test]
    public void invalidEntry_InvalidTries()
    {
        tries="notNum";
        ClimbData result = Button_Clicked();
        Assert.IsNull(result);

        tries="1";
    }

    /// <summary>
    /// A mock climb_higher.climbDataEntryPage.Button_Clicked() method to check
    /// the logic on the original with different entry field values.
    /// </summary>
    /// <returns>ClimbData climb or null</returns>
    public ClimbData Button_Clicked()
    {
        ClimbData climb = null;
        int check = 0;
        
        if (endTime <= startTime) {
            return null;
        }

        if (grade == null || walltype == null 
            || color == null || title == null) {
            return null;
        }

        if (tries == null || !int.TryParse(tries, out check) || check <= 0) {
            return null;
        }

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
}