using System.ComponentModel.DataAnnotations;

namespace QuizApp.Domain.Dto;
public class CategoryCreateDto
{
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
    public string? Description { get; set; }
}