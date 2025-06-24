using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Domain.DataModels;

public class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Createdat { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }
    public User Creator { get; set; } = null!;

    public int? Updatedby { get; set; }
    public User? Updater { get; set; }
    public  ICollection<Question> Questions { get; set; }  = new List<Question>();
    public  ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    // public ICollection<Userquizattempt> Userquizattempts { get; set; } = new List<Userquizattempt>();
    
}
