using Xunit;
using Domain.Rubricators;
using Domain.Students;
using Domain.Services;
using Domain.Common;
using Domain.Factories;

namespace PosModule.Tests
{
    public class DomainTests
    {

        [Fact]
        public void Score_Cannot_Be_Negative()
        {
            Assert.Throws<DomainException>(() => new Score(-5));
        }

        [Fact]
        public void Weight_Cannot_Be_Out_Of_Range()
        {
            Assert.Throws<DomainException>(() => new Weight(0));
            Assert.Throws<DomainException>(() => new Weight(1.5));
        }

        [Fact]
        public void StudentName_Cannot_Be_Empty()
        {
            Assert.Throws<DomainException>(() => new StudentName("   "));
        }

        [Fact]
        public void Rubricator_Cannot_Add_Duplicate_Criterion()
        {
            var rubricator = new Rubricator();
            var id = CriterionId.New();
            var criterion1 = new Criterion(id, "Test", 10, new Weight(0.5));
            var criterion2 = new Criterion(id, "Duplicate", 10, new Weight(0.5));

            rubricator.AddCriterion(criterion1);

            Assert.Throws<DomainException>(() => rubricator.AddCriterion(criterion2));
        }

        [Fact]
        public void Student_Cannot_Have_Two_Evaluations_For_Same_Criterion()
        {
            var student = new Student(StudentId.New(), new StudentName("Ivan"));
            var criterionId = CriterionId.New();

            var eval1 = new Evaluation(EvaluationId.New(), criterionId, new Score(5), DateTime.UtcNow, null);
            var eval2 = new Evaluation(EvaluationId.New(), criterionId, new Score(10), DateTime.UtcNow, null);

            student.AddEvaluation(eval1);

            Assert.Throws<DomainException>(() => student.AddEvaluation(eval2));
        }


        [Fact]
        public void EvaluationFactory_Cannot_Exceed_Max_Score()
        {
            var student = new Student(StudentId.New(), new StudentName("Ivan"));
            var rubricator = new Rubricator();
            var criterion = new Criterion(CriterionId.New(), "Exam", 10, new Weight(1));
            rubricator.AddCriterion(criterion);
            var factory = new EvaluationFactory();

            Assert.Throws<DomainException>(() =>
                factory.Create(student, rubricator, criterion.Id, 12, "Too much"));
        }

        [Fact]
        public void TotalScoreCalculator_Calculates_Weighted_Sum_Correctly()
        {
            var student = new Student(StudentId.New(), new StudentName("Ivan"));
            var rubricator = new Rubricator();

            var c1 = new Criterion(CriterionId.New(), "HW", 10, new Weight(0.5));
            var c2 = new Criterion(CriterionId.New(), "Exam", 5, new Weight(0.5));
            rubricator.AddCriterion(c1);
            rubricator.AddCriterion(c2);

            var factory = new EvaluationFactory();
            factory.Create(student, rubricator, c1.Id, 8, null);
            factory.Create(student, rubricator, c2.Id, 5, null);

            var calculator = new TotalScoreCalculator(new WeightedSumStrategy());

            var result = calculator.Calculate(student, rubricator);

            Assert.Equal(9, result.Value);
        }
    }
}