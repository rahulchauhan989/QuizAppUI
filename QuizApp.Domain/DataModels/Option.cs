namespace QuizApp.Domain.DataModels;

public class Option
{
    public int Id { get; set; }

    public int QuestionId { get; set; }
    public  Question Question { get; set; } = null!;
    public string Text { get; set; } = null!;

    public bool Iscorrect { get; set; }

    public DateTime? Createdat { get; set; }
}
