namespace QuizApp.Domain.DataModels;

public class Userquizattempt
{
    public int Id { get; set; } 

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;

    public DateTime? Startend { get; set; }
    public DateTime? EndAt { get; set; }

    public int Timespent { get; set; }

    public int Score { get; set; }
    public bool Issubmitted { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<Useranswer> Useranswers { get; set; } = new List<Useranswer>();
}