using Application.Abstract.Repasitories;
using Application.Abstract.Services;
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

    public async Task<IEnumerable<GetProfessorsResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var professors = _professorRepository.GetAll(cancellationToken);

        var professorsDto = await professors.Select(p => new GetProfessorsResponseDto
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
}