using Application.Dtos.Request;
using Application.Dtos.Request.StudentDto;
using Application.Dtos.Response;
using Application.Dtos.Response.StudentDto;
using Application.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace University.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateStudentRequestDto), 201)]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateStudentRequestDto studentDto, CancellationToken cancellationToken)
    {
        if (studentDto == null)
        {
            return BadRequest();
        }

        var student = await _studentService.CreateAsync(studentDto, cancellationToken);

        return StatusCode(201, student);
    }

    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _studentService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("filtered")]
    [ProducesResponseType(typeof(StudentsPageResponseDto), 200)]
    [Authorize(Roles = "Reader")]
    public async Task<ActionResult<StudentsPageResponseDto>> GetPagedAsync([FromQuery] GetStudentsRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _studentService.GetPagedAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetStudentByIdResponseDto), 200)]
    [Authorize(Roles = "Reader")]
    public async ValueTask<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var student = await _studentService.GetByIdAsync(id, cancellationToken);

        if (student == null)
        {
            return NotFound();
        }

        return StatusCode(200, student);
    }

    [HttpGet("{id:int}/professors")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetByIdWithProfessorsAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var studentAndProfessors = await _studentService.GetByIdWithProfessorsAsync(id, cancellationToken);

        if (studentAndProfessors == null)
        {
            return NotFound();
        }

        return Ok(studentAndProfessors);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateFullAsync([FromRoute] int id, [FromBody] UpdateStudentRequestDto studentDto, CancellationToken cancellationToken)
    {
        var result = await _studentService.UpdateFullAsync(id, studentDto, cancellationToken);

        if(result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdatePartialAsync([FromRoute] int id, [FromBody] UpdateStudentFirstNameAndLastNameRequestDto studentDto, CancellationToken cancellationToken)
    {
        var result = await _studentService.UpdatePartialAsync(id,studentDto, cancellationToken);

        if( result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut("bulk")]
    [Authorize(Roles = "Writer")]
    public async Task<ActionResult<BulkUpdateDto>> BulkUpdateAsync([FromBody] List<BulkUpdateStudentRequestDto> studentsDto, CancellationToken cancellationToken)
    {
        var result = await _studentService.BulkUpdateAsync(studentsDto, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _studentService.DeleteAsync(id, cancellationToken);

        if(result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("{id}/professors")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> AttachProfessorsAsync([FromRoute]int id, [FromBody]List<int> ids, CancellationToken cancellationToken)
    {
        if (ids is null || ids.Count == 0)
        {
            return BadRequest("ids is required");
        }

        var attachDto = new AttachIdsDto
        {
            Id = id,
            Ids = ids
        };

        var result = await _studentService.AttachProfessorsAsync(attachDto, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}/professors/{professorId:int}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> RemoveProfessor([FromRoute]int id, [FromRoute]int professorId, CancellationToken cancellationToken)
    {
        var result = await _studentService.RemoveProfessorAsync(id, professorId, cancellationToken);

        if(result == 0)
        {
            NotFound();
        }

        return Ok(result);
    }
}