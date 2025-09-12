using Application.Abstract.Repasitories;
using Application.Abstract.Services;
using Application.Common.Extensions;
using Application.Common.Pagination;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ProfessorDto;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProfessorService(IGenericRepository<Professor> _professorRepository, IMapper _maapper) : IProfessorService
{
    public async Task<CreateProfessorRequestDto> CreateAsync(CreateProfessorRequestDto professorDto, CancellationToken cancellationToken)
    {
        var professor = _maapper.Map<Professor>(professorDto);

        await _professorRepository.CreateAsync(professor, cancellationToken);

        return professorDto;
    }

    public async Task<IEnumerable<ProfessorsResponseDto>> GetAllAsync(CancellationToken cancellationToken)
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
        }).ToListAsync();
        
        return professorsDto;
    }

    public async Task<GetProfessorByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var professor = await _professorRepository.GetByIdAsync(id, cancellationToken);

        if (professor == null)
        {
            return null;
        }

        var professorDto = _maapper.Map<GetProfessorByIdResponseDto>(professor);

        return professorDto;
    }

    public async Task<UpdateProfessorRequestDto?> UpdateFullAsync(int id, UpdateProfessorRequestDto professorDto, CancellationToken cancellationToken)
    {
        var professor = _maapper.Map<Professor>(professorDto);

        var prof = await _professorRepository.UpdateFullAsync(id, professor, cancellationToken);

        if(prof == null)
        {
            return null;
        }

        return professorDto;
    }

    public async Task<UpdateProfessorFirstNameAndLastNameRequestDto?> UpdatePartialAsync(int id, UpdateProfessorFirstNameAndLastNameRequestDto professorDto, CancellationToken cancellationToken)
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

    public async Task<ProfessorResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken)
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
}