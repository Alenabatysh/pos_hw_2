namespace Domain.Rubricators;

public interface IRubricatorRepository
{
    Rubricator? Find();
    void Save(Rubricator rubricator);
}