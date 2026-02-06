
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TestController:ControllerBase
{
    [HttpPost]
    [Route("api/web/[controller]/[action]")]
    public List<TestDto> PageAsync(TestPageInputDto input)
        => new(){ new("One"), new("Two") };
}
