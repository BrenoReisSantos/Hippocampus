namespace Hippocampus.Domain.Models.Entities;

public class RecipientLevelLimit
{
    public float MaxHeight { get; init; }

    private float _minHeight;

    public float MinHeight
    {
        get => _minHeight;
        init
        {
            if (value < 0)
            {
                throw LessThanZeroException();
            }

            _minHeight = value;
        }
    }

    private static Exception LessThanZeroException() => new ArgumentException("MinValue can't be less than 0");
}