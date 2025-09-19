using Application.Abstract.Services;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ProfessorDto;
using Microsoft.AspNetCore.Mvc;

namespace University.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfessorController : ControllerBase
{
    private readonly IProfessorService _service;

    public ProfessorController(IProfessorService service)
    {
        _service = service;
    }

    [HttpPost]
    public async ValueTask<IActionResult> CreateAsync([FromBody] CreateProfessorRequestDto professorDto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(professorDto, cancellationToken);

        return Ok(created);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("filtered")]
    [ProducesResponseType(typeof(ProfessorsPageResponseDto), 200)]
    public async Task<ActionResult<ProfessorsPageResponseDto>> GetPagedAsync([FromQuery] GetProfessorsRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]int id, CancellationToken cancellationToken)
    {
        var professor = await _service.GetByIdAsync(id, cancellationToken);

        if (professor == null)
        {
            return NotFound();
        }
            
        return Ok(professor);
    }
    

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateFullAsync([FromRoute] int id, [FromBody] UpdateProfessorRequestDto professorDto, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateFullAsync(id, professorDto, cancellationToken);

        return Ok(updated);
    }

    [HttpPatch("/patch/{id:int}")]
    public async Task<IActionResult> UpdatePartialAsync([FromRoute]int id, [FromBody]UpdateProfessorFirstNameAndLastNameRequestDto professorDto, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdatePartialAsync(id, professorDto, cancellationToken);

        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(id, cancellationToken);

        if (deleted == null)
        {
            return NotFound();
        }

        return Ok(deleted);
    }
}