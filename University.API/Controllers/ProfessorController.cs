using Application.Abstract.Services;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ResponseProfessorDto;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetProfessorsResponseDto>>> GetAll(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetProfessorByIdResponseDto>> GetById(int id, CancellationToken ct)
    {
        var dto = await _service.GetByIdAsync(id, ct);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CreateProfessorRequestDto>> Create([FromBody] CreateProfessorRequestDto dto, CancellationToken ct)
    {
        if (dto == null) return BadRequest();


        var created = await _service.CreateAsync(dto, ct);
        return Ok(created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UpdateProfessorRequestDto>> Update(int id, [FromBody] UpdateProfessorRequestDto dto, CancellationToken ct)
    {
        if (dto == null) return BadRequest();

        var updated = await _service.UpdateAsync(id, dto, ct);
        if (updated == null) return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProfessorResponseDto>> Delete(int id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        if (deleted == null) return NotFound();

        return Ok(deleted);
    }
}