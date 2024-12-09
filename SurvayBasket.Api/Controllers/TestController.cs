using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurvayBasket.Api.Interfaces;

namespace SurvayBasket.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IOS _mac;
    private readonly IOS _windows;
    private readonly ILogger<TestController> _logger;

    public TestController(

        [FromKeyedServices("windos")] IOS windows,
        [FromKeyedServices("mac")] IOS mac,

        ILogger<TestController> logger)
    {
        _windows = windows;
        _mac = mac;
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Get(
        //[FromKeyedServices("mac")] IOS mac, 
        //[FromKeyedServices("windos")] IOS windos
        )
    {
        _logger.LogWarning("Windows :{0}", _windows.Run());
        _logger.LogError("MAC :{0}", _mac.Run());

        return Ok();
    }
}
