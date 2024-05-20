namespace DentalClinic.Services.Jobs.MutableTime;

public class MutableTimeOnly
{
    public TimeOnly Time { get; private set; }

    public MutableTimeOnly(TimeOnly time)
    {
        Time = time;
    }

    public void AddMinutes(int minutes)
    {
        Time = Time.AddMinutes(minutes);
    }

    public void AddHours(int hours)
    {
        Time = Time.AddHours(hours);
    }
}
