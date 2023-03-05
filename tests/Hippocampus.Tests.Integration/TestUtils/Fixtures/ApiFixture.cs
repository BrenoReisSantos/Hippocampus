namespace Hippocampus.Tests.Integration.TestUtils.Fixtures;

public class ApiFixture : DatabaseFixture
{
    protected HttpClient Api { get; private set; }

    [OneTimeSetUp]
    public void ApiFixtureOneTimeSetup()
    {
    }

    [SetUp]
    public void ApiFixtureSetupAsync()
    {
        Api = ApplicationFactory.CreateClient();
    }

    [TearDown]
    public void ApiFixtureTearDown()
    {
        Api.Dispose();
    }
}