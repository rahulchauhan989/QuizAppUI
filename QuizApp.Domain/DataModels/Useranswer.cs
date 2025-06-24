namespace QuizApp.Domain.DataModels;

public class Useranswer
{
    public int Id { get; set; } 

    public int UserquizattemptId { get; set; } 
    public Userquizattempt Attempt { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public int OptionId { get; set; }
    public Option Option { get; set; } = null!;

    public bool Iscorrect { get; set; }
}