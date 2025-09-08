using Application.Abstract.Repasitories;
using Application.Abstract.Services;
using Domain.Entities;

namespace Infrastructure.Services;

//public class StudentService : IStudentService
//{
//    private readonly IGenericRepository<Student> _repository;

//    public StudentService(IGenericRepository<Student> repository)
//    {
//        _repository = repository;
//    }

//    public async Task<Student> CreateAsync(Student student, CancellationToken cancellationToken = default)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<Student?> DeleteAsync(int id, CancellationToken cancellationToken = default)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken = default)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<Student?> UpdateAsync(int id, Student student, CancellationToken cancellationToken = default)
//    {
//        throw new NotImplementedException();
//    }
//}