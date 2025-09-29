using Application.Common.Exceptions;
using Application.Dtos.Request;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ProfessorDto;
using Application.Services.Abstract;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace University.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ProfessorController : ControllerBase
{
    private readonly IProfessorService _professorService;

    public ProfessorController(IProfessorService professorService)
    {
        _professorService = professorService;
    }
    
    [HttpPost]
    [Authorize(Roles = "Writer")]
    public async ValueTask<IActionResult> CreateAsync([FromBody] CreateProfessorRequestDto professorDto, CancellationToken cancellationToken)
    {
        if(professorDto == null)
        {
            throw new RestApiException("Professor object is required");
        }

        var created = await _professorService.CreateAsync(professorDto, cancellationToken);

        return Ok(created);
    }

    
    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _professorService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("filtered")]
    [ProducesResponseType(typeof(ProfessorsPageResponseDto), 200)]
    [Authorize(Roles = "Reader")]
    public async Task<ActionResult<ProfessorsPageResponseDto>> GetPagedAsync([FromQuery] GetProfessorsRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _professorService.GetPagedAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    //[Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]int id, CancellationToken cancellationToken)
    {
        var professor = await _professorService.GetByIdAsync(id, cancellationToken);

        if (professor == null)
        {
            throw new NotFoundException(nameof(Professor), id);
        }
            
        return Ok(professor);
    }

    [HttpPut("bulk")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> BulkUpdateAsync([FromBody] List<BulkUpdateProfessorRequestDto> professorsDto, CancellationToken cancellationToken)
    {
        var result = await _professorService.BulkUpdateAsync(professorsDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}/students")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetByIdWithStudentsAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var professorAndStudents = await _professorService.GetByIdWithStudentsAsync(id, cancellationToken);

        if(professorAndStudents == null)
        {
            throw new NotFoundException(nameof(Professor), id);
        }

        return Ok(professorAndStudents);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateFullAsync([FromRoute] int id, [FromBody] UpdateProfessorRequestDto professorDto, CancellationToken cancellationToken)
    {
        var updated = await _professorService.UpdateFullAsync(id, professorDto, cancellationToken);

        if(updated == null)
        {
            throw new NotFoundException(nameof(Professor), id);
        }

        return Ok(updated);
    }

    [HttpPatch("/patch/{id:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdatePartialAsync([FromRoute]int id, [FromBody]UpdateProfessorFirstNameAndLastNameRequestDto professorDto, CancellationToken cancellationToken)
    {
        var updated = await _professorService.UpdatePartialAsync(id, professorDto, cancellationToken);

        if (updated == null)
        {
            throw new NotFoundException(nameof(Professor), id);
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _professorService.DeleteAsync(id, cancellationToken);

        if (deleted == null)
        {
            throw new NotFoundException(nameof(Professor), id);
        }

        return Ok(deleted);
    }

    [HttpPost]
    [Route("{id}/students")]
    //[Authorize(Roles = "Writer")]
    public async Task<IActionResult> AttachStudentsAsync([FromRoute] int id, [FromBody] List<int> ids, CancellationToken cancellationToken)
    {
        if (ids is null || ids.Count == 0)
        {
            throw new RestApiException("ids is required");
        }

        var attachDto = new AttachIdsDto
        {
            Id = id,
            Ids = ids
        };

        var result = await _professorService.AttachStudentsAsync(attachDto, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}/students/{studentId:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> RemoveStudentAsync([FromRoute]int id, [FromRoute]int studentId, CancellationToken cancellationToken)
    {
        var result = await _professorService.RemoveStudentAsync(id, studentId, cancellationToken);

        if(result == 0)
        {
            throw new NotFoundException(nameof(Professor), id);
        }

        return Ok(result);
    }
}