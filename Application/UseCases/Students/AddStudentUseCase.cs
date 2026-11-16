using Application.Dto;
using Domain.Students;

namespace Application.UseCases.Students;

public sealed class AddStudentUseCase
{
    private readonly IStudentRepository _studentRepository;

    public AddStudentUseCase(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
    }

    public StudentDto Execute(string name)
    {
        var studentName = new StudentName(name);

        var studentId = StudentId.New();
        var student = new Student(studentId, studentName);

        _studentRepository.Save(student);

        return new StudentDto(student.Id.Value, student.Name.Value);
    }
}