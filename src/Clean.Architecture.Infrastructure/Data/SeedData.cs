using Bogus;
using Clean.Architecture.Core.ContributorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Architecture.Infrastructure.Data;

public static class SeedData
{
  //public static readonly Contributor Contributor1 = new(
  //    email: "Email1@Microsoft.com",
  //    firstName: "FirstName1",
  //    lastName: "LastName1",
  //    followers: 1,
  //    following: 2,
  //    stars: 3,
  //    status: ContributorStatus.NotSet.Name);
  //public static readonly Contributor Contributor2 = new(
  //    email: "Email2@Microsoft.com",
  //    firstName: "FirstName2",
  //    lastName: "LastName2",
  //    followers: 4,
  //    following: 5,
  //    stars: 6,
  //    status: ContributorStatus.NotSet.Name);

  private static readonly ContributorFaker _contributorFaker = new();
  public static Contributor Contributor1 = _contributorFaker.Generate();
  public static Contributor Contributor2 = _contributorFaker.Generate();

  public static void PopulateTestData(AppDbContext dbContext)
  {
    foreach (var item in dbContext.Contributors)
    {
      dbContext.Remove(item);
    }
    dbContext.SaveChanges();

    dbContext.Contributors.Add(Contributor1);
    dbContext.Contributors.Add(Contributor2);

    dbContext.SaveChanges();
  }

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null);
    // Look for any Contributors.
    if (dbContext.Contributors.Any())
    {
      return; // DB has been seeded
    }

    PopulateTestData(dbContext);
  }
}

public class ContributorFaker : Faker<Contributor>
{
  private int _initialId = 1;

  public ContributorFaker()
  {
    CustomInstantiator(f => new Contributor(
      email: f.Person.Email,
      firstName: f.Person.FirstName,
      lastName: f.Person.LastName,
      followers: f.Random.Int(0, 100),
      following: f.Random.Int(0, 100),
      stars: f.Random.Int(0, 100),
      status: f.PickRandom<ContributorStatus>(ContributorStatus.List).Name))
      .RuleFor(o => o.Id, f => _initialId++);
  }
}
