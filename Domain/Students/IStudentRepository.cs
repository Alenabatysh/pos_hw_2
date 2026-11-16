namespace Domain.Students;

public interface IStudentRepository
{
    Student? FindById(StudentId id);
    IReadOnlyList<Student> FindAll();
    void Save(Student student);
    void Delete(StudentId id);
}