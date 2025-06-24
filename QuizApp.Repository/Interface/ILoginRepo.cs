using QuizApp.Domain.DataModels;

namespace QuizApp.Repository.Interface;

public interface ILoginRepo
{
     Task<bool> ValidateUserAsync(string email, string password);

    Task<User> GetUserByEmailAsync(string email);

    Task<string> RegisterUserAsync(User user);

    Task<User?> GetUserByIdAsync(int userId);
}
