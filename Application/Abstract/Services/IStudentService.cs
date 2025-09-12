using Application.Dtos.Request.StudentDto;
using Application.Dtos.Response.StudentDto;

namespace Application.Abstract.Services;

public interface IStudentService
{
    Task<CreateStudentRequestDto> CreateAsync(CreateStudentRequestDto student, CancellationToken cancellationToken = default);
    Task<IEnumerable<GetStudentsResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GetStudentByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UpdateStudentRequestDto?> UpdateAsync(int id, UpdateStudentRequestDto student, CancellationToken cancellationToken = default);
    Task<StudentResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken = default);
}