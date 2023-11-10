using Example.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Examples.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FeaturesController: ControllerBase
{
    private IFeatureManager _featureManager;
    public FeaturesController(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Car>> GetTwo()
    {
        if (await _featureManager.IsEnabledAsync("FeatureA"))
        {
                return Enumerable.Range(1, 5).Select(index => new Car()
                    {
                        Model = $"Model {index}"
                    })
                    .ToArray();
        }
        return null;
    }
    
    [HttpGet("feature-b")]
    [FeatureGate("FeatureB")]
    public async Task<IEnumerable<Car>> GetThree()
    {
            return Enumerable.Range(1, 5).Select(index => new Car()
                {
                    Model = $"Examples {index}"
                })
                .ToArray();
        return null;
    }
}