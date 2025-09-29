using Application.Common.Extensions;
using Application.Common.Pagination;
using Application.Common.Results;
using Application.Dtos.Request;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ProfessorDto;
using Application.Dtos.Response.StudentDto;
using Application.Repositories.GenericRepository;
using Application.Services.Abstract;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Concrete;

public class ProfessorService(IGenericRepository<Professor> _professorRepository,
    IGenericRepository<ProfessorStudent> _professorStudentRepository,
    IGenericRepository<Student> _studentRepository,
    IMapper _maapper) : IProfessorService
{
    public async Task<CreateProfessorRequestDto> CreateAsync(CreateProfessorRequestDto professorDto, CancellationToken cancellationToken)
    {
        var professor = _maapper.Map<Professor>(professorDto);

        await _professorRepository.CreateAsync(professor, cancellationToken);

        return professorDto;
    }

    public async Task<IEnumerable<ProfessorsResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var professors = _professorRepository.GetAll(cancellationToken);

        var professorsDto = await professors.Select(p => new ProfessorsResponseDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Email = p.Email,
            Status = p.Status,
            HireDate = p.HireDate,
            CreatedAt = p.CreatedAt
        }).ToListAsync(cancellationToken);
        
        return professorsDto;
    }

    public async Task<GetProfessorByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var professor = await _professorRepository.GetByIdAsync(id, cancellationToken);

        if (professor == null)
        {
            return null;
        }

        var professorDto = _maapper.Map<GetProfessorByIdResponseDto>(professor);

        return professorDto;
    }

    public async Task<GetProfessorByIdWithStudentsDto?> GetByIdWithStudentsAsync(int id, CancellationToken cancellationToken = default)
    {
        var professor = await _professorRepository.GetByIdAsync(id, cancellationToken);

        if(professor == null)
        {
            return null;
        }

        var professorStudentsIds = await _professorStudentRepository
            .GetAll(cancellationToken)
            .Where(ps => ps.ProfessorId == id)
            .Select(ps => ps.StudentId)
            .ToListAsync(cancellationToken);

        var students = _studentRepository
            .GetAll(cancellationToken)
            .Where(s => professorStudentsIds.Contains(s.Id))
            .ToList();

        var result = new GetProfessorByIdWithStudentsDto
        {
            Id = professor.Id,
            FirstName = professor.FirstName,
            LastName = professor.LastName,
            Email = professor.Email,
            HireDate = professor.HireDate,
            Status = professor.Status,
            Students = students.Select(s => new StudentsResponseDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                EnrollmentDate = s.EnrollmentDate,
                Status = s.Status,
                CreatedAt = s.CreatedAt
            }).ToList()
        };

        return result;
    }

    public async Task<UpdateProfessorRequestDto?> UpdateFullAsync(int id, UpdateProfessorRequestDto professorDto, CancellationToken cancellationToken = default)
    {
        var professor = _maapper.Map<Professor>(professorDto);

        var prof = await _professorRepository.UpdateFullAsync(id, professor, cancellationToken);

        if(prof == null)
        {
            return null;
        }

        return professorDto;
    }

    public async Task<UpdateProfessorFirstNameAndLastNameRequestDto?> UpdatePartialAsync(int id, UpdateProfessorFirstNameAndLastNameRequestDto professorDto, CancellationToken cancellationToken = default)
    {
        var professor = new Professor
        {
            FirstName = professorDto.FirstName,
            LastName = professorDto.LastName,
        };
        
        var prof = await _professorRepository.UpdatePartialAsync(id, professor, cancellationToken);

        if(prof == null)
        {
            return null;
        }

        return professorDto;
    }

    public async Task<BulkUpdateResult> BulkUpdateAsync(List<BulkUpdateProfessorRequestDto> professorsDto, CancellationToken cancellationToken = default)
    {
        var result = new BulkUpdateResult();

        if (professorsDto == null || professorsDto.Count == 0)
        {
            return result;
        }

        var entities = new List<Professor>();

        foreach (var professorDto in professorsDto)
        {
            var professor = new Professor
            {
                Id = professorDto.Id,
                FirstName = professorDto.FirstName,
                LastName = professorDto.LastName,
                Email = professorDto.Email,
                HireDate = professorDto.HireDate,
                Status = professorDto.Status,
            };

            entities.Add(professor);
        }

        var bulkDto = await _professorRepository.UpdateBulkAsync(entities, cancellationToken);

        result.Updated = bulkDto.Updated;
        result.NotFoundIds = bulkDto.NotFoundIds;
        return result;
    }

    public async Task<ProfessorResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var professor = await _professorRepository.DeleteAsync(id, cancellationToken);

        if(professor == null)
        {
            return null;
        }

        var professorDto = _maapper.Map<ProfessorResponseDto>(professor);

        return professorDto;
    }

    public async Task<ProfessorsPageResponseDto> GetPagedAsync(FilterOptions<GetProfessorsFilterRequestDto> request, CancellationToken cancellationToken = default)
    {
        var q = _professorRepository.GetAll(cancellationToken)
            .Where(p => !p.IsDeleted);

        if (request.Filters is { } f)
        {
            if (!string.IsNullOrWhiteSpace(f.Name))
            {
                var name = f.Name;
                q = q.Where(p => p.FirstName == name || p.LastName == name);
            }

            if (!string.IsNullOrWhiteSpace(f.Email))
            {
                var email = f.Email;
                q = q.Where(p => p.Email == email);
            }

            if (f.Status.HasValue)
            {
                q = q.Where(p => p.Status == f.Status.Value);
            }

            if (f.MinStudents.HasValue)
            {
                q = q.Where(p => p.ProfessorStudents.Count >= f.MinStudents.Value);
            }
        }

        var total = await q.CountAsync(cancellationToken);
        if (total == 0)
        {
            return new ProfessorsPageResponseDto
            {
                Meta = PaginationInfo.Create(0, request.Page, request.PerPage),
                Items = new()
            };
        }

        var pageQ = ProfessorQueryableExtensions.Paginate(q, request);

        var items = await pageQ.Select(p => new GetProfessorDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Email = p.Email,
            Status = p.Status,
            HireDate = p.HireDate,
            CreatedAt = p.CreatedAt,
            StudentsCount = p.ProfessorStudents.Count
        }).ToListAsync(cancellationToken);

        var meta = PaginationInfo.Create(total, request.Page, request.PerPage);

        return new ProfessorsPageResponseDto { Meta = meta, Items = items };
    }

    public async Task<int> AttachStudentsAsync(AttachIdsDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Ids == null || dto.Ids.Count == 0) return 0;

        var professor = await _professorRepository.GetByIdAsync(dto.Id, cancellationToken);

        if (professor is null)
        {
            throw new KeyNotFoundException($"Student by id {dto.Id} not found ");
        }

        var requested = dto.Ids.Where(id => id > 0).Distinct().ToList();
        if (requested.Count == 0)
        {
            throw new KeyNotFoundException($"Student by id {dto.Id} not found ");
        }

        var existStudIds = await _studentRepository
            .GetAll(cancellationToken)
            .AsNoTracking()
            .Where(s => dto.Ids.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        if (existStudIds.Count == 0)
        {
            return 0;
        }

        var already = await _professorStudentRepository
            .GetAll(cancellationToken)
            .Where(ps => ps.ProfessorId == dto.Id && existStudIds.Contains(ps.StudentId))
            .Select(ps => ps.StudentId)
            .ToListAsync(cancellationToken);

        var toAdd = existStudIds.Except(already).ToList();

        if (toAdd.Count == 0)
        {
            return 0;
        }

        var rows = toAdd.Select(pid => new ProfessorStudent { ProfessorId = dto.Id, StudentId = pid });

        return await _professorStudentRepository.AddRangeAsync(rows, cancellationToken);
    }

    public async Task<int> RemoveStudentAsync(int professorId, int studentId, CancellationToken cancellationToken = default)
    {
        var affected = await _professorStudentRepository.DeleteFromListAsync(
            ps => ps.ProfessorId == professorId && ps.StudentId == studentId, cancellationToken);

        return affected;
    }
}