using Application.Abstract.Repasitories;
using Application.Abstract.Services;
using Application.Common.Extensions;
using Application.Common.Pagination;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ResponseProfessorDto;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProfessorService : IProfessorService
{
    private readonly IGenericRepository<Professor> _professorRepository;
    private readonly IMapper _maapper;

    public ProfessorService(IGenericRepository<Professor> professorRepository, IMapper maapper)
    {
        _professorRepository = professorRepository;
        _maapper = maapper;
    }

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

    public async Task<ProfessorsPageResponseDto> GetPagedAsync(
        FilterOptions<GetProfessorsFilterRequestDto> request,
        CancellationToken ct = default)
    {
        // 1) База: IQueryable из репозитория
        var q = _professorRepository.GetAll(ct)
            .Where(p => !p.IsDeleted);

        // 2) Предметные фильтры
        if (request.Filters is { } f)
        {
            if (!string.IsNullOrWhiteSpace(f.Name))
            {
                // ВНИМАНИЕ: StringComparison EF не переведёт в SQL.
                // Если нужна case-insensitive проверка гарантированно — используй collation БД или ToLower().
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

        // 3) Total ДО пагинации
        var total = await q.CountAsync(ct);
        if (total == 0)
        {
            return new ProfessorsPageResponseDto
            {
                Meta = PaginationInfo.Create(0, request.Page, request.PerPage),
                Items = new()
            };
        }

        // 4) Сортировка + пагинация (кастом для Professors)
        var pageQ = ProfessorQueryableExtensions.Paginate(q, request);

        // 5) Текущая страница → DTO
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
        }).ToListAsync(ct);

        // 6) Мета
        var meta = PaginationInfo.Create(total, request.Page, request.PerPage);

        return new ProfessorsPageResponseDto { Meta = meta, Items = items };
    }
}