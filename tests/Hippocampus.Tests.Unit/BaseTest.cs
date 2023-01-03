using Bogus;
using Hippocampus.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace Hippocampus.Tests.Specs;

public class BaseTest
{
    protected readonly Faker faker = new("pt_BR");
}
