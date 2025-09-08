using Application.Dtos.Request.ProfessorDto;
using Application.Dtos.Request.StudentDto;
using Application.Dtos.Response.ResponseProfessorDto;
using Application.Dtos.Response.ResponseStudentDto;
using Application.Dtos.Response.StudentDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Professor, CreateProfessorRequestDto>().ReverseMap();
        CreateMap<UpdateProfessorRequestDto, Professor>().ForMember(p => p.Id, x => x.Ignore()).ReverseMap();
            

        CreateMap<Professor, GetProfessorByIdResponseDto>().ReverseMap();
        CreateMap<Professor, GetProfessorsResponseDto>().ReverseMap();
        CreateMap<Professor, ProfessorResponseDto>().ReverseMap();

        CreateMap<Student, CreateStudentRequestDto>().ReverseMap();
        CreateMap<Student, UpdateStudentRequestDto>().ReverseMap();
        CreateMap<Student, GetStudentByIdResponseDto>().ReverseMap();
        CreateMap<Student, GetStudentsResponseDto>().ReverseMap();
        CreateMap<Student, StudentResponseDto>().ReverseMap();
    }
}