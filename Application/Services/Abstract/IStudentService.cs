using Application.Common.Pagination;
using Application.Dtos.Request;
using Application.Dtos.Request.StudentDto;
using Application.Dtos.Response;
using Application.Dtos.Response.StudentDto;

namespace Application.Services.Abstract;

public interface IStudentService
{
    Task<CreateStudentRequestDto> CreateAsync(CreateStudentRequestDto student, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentsResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GetStudentByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GetStudentByIdWithProfessorsDto?> GetByIdWithProfessorsAsync(int id, CancellationToken cancellationToken = default);
    Task<UpdateStudentRequestDto?> UpdateFullAsync(int id, UpdateStudentRequestDto student, CancellationToken cancellationToken = default);
    Task<UpdateStudentFirstNameAndLastNameRequestDto?> UpdatePartialAsync(int id, UpdateStudentFirstNameAndLastNameRequestDto stdentDto, CancellationToken cancellationToken = default);
    Task<StudentResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<StudentsPageResponseDto> GetPagedAsync(FilterOptions<GetStudentsFilterRequestDto> request, CancellationToken cancellationToken = default);
    Task<BulkUpdateDto> BulkUpdateAsync(List<BulkUpdateStudentRequestDto> items, CancellationToken cancellationToken = default);
    Task<int> AttachProfessorsAsync(AttachIdsDto dto, CancellationToken cancellationToken = default);
    Task<int> RemoveProfessorAsync(int studentId, int professorId, CancellationToken cancellationToken = default);
}