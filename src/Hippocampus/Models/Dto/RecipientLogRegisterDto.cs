using Hippocampus.Models.Entities;
using Hippocampus.Models.Values;
using Hippocampus.Services.ApplicationValues;

namespace Hippocampus.Models.Dto;

public class RecipientLogRegisterDto
{
    public MacAddress MacAddress { get; init; } = new();
    public LevelPercentage LevelPercentage { get; init; } = new();
    public State State { get; init; } = State.Average;
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.New();
}