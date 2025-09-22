using Application.Common.Pagination;
using Application.Common.Results;
using Application.Dtos.Request;
using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ProfessorDto;

namespace Application.Services.Abstract;

public interface IProfessorService
{
    Task<CreateProfessorRequestDto> CreateAsync(CreateProfessorRequestDto professorDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProfessorsResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GetProfessorByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GetProfessorByIdWithStudentsDto?> GetByIdWithStudentsAsync(int id, CancellationToken cancellationToken = default);
    Task<UpdateProfessorRequestDto?> UpdateFullAsync(int id, UpdateProfessorRequestDto professorDto, CancellationToken cancellationToken = default);
    Task<UpdateProfessorFirstNameAndLastNameRequestDto?> UpdatePartialAsync(int id, UpdateProfessorFirstNameAndLastNameRequestDto professorDto, CancellationToken cancellationToken = default);
    Task<ProfessorResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ProfessorsPageResponseDto> GetPagedAsync(FilterOptions<GetProfessorsFilterRequestDto> request, CancellationToken cancellationToken = default);
    Task<BulkUpdateResult> BulkUpdateAsync(List<BulkUpdateProfessorRequestDto> items, CancellationToken cancellationToken = default);
    Task<int> AttachStudentsAsync(AttachIdsDto dto, CancellationToken cancellationToken = default);
    Task<int> RemoveStudentAsync(int professorId, int studentId, CancellationToken cancellationToken = default);
}