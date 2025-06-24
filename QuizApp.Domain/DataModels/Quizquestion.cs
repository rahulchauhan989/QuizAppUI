namespace QuizApp.Domain.DataModels;

public class Quizquestion
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public DateTime? Createdat { get; set; }
}
