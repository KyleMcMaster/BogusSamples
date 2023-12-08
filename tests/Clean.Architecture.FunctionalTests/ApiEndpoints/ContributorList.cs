using Ardalis.HttpClientTestExtensions;
using Bogus;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Web.Endpoints.ContributorEndpoints;
using FluentAssertions;
using Xunit;

namespace Clean.Architecture.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ContributorList(CustomWebApplicationFactory<Program> factory) 
  : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
  public void GeneratesDefaultData()
  {
    var faker = new Faker<BogusEntity>();
    var entity = faker.Generate();
    //string json = System.Text.Json.JsonSerializer.Serialize(entity);
  }

  [Fact]
  public void GeneratesConfiguredData()
  {
    var faker = new Faker<BogusEntity>()
      .RuleFor(o => o.Id, f => f.Random.Guid())
      .RuleFor(o => o.Name, f => f.Name.FullName())
      .RuleFor(o => o.DateOfBirth, f => f.Date.Past());

    var entity = faker.Generate();

    // Generates something like this:
    //{
    //  "Id": "f5ec1d0f-f1de-398b-1c2b-592a4921cc04",
    //  "DateOfBirth": "2023-10-31T22:04:22.7426011-04:00",
    //  "Name": "Lora Kuvalis"
    //}

    var strictFaker = new Faker<BogusEntity>()
      .StrictMode(true)
      .RuleFor(o => o.Id, f => f.Random.Guid())
      .RuleFor(o => o.Name, f => f.Random.String());

    // Throws a Bogus.ValidationException since DateOfBirth is not configured
    var strictEntity = strictFaker.Generate(); 
  }

  [Fact]
  public async Task ReturnsTwoContributorsOriginal()
  {
    var result = await _client.GetAndDeserializeAsync<ContributorListResponse>("/Contributors");

    Assert.Equal(2, result.Contributors.Count);
    Assert.Contains(result.Contributors, i => i.Id == SeedData.Contributor1.Id);
    Assert.Contains(result.Contributors, i => i.Id == SeedData.Contributor2.Id);
  }

  [Fact]
  public void DemosGenerating100Contributors()
  {
    int count = 100;
    var faker = new ContributorFaker();
    var contributors = faker.Generate(count);

    //string json = System.Text.Json.JsonSerializer.Serialize(contributors);

    contributors.Should().HaveCount(count);
  }
}

public class BogusEntity
{
  public Guid Id { get; set; }
  public DateTime DateOfBirth { get; set; }
  public string Name { get; set; } = "";
}
