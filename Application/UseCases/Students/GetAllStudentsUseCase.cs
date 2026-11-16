using Application.Dto;
using Domain.Students;

namespace Application.UseCases.Students;

public sealed class GetAllStudentsUseCase
{
    private readonly IStudentRepository _studentRepository;

    public GetAllStudentsUseCase(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
    }

    public IReadOnlyList<StudentDto> Execute()
    {
        var students = _studentRepository.FindAll();

        return students
            .Select(s => new StudentDto(s.Id.Value, s.Name.Value))
            .ToList();
    }
}