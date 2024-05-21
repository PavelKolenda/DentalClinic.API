namespace DentalClinic.Services.Jobs.MutableTime;

public class MutableDateTime
{
    public DateTime Date { get; private set; }

    public MutableDateTime(DateTime date)
    {
        Date = date;
    }

    public void AddDays(int days)
    {
        Date = Date.AddDays(days);
    }
}