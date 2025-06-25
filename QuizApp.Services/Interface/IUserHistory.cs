using QuizApp.Domain.DataModels;
using QuizApp.Domain.Dto;

namespace QuizApp.Services.Interface;

public interface IUserHistory
{
    Task<List<UserQuizHistoryDto>> GetUserQuizHistoryAsync(int userId);
    Task<bool> isUserExistAsync(int userId);
    Task<ValidationResult> ValidateUserIdAsync(int userId);

    Task<IEnumerable<User>> GetAllUser();
}
