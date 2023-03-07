namespace Hippocampus.Domain.Services.ApplicationValues;

public interface IClock
{
    DateTime Now { get; }
    DateOnly Today => DateOnly.FromDateTime(Now);
}

public class Clock : IClock
{
    public DateTime Now => DateTime.UtcNow;
}