using Application.Common.Pagination;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ResponseProfessorDto;

namespace Application.Abstract.Services;

public interface IProfessorService
{
    Task<CreateProfessorRequestDto> CreateAsync(CreateProfessorRequestDto professorDto, CancellationToken cancellationToken);
    Task<IEnumerable<ProfessorsResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<GetProfessorByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<UpdateProfessorRequestDto?> UpdateFullAsync(int id, UpdateProfessorRequestDto professorDto, CancellationToken cancellationToken);
    Task<UpdateProfessorFirstNameAndLastNameRequestDto?> UpdatePartialAsync(int id, UpdateProfessorFirstNameAndLastNameRequestDto professorDto, CancellationToken cancellationToken);
    Task<ProfessorResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<ProfessorsPageResponseDto> GetPagedAsync(FilterOptions<GetProfessorsFilterRequestDto> request, CancellationToken cancellationToken = default);
}