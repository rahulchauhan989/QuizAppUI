using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Domain.DataModels;

public class Question
{
    public int QuestionId { get; set; }

    public string Text { get; set; } = null!;

    public int? Marks { get; set; }

    public string? Difficulty { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public int Createdby { get; set; }
    public User Creator { get; set; } = null!;

    public int? Updatedby { get; set; }
    public User? Updater { get; set; }
    public int CategoryId { get; set; }
    public  Category category { get; set; } = null!;

    public  ICollection<Option> Options { get; set; } = new List<Option>();

}
