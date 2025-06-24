using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.DataContext;
using QuizApp.Domain.DataModels;
using QuizApp.Domain.Dto;
using QuizApp.Repository.Interface;

namespace QuizApp.Repository.Implemntation;

public class UserQuizAttemptRepository : IUserQuizAttemptRepository
{
    private readonly QuizAppContext _context;

    public UserQuizAttemptRepository(QuizAppContext context)
    {
        _context = context;
    }

    public async Task<Userquizattempt?> GetAttemptByUserAndQuizAsync(int userId, int quizId, int categoryId)
    {
        return await _context.Userquizattempts
            .FirstOrDefaultAsync(a => a.UserId == userId && a.QuizId == quizId);
    }

    public async Task<int> CreateAttemptAsync(Userquizattempt attempt)
    {
        if (attempt.Startend.HasValue)
            attempt.Startend = DateTime.SpecifyKind(attempt.Startend.Value, DateTimeKind.Unspecified);

        if (attempt.EndAt.HasValue)
            attempt.EndAt = DateTime.SpecifyKind(attempt.EndAt.Value, DateTimeKind.Unspecified);

        _context.Userquizattempts.Add(attempt);
        await _context.SaveChangesAsync();
        return attempt.Id;
    }

    public async Task UpdateAttemptAsync(Userquizattempt attempt)
    {
        if (attempt.Startend.HasValue)
            attempt.Startend = DateTime.SpecifyKind(attempt.Startend.Value, DateTimeKind.Unspecified);

        if (attempt.EndAt.HasValue)
            attempt.EndAt = DateTime.SpecifyKind(attempt.EndAt.Value, DateTimeKind.Unspecified);

        _context.Userquizattempts.Update(attempt);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ActiveQuiz>> GetActiveQuizzesAsync()
    {
        var attempts = await _context.Userquizattempts
            .Where(a => a.Issubmitted == false && a.Startend.HasValue)
            .Include(a => a.Quiz)
            .ToListAsync();

        return attempts.Select(a => new ActiveQuiz
        {
            AttemptId = a.Id,
            StartedAt = a.Startend!.Value,
            DurationMinutes = a.Quiz?.Durationminutes
        }).ToList();
    }

    public async Task<Userquizattempt?> GetAttemptByIdAsync(int attemptId)
    {
        return await _context.Userquizattempts
            .Include(a => a.Useranswers)
            .FirstOrDefaultAsync(a => a.Id == attemptId);
    }

    public async Task<Userquizattempt?> GetAttemptByUserAndQuizAsync(int userId, int quizId)
    {
        return await _context.Userquizattempts
            .FirstOrDefaultAsync(a => a.UserId == userId && a.QuizId == quizId);
    }

    public async Task<bool> IsUserExistAsync(int userId)
    {
        return await _context.Users.AnyAsync(u => u.UserId == userId);
    }

    public async Task<IEnumerable<Userquizattempt>> GetAttemptsByCategoryIdAsync(int categoryId)
    {
        return await _context.Userquizattempts
            .Where(uqa => uqa.CategoryId == categoryId)
            .ToListAsync();
    }
}
