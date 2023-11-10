using System.Collections.Generic;
using System.Linq;
using Example.Core.Exceptions;
using Example.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Examples.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamplesController : ControllerBase
{
    private readonly ILogger<ExamplesController> _logger;
    private IMemoryCache _cache;

    public ExamplesController(ILogger<ExamplesController> logger, IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    [HttpGet]
    public async Task<string> GetOne()
    {
        var response = await _cache.GetOrCreateAsync<string>("key", async x =>
        {
            x.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
            return $"Example {DateTime.Now.ToLongTimeString()}";
        });
        return response;
    }
}