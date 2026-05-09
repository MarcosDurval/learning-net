using DBZ.Application.Abstractions;
using DBZ.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DBZ.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacesController : ControllerBase
{
    private readonly IRaceProfileProvider _raceProfileProvider;

    public RacesController(IRaceProfileProvider raceProfileProvider)
    {
        _raceProfileProvider = raceProfileProvider;
    }

    [HttpGet]
    public IActionResult GetRaces()
    {
        var races = _raceProfileProvider.GetProfiles()
            .Select(profile => new RaceResult
            {
                Race = profile.Race,
                PowerBonus = profile.PowerBonus,
                Transformations = profile.Transformations.Keys.ToArray()
            });

        return Ok(races);
    }
}
