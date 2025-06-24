using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.DataContext;
using QuizApp.Domain.DataModels;
using QuizApp.Repository.Interface;

namespace QuizApp.Repository.Implemntation;

public class UserAnswerRepository:IUserAnswerRepository
{
    private readonly QuizAppContext _context;

    public UserAnswerRepository(QuizAppContext context)
    {
        _context = context;
    }

    public async Task SaveAnswerAsync(Useranswer answer)
    {
        _context.Useranswers.Add(answer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> isUserExist(int userId)
    {
        bool userExist = await _context.Users.AnyAsync(u => u.UserId == userId);
        return userExist;
    }
}
