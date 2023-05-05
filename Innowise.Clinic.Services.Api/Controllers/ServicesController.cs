using Innowise.Clinic.Services.Dto;
using Innowise.Clinic.Services.Services.ServiceService.Interfaces;
using Innowise.Clinic.Shared.BaseClasses;
using Innowise.Clinic.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Services.Api.Controllers;

[Route("services")]
public class ServicesController : ApiControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
    {
        var isDisplayOnlyActiveServices = !User.IsInRole("Receptionist");
        return Ok(await _serviceService.GetServicesAsync(isDisplayOnlyActiveServices));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceDto>> GetServiceInfo([FromRoute] Guid id)
    {
        var serviceInfo = await _serviceService.GetServiceInfoAsync(id);
        if (!serviceInfo.IsActive && !User.IsInRole("Receptionist"))
        {
            return Forbid();
        }

        return Ok(serviceInfo);
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
        await _serviceService.UpdateServiceAsync(id, updatedServiceDto);
        return Ok();
    }
}