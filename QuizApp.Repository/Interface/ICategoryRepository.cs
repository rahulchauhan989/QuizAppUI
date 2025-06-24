using QuizApp.Domain.DataModels;

namespace QuizApp.Repository.Interface;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int id);
    Task<bool> CheckDuplicateCategoryAsync(string name);
    Task<IEnumerable<Quiz>> GetQuizzesByCategoryIdAsync(int categoryId);
}
