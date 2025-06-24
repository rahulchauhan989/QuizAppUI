using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Domain.DataModels;

public class Quiz
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Totalmarks { get; set; }

    public int? Durationminutes { get; set; }

    public bool? Ispublic { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public bool? Isdeleted { get; set; }
    public DateTime? Modifiedat { get; set; }

    public int Createdby { get; set; }

    public User Creator { get; set; } = null!;

    public int? Updatedby { get; set; }
    public User? Updater { get; set; }    

    // public ICollection<Userquizattempt> Userquizattempts { get; set; } = new List<Userquizattempt>();
}
