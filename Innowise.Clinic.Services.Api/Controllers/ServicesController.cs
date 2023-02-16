using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Services.ServiceService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Services.Api.Controllers;

[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
    {
        // results for certain category + specialization could be requested

        return Ok(await _serviceService.GetServicesAsync(!User.IsInRole("Receptionist")));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceDto>> GetServiceInfo([FromRoute] Guid id)
    {
        // figure out whether authorization is needed here
        // if allowed anonymous, how to restrict users from watching unavailable services?

        return Ok(await _serviceService.GetServiceAsyncInfoAsync(id));
    }

    [HttpPost]
    [Authorize(Roles = "Receptionist")]
    public async Task<ActionResult<ServiceDto>> CreateService([FromBody] ServiceDto newService)
    {
        return Ok((await _serviceService.CreateServiceAsync(newService)).ToString());
    }


    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> EditService([FromRoute] Guid id, [FromBody] ServiceEditStatusDto updatedServiceDto)
    {
        // Polymorphic deserialization (StatusUpdate, CompleteUpdate)
        await _serviceService.UpdateServiceAsync(id, updatedServiceDto);
        return Ok();
    }
}