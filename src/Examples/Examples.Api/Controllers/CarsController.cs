using System.Collections.Generic;
using System.Linq;
using Example.Core.Exceptions;
using Example.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Examples.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly ILogger<CarsController> _logger;

    public CarsController(ILogger<CarsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Car> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Car()
            {
                Model = $"Model {index}"
            })
            .ToArray();
    }
    
    [HttpPost]
    public IEnumerable<Car> Post()
    {
        return Enumerable.Range(1, 5).Select(index => new Car()
            {
                Model = $"Model {index}"
            })
            .ToArray();
    }
}