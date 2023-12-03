using Clean.Architecture.UseCases.Contributors;
using Clean.Architecture.UseCases.Contributors.List;

namespace Clean.Architecture.Infrastructure.Data.Queries;

public class FakeListContributorsQueryService : IListContributorsQueryService
{
  public Task<IEnumerable<ContributorDTO>> ListAsync()
  {
    List<ContributorDTO> result = 
        [new ContributorDTO(1, "Fake Contributor 1 FirstName", "Fake Contributor 1 LastName"), 
        new ContributorDTO(2, "Fake Contributor 2 FirstName", "Fake Contributor 2 LastName")];

    return Task.FromResult(result.AsEnumerable());
  }
}
