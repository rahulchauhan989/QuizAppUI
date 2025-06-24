using QuizApp.Domain.Dto;

namespace QuizApp.Services.Interface;

public interface ILoginService
{
       Task<bool> ValidateUserAsync(string email, string password);

    Task<string> GenerateToken(LoginModel request);

    Task<ValidationResult> ValidateLoginAsync(LoginModel request);

    Task<String> RegisterUserAsync(RegistrationDto request);

    int ExtractUserIdFromToken(string token);

    Task<UserProfileViewDto?> GetCurrentUserProfileAsync(string token);
}
