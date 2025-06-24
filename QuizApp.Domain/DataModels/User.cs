namespace QuizApp.Domain.DataModels;

public class User
{
    public int UserId { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string? Role { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Createdat { get; set; }

    // public required ICollection<Category> Categories { get; set; } 

    // public required ICollection<Question> Questions { get; set; }

    // public required ICollection<Quiz> Quizzes { get; set; }

    // public required ICollection<Userquizattempt> Userquizattempts { get; set; }
}
