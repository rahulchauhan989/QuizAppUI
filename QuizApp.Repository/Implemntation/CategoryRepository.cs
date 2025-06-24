using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.DataContext;
using QuizApp.Domain.DataModels;
using QuizApp.Repository.Interface;

namespace QuizApp.Repository.Implemntation;

public class CategoryRepository : ICategoryRepository
{
    private readonly QuizAppContext _context;

    public CategoryRepository(QuizAppContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories
        .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task CreateCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return false;


        category.Isdeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckDuplicateCategoryAsync(string name)
    {
        return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Isdeleted == false);
    }

    public async Task<IEnumerable<Quiz>> GetQuizzesByCategoryIdAsync(int categoryId)
    {
        return await _context.Quizzes
            .Where(q => q.CategoryId == categoryId && q.Isdeleted != true)
            .ToListAsync();
    }

}
