using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Services.SpecializationService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Services.Api.Controllers;

[ApiController]
public class SpecializationsController : ControllerBase
{
    
    private readonly ISpecializationService _specializationService;

    public SpecializationsController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetSpecializations()
    {
        return Ok(await _specializationService.GetSpecializationsAsync(!User.IsInRole("Receptionist")));
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<ActionResult<SpecializationWithServicesDto>> GetSpecializationInfo([FromRoute] Guid id)
    {
        return Ok(await _specializationService.GetSpecializationInfoAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateSpecialization([FromBody] SpecializationDto newSpecialization)
    {
        return Ok((await _specializationService.CreateSpecializationAsync(newSpecialization)).ToString());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditSpecialization([FromRoute] Guid id, [FromBody] SpecializationEditStatusDto updatedSpecialization)
    {

        await _specializationService.UpdateSpecializationAsync(id, updatedSpecialization);
        return Ok();
        // Polymorphic deserialization (StatusUpdate, CompleteUpdate)
    }
    
    
}