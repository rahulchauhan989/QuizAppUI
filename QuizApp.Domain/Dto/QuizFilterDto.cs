using System.ComponentModel.DataAnnotations;

namespace QuizApp.Domain.Dto;

public class QuizFilterDto
{
    public int? CategoryId { get; set; }
    
    [MaxLength(100, ErrorMessage = "Title keyword cannot exceed 100 characters.")]
    public string? TitleKeyword { get; set; }
    public bool? IsPublic { get; set; }
}

