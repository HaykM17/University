using Application.Abstract.Repasitories;
using Application.Abstract.Services;
using Application.Common.Extensions;
using Application.Common.Pagination;
using Application.Dtos.Request;
using Application.Dtos.Request.StudentDto;
using Application.Dtos.Response;
using Application.Dtos.Response.ProfessorDto;
using Application.Dtos.Response.StudentDto;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StudentService(IGenericRepository<Student> _studentRepository,
    IGenericRepository<ProfessorStudent> _professorStudentRepository,
    IGenericRepository<Professor> _professorRepository,
    IMapper _mapper) : IStudentService
{
    public async Task<CreateStudentRequestDto> CreateAsync(CreateStudentRequestDto studentDto, CancellationToken cancellationToken = default)
    {
        var student = _mapper.Map<Student>(studentDto);

        await _studentRepository.CreateAsync(student, cancellationToken);

        return studentDto;
    }

    public async Task<IEnumerable<StudentsResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var students = _studentRepository.GetAll(cancellationToken);

        var StudentsDto = await students.Select(s => new StudentsResponseDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            Status = s.Status,
            EnrollmentDate = s.EnrollmentDate,
            CreatedAt = s.CreatedAt,
        }).ToListAsync(cancellationToken);

        return StudentsDto;
    }

    public async Task<GetStudentByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);

        if (student == null)
        {
            return null;
        }

        var studentDto = _mapper.Map<GetStudentByIdResponseDto?>(student);

        return studentDto;
    }

    public async Task<GetStudentByIdWithProfessorsDto?> GetByIdWithProfessorsAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);

        if (student == null)
        {
            return null;
        }

        var professorStudentsIds = _professorStudentRepository
            .GetAll()
            .Where(ps => ps.StudentId == id)
            .Select(ps => ps.ProfessorId)
            .ToList();

        if (professorStudentsIds == null || professorStudentsIds.Count == 0)
        {
            return new GetStudentByIdWithProfessorsDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                EnrollmentDate = student.EnrollmentDate,
                Status = student.Status,
                Professors = new List<ProfessorsResponseDto>()
            };
        }

        var professors = _professorRepository
            .GetAll()
            .Where(p => professorStudentsIds.Contains(p.Id)).ToList();

        var result = new GetStudentByIdWithProfessorsDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            EnrollmentDate = student.EnrollmentDate,
            Status = student.Status,
            Professors = professors.Select(p => new ProfessorsResponseDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                HireDate = p.HireDate,
                Status = p.Status,
                CreatedAt = student.CreatedAt,
            }).ToList()
        };

        return result;
    }

    public async Task<UpdateStudentRequestDto?> UpdateFullAsync(int id, UpdateStudentRequestDto studentDto, CancellationToken cancellationToken = default)
    {
        var student = _mapper.Map<Student>(studentDto);

        var stud = await _studentRepository.UpdateFullAsync(id, student, cancellationToken);

        if (stud == null)
        {
            return null;
        }

        return studentDto;
    }

    public async Task<UpdateStudentFirstNameAndLastNameRequestDto?> UpdatePartialAsync(int id, UpdateStudentFirstNameAndLastNameRequestDto studentDto, CancellationToken cancellationToken = default)
    {
        var student = new Student
        {
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
        };

        var stud = await _studentRepository.UpdatePartialAsync(id, student, cancellationToken);

        if (stud == null)
        {
            return null;
        }

        return studentDto;
    }

    public async Task<BulkUpdateDto> BulkUpdateAsync(List<BulkUpdateStudentRequestDto> studentsDto, CancellationToken cancellationToken = default)
    {
        var result = new BulkUpdateDto();

        if (studentsDto == null || studentsDto.Count == 0)
        {
            return result;
        }

        var entities = new List<Student>();

        foreach (var studentDto in studentsDto)
        {
            var student = new Student
            {
                Id = studentDto.Id,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                EnrollmentDate = studentDto.EnrollmentDate,
                Status = studentDto.Status,
            };

            entities.Add(student);
        }

        var bulkDto = await _studentRepository.UpdateBulkAsync(entities, cancellationToken);

        result.Updated = bulkDto.Updated;
        result.NotFoundIds = bulkDto.NotFoundIds;
        return result;
    }

    public async Task<StudentResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.DeleteAsync(id, cancellationToken);
        if (student == null)
            return null;

        return _mapper.Map<StudentResponseDto>(student);
    }

    public async Task<StudentsPageResponseDto> GetPagedAsync(FilterOptions<GetStudentsFilterRequestDto> request, CancellationToken cancellationToken = default)
    {
        var q = _studentRepository.GetAll(cancellationToken)
            .Where(s => !s.IsDeleted);

        if (request.Filters is { } f)
        {
            if (!string.IsNullOrWhiteSpace(f.Name))
            {
                var name = f.Name;
                q = q.Where(s => s.FirstName == name || s.LastName == name);
            }

            if (!string.IsNullOrWhiteSpace(f.Email))
            {
                var email = f.Email;
                q = q.Where(s => s.Email == email);
            }

            if (f.Status.HasValue)
            {
                q = q.Where(s => s.Status == f.Status.Value);
            }

            if (f.MinProfessors.HasValue)
            {
                q = q.Where(s => s.ProfessorStudents.Count >= f.MinProfessors.Value);
            }
        }

        var total = await q.CountAsync(cancellationToken);
        if (total == 0)
        {
            return new StudentsPageResponseDto
            {
                Meta = PaginationInfo.Create(0, request.Page, request.PerPage),
                Items = new()
            };
        }

        var pageQ = StudentQueryableExtensions.Paginate(q, request);

        var items = await pageQ.Select(s => new GetStudentsResponseDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            Status = s.Status,
            EnrollmentDate = s.EnrollmentDate,
            CreatedAt = s.CreatedAt,
            ProfessorsCount = s.ProfessorStudents.Count
        }).ToListAsync(cancellationToken);

        var meta = PaginationInfo.Create(total, request.Page, request.PerPage);

        return new StudentsPageResponseDto { Meta = meta, Items = items };
    }

    public async Task<int> AttachProfessorsAsync(AttachIdsDto dto, CancellationToken ct = default)
    {
        if (dto.Ids == null || dto.Ids.Count == 0) return 0;

        var student = await _studentRepository.GetByIdAsync(dto.Id, ct);

        if (student is null)
        {
            throw new KeyNotFoundException($"Student by id {dto.Id} not found ");
        }

        var requested = dto.Ids.Where(id => id > 0).Distinct().ToList();
        if (requested.Count == 0)
        {
            throw new KeyNotFoundException($"Student by id {dto.Id} not found ");
        }

        var existProfIds = await _professorRepository
            .GetAll(ct)
            .AsNoTracking()
            .Where(p => dto.Ids.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(ct);

        if (existProfIds.Count == 0) 
        {
            return 0;
        }

        var already = await _professorStudentRepository
            .GetAll(ct)
            .Where(ps => ps.StudentId == dto.Id && existProfIds.Contains(ps.ProfessorId))
            .Select(ps => ps.ProfessorId)
            .ToListAsync(ct);

        var toAdd = existProfIds.Except(already).ToList();
        if (toAdd.Count == 0)
        {
            return 0;
        }

        var rows = toAdd.Select(pid => new ProfessorStudent { StudentId = dto.Id, ProfessorId = pid });

        return await _professorStudentRepository.AddRangeAsync(rows, ct);
    }

    public async Task<int> RemoveProfessorAsync(int studentId, int professorId, CancellationToken cancellationToken = default)
    {
        var affected = await _professorStudentRepository.DeleteFromListAsync(
            ps => ps.StudentId == studentId && ps.ProfessorId == professorId, cancellationToken);

        return affected;
    }
}