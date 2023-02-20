using Innowise.Clinic.Services.Services.SpecializationService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Services.Api.Controllers;

[ApiController]
[Route("{controller}")]
public class HelperServicesController : ControllerBase
{
    private readonly ISpecializationService _specializationService;

    public HelperServicesController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }

    [HttpGet("ensure-exists/specialization/{id:guid}")]
    public async Task<IActionResult> EnsureSpecializationExists([FromRoute] Guid id)
    {
        await _specializationService.GetSpecializationInfoAsync(id);
        return Ok();
    }
}