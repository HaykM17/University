using Application.Common.Pagination;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ProfessorDto;

namespace Application.Abstract.Services;

public interface IProfessorService
{
    Task<CreateProfessorRequestDto> CreateAsync(CreateProfessorRequestDto professorDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProfessorsResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GetProfessorByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UpdateProfessorRequestDto?> UpdateFullAsync(int id, UpdateProfessorRequestDto professorDto, CancellationToken cancellationToken = default);
    Task<UpdateProfessorFirstNameAndLastNameRequestDto?> UpdatePartialAsync(int id, UpdateProfessorFirstNameAndLastNameRequestDto professorDto, CancellationToken cancellationToken = default);
    Task<ProfessorResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<ProfessorsPageResponseDto> GetPagedAsync(FilterOptions<GetProfessorsFilterRequestDto> request, CancellationToken cancellationToken = default);
}