
using Blazor.HttpInterfaceProxy;

public record TestDto(string Name);
public record TestPageInputDto(int Page);

public interface ITestService
{
    [ApiPost]
    Task<List<TestDto>> PageAsync(TestPageInputDto input);
}
