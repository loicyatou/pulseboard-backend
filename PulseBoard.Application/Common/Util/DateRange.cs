public class DateRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateRange(DateTime start, DateTime end)
    {
        if (end < start)
            throw new ArgumentException("End date cannot be before start date.");

        Start = start;
        End = end;
    }
}
