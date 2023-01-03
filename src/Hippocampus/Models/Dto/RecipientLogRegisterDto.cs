using Hippocampus.Models.Values;

namespace Hippocampus.Models.Dto;

public class RecipientLogRegisterDto
{
    public MacAddress MacAddress { get; init; } = new();

    public int Level { get; init; }

    public State State { get; init; }
}

public static class RecipientLogRegisterDtoExtensions
{
    public static RecipientLog ToEntity(this RecipientLogRegisterDto recipientDto) => new RecipientLog
    {
        MacAddress = recipientDto.MacAddress,
        State = recipientDto.State,
        Level = recipientDto.Level,
        Id = RecipientLogId.New(),
        RegisterDate = DateTime.Now
    };
}