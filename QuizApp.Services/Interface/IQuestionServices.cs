using quiz.Domain.Dto;
using QuizApp.Domain.Dto;

namespace QuizApp.Services.Interface;

public interface IQuestionServices
{
    Task<ValidationResult> ValidateQuestionAsync(QuestionCreateDto dto);

    Task<ValidationResult> ValidateQuestionUpdate(QuestionUpdateDto dto);
    Task<int> GetTotalQuestionsAsync();
    Task<List<QuestionDto>> GetAllQuestionsAsync();
    Task<QuestionDto> CreateQuestionAsync(QuestionCreateDto dto);
    Task<bool> IsCategoryExistsAsync(int Categoryid);
    Task<int> GetQuestionCountByCategoryAsync(int Categoryid);
    Task<List<QuestionDto>> GetRandomQuestionsAsync(int Categoryid, int count);
    Task<int> GetQuestionCountByQuizIdAsync(int quizId);
    Task<List<QuestionDto>> GetRandomQuestionsByQuizIdAsync(int quizId, int count);
    Task<List<QuestionDto>> GetRandomQuestionsByQuizIdAsync(int quizId);
    Task<bool> IsQuizExistsAsync(int quizId);
    Task<QuestionEditDto?> GetQuestionForEditAsync(int questionId);
    Task<QuestionDto?> EditQuestionAsync(QuestionUpdateDto dto);
    Task<bool> SoftDeleteQuestionAsync(int id);
    Task<ValidationResult> ValidateGetRandomQuestionsAsync(int categoryId, int count);
    Task<ValidationResult> ValidateGetRandomQuestionsByQuizIdAsync(int quizId, int count);
    Task<ValidationResult> validateDeleteQuestionAsync(int questionId);
}
