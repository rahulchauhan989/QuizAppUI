using QuizApp.Domain.DataModels;
using QuizApp.Domain.Dto;

namespace QuizApp.Repository.Interface;

public interface IUserQuizAttemptRepository
{
    Task<Userquizattempt?> GetAttemptByUserAndQuizAsync(int userId, int quizId, int categoryId);
    Task<int> CreateAttemptAsync(Userquizattempt attempt);

    Task UpdateAttemptAsync(Userquizattempt attempt);

    Task<IEnumerable<ActiveQuiz>> GetActiveQuizzesAsync();
    Task<Userquizattempt> GetAttemptByIdAsync(int attemptId);
    Task<Userquizattempt?> GetAttemptByUserAndQuizAsync(int userId, int quizId);
    Task<bool> IsUserExistAsync(int userId);

    Task<IEnumerable<Userquizattempt>> GetAttemptsByCategoryIdAsync(int categoryId);
}
