using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quiz.Domain.Dto;
using QuizApp.Domain.Dto;
using QuizApp.Services.Interface;

namespace QuizApp.Web.Controllers;

public class QuestionController : Controller
{
    private readonly IQuestionServices _questionService;
    private readonly ILogger<QuestionController> _logger;

    public QuestionController(IQuestionServices questionService, ILogger<QuestionController> logger)
    {
        _questionService = questionService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<ResponseDto>> GetTotalQuestions()
    {
        try
        {
            int totalQuestions = await _questionService.GetTotalQuestionsAsync();
            return Ok(new { success = true, data = totalQuestions });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching total questions.");
            return StatusCode(500, new { success = false, message = "An internal server error occurred." });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetAllQuestions()
    {
        try
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(new { success = true, data = questions });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching questions.");
            return StatusCode(500, new { success = false, message = "An internal server error occurred." });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ResponseDto>> CreateQuestion([FromBody] QuestionCreateDto dto)
    {

        try
        {
            var validationResult = await _questionService.ValidateQuestionAsync(dto);
            if (!validationResult.IsValid)
                return new ResponseDto(false, validationResult.ErrorMessage, null, 400);

            var createdQuestion = await _questionService.CreateQuestionAsync(dto);
            return new ResponseDto(true, "Question created successfully.", createdQuestion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating question.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ResponseDto>> GetQuestionForEdit(int id)
    {
        try
        {
            if (id <= 0)
                return new ResponseDto(false, "Invalid Question ID.", null, 400);

            var question = await _questionService.GetQuestionForEditAsync(id);
            return question != null
                ? new ResponseDto(true, "Question fetched successfully.", question)
                : new ResponseDto(false, "Question not found.", null, 404);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching question for edit.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ResponseDto>> EditQuestion([FromBody] QuestionUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return new ResponseDto(false, "Invalid data provided.", null, 400);
        try
        {
            var validationResult = await _questionService.ValidateQuestionUpdate(dto);
            if (!validationResult.IsValid)
                return new ResponseDto(false, validationResult.ErrorMessage, null, 400);

            var result = await _questionService.EditQuestionAsync(dto);
            return result != null ? new ResponseDto(true, "Question updated successfully.", result)
                                  : new ResponseDto(false, "Failed to update question.", null, 404);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update question.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }



    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ResponseDto>> SoftDeleteQuestion(int id)
    {
        try
        {
            var validationResult = await _questionService.validateDeleteQuestionAsync(id);
            if (!validationResult.IsValid)
                return new ResponseDto(false, validationResult.ErrorMessage, null, 400);

            bool result = await _questionService.SoftDeleteQuestionAsync(id);
            return result ? new ResponseDto(true, "Question deleted successfully.")
                          : new ResponseDto(false, "Failed to delete question or question not found.", null, 404);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete question.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }



    [HttpGet("category/{categoryId}/random/{count}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<ResponseDto>> GetRandomQuestions(int categoryId, int count)
    {
        try
        {
            var validationResult = await _questionService.ValidateGetRandomQuestionsAsync(categoryId, count);
            if (!validationResult.IsValid)
                return new ResponseDto(false, validationResult.ErrorMessage, null, 400);

            var questions = await _questionService.GetRandomQuestionsAsync(categoryId, count);
            return questions != null
                ? new ResponseDto(true, "Random questions fetched successfully.", questions)
                : new ResponseDto(false, "No questions found for the specified category.", null, 404);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching random questions.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }

    [HttpGet("quiz/{quizId}/random/{count}")]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<ResponseDto>> GetRandomQuestionByQuizId(int quizId, int count)
    {
        try
        {
            var validationResult = await _questionService.ValidateGetRandomQuestionsByQuizIdAsync(quizId, count);
            if (!validationResult.IsValid)
                return new ResponseDto(false, validationResult.ErrorMessage, null, 400);

            var questions = await _questionService.GetRandomQuestionsByQuizIdAsync(quizId, count);
            return questions != null
                ? new ResponseDto(true, "Random questions by quiz ID fetched successfully.", questions)
                : new ResponseDto(false, "No questions found for the specified quiz ID.", null, 404);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching random questions.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<ResponseDto>> GetRandomQuestionByQuizIds(int quizId)
    {
        try
        {
            var questions = await _questionService.GetRandomQuestionsByQuizIdAsync(quizId);
            return questions != null
                ? new ResponseDto(true, "Random questions by quiz ID fetched successfully.", questions)
                : new ResponseDto(false, "No questions found for the specified quiz ID.", null, 404);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching random questions.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }
}
