using Innowise.Clinic.Services.Services.ServiceService.Interfaces;
using Innowise.Clinic.Services.Services.SpecializationService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Services.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HelperServicesController : ControllerBase
{
    [HttpGet("ensure-exists/specialization/{id:guid}")]
    public async Task<IActionResult> EnsureSpecializationExists([FromRoute] Guid id,
        [FromServices] ISpecializationService specializationService)
    {
        await specializationService.GetSpecializationInfoAsync(id);
        return Ok();
    }

    [HttpGet("ensure-exists/service/{id:guid}")]
    public async Task<IActionResult> EnsureServiceExists([FromRoute] Guid id, [FromQuery] Guid? specializationId,
        [FromServices] IServiceService serviceService)
    {
        var serviceDto = await serviceService.GetServiceInfoAsync(id);
        if (specializationId != null && specializationId != serviceDto.SpecializationId) return BadRequest();
        return Ok();
    }
}