using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Response.ResponseProfessorDto;
using Domain.Entities;

namespace Application.Abstract.Services;

public interface IProfessorService
{
    Task<CreateProfessorRequestDto> CreateAsync(CreateProfessorRequestDto professor, CancellationToken cancellationToken);
    Task<IEnumerable<GetProfessorsResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<GetProfessorByIdResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<UpdateProfessorRequestDto?> UpdateAsync(int id, UpdateProfessorRequestDto professor, CancellationToken cancellationToken);
    Task<ProfessorResponseDto?> DeleteAsync(int id, CancellationToken cancellationToken);
}