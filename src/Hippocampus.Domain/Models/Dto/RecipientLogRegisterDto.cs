using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Domain.Models.Dto;

public class RecipientLogRegisterDto
{
    public MacAddress MacAddress { get; init; } = new();
    public LevelPercentage LevelPercentage { get; init; } = new();
    public WaterTankId WaterTankId { get; init; } = WaterTankId.New();
}
