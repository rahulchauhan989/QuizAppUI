using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.DataContext;
using QuizApp.Domain.DataModels;
using QuizApp.Domain.Dto;
using QuizApp.Repository.Interface;

namespace QuizApp.Repository.Implemntation;

public class QuizRepository : IQuizRepository
{

    private readonly QuizAppContext _context;

    public QuizRepository(QuizAppContext context)
    {
        _context = context;
    }

    public async Task<Quiz> CreateQuizAsync(Quiz quiz)
    {
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return quiz;
    }

    public async Task<int> GetTotalQuizzesAsync()
    {
        return await _context.Quizzes.CountAsync(q => q.Isdeleted == false);
    }

    public async Task<int> GetPublishedQuizzesAsync()
    {
        return await _context.Quizzes.CountAsync(q => q.Isdeleted == false && q.Ispublic == true);
    }
    public async Task<List<QuizListDto>> GetAllQuizzesAsync()
    {
        var quizzes = await (from quiz in _context.Quizzes
                             where quiz.Isdeleted==false
                             join category in _context.Categories on quiz.CategoryId equals category.CategoryId
                             join quizQuestion in _context.Quizquestions on quiz.Id equals quizQuestion.QuizId into quizQuestionsGroup
                             select new QuizListDto
                             {
                                 Id = quiz.Id,
                                 Title = quiz.Title,
                                 CategoryName = category.Name,
                                 TotalQuestions = quizQuestionsGroup.Count(),
                                 DurationMinutes = quiz.Durationminutes ?? 0,
                                 IsPublic = quiz.Ispublic ?? false
                             }).ToListAsync();

        return quizzes;
    }

    public async Task LinkExistingQuestionToQuizAsync(int quizId, int questionId)
    {
        Quizquestion quizquestion = new Quizquestion
        {
            QuizId = quizId,
            QuestionId = questionId
        };

        await _context.Quizquestions.AddAsync(quizquestion);
        await _context.SaveChangesAsync();
    }

    public async Task AddQuestionToQuizAsync(Quiz quiz, Question question)
    {
        if (question.QuestionId == 0)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        Quizquestion quizquestion = new Quizquestion
        {
            QuizId = quiz.Id,
            QuestionId = question.QuestionId
        };

        await _context.Quizquestions.AddAsync(quizquestion);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Question>> GetRandomQuestionsAsync(int Categoryid, int count)
    {
        return await _context.Questions
            .Where(q => q.CategoryId == Categoryid && q.Isdeleted == false)
            .Include(q => q.Options)
            .OrderBy(q => EF.Functions.Random())
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetRandomQuestionsByQuizIdAsync(int quizId, int count)
    {
        return await _context.Questions
            .Where(q => _context.Quizquestions
                .Where(qq => qq.QuizId == quizId)
                .Select(qq => qq.QuestionId)
                .Contains(q.QuestionId))
            .Include(q => q.Options)
            .OrderBy(q => EF.Functions.Random())
            .Take(count)
            .ToListAsync();
    }

    public async Task UpdateQuizAsync(Quiz quiz)
    {
        _context.Quizzes.Update(quiz);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteQuizAsync(int id)
    {
        var quiz = await _context.Quizzes.FindAsync(id);
        if (quiz != null)
        {
            quiz.Isdeleted = true;
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetQuestionCountByQuizIdAsync(int quizId)
    {
        return await _context.Quizquestions
            .Where(qq => qq.QuizId == quizId)
            .CountAsync();
    }

    public async Task<bool> IsQuizExistsAsync(int quizId)
    {
        return await _context.Quizzes.AnyAsync(q => q.Id == quizId);
    }

    public async Task<int> GetTotalQuestionsAsync()
    {
        return await _context.Questions.CountAsync(q => q.Isdeleted == false);
    }

    public async Task<Question> CreateQuestionAsync(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return question;
    }
    public async Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(int quizId)
    {
        return await _context.Questions
            .Where(q => _context.Quizquestions
                .Where(qq => qq.QuizId == quizId)
                .Select(qq => qq.QuestionId)
                .Contains(q.QuestionId))
            .Include(q => q.Options)
            .ToListAsync();
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _context.Questions
            .Include(q => q.category)
            .Where(q => q.Isdeleted == false)
            .ToListAsync();
    }

    public async Task<int> GetQuestionCountByCategoryAsync(int Categoryid)
    {
        return await _context.Questions
            .CountAsync(q => q.CategoryId == Categoryid);
    }

    public async Task<bool> IsCategoryExistsAsync(int Categoryid)
    {
        return await _context.Categories
            .AnyAsync(c => c.CategoryId == Categoryid);
    }

    public async Task<bool> IsQuizTitleExistsAsync(string title)
    {
        return await _context.Quizzes
            .AnyAsync(q => q.Title == title);
    }

    public async Task<IEnumerable<QuizListDto>> GetFilteredQuizzesAsync(QuizFilterDto filter)
    {
        var query = _context.Quizzes
            .Where(q => !q.Isdeleted!.Value)
            .Include(q => q.Category)
            .AsQueryable();

        if (filter.CategoryId.HasValue)
            query = query.Where(q => q.CategoryId == filter.CategoryId.Value);

        if (!string.IsNullOrEmpty(filter.TitleKeyword))
            query = query.Where(q => q.Title.ToLower().Trim().Contains(filter.TitleKeyword.ToLower().Trim()));

        if (filter.IsPublic.HasValue)
            query = query.Where(q => q.Ispublic == filter.IsPublic);

        return await query.Select(q => new QuizListDto
        {
            Id = q.Id,
            Title = q.Title,
            Description = q.Description,
            TotalMarks = q.Totalmarks,
            DurationMinutes = q.Durationminutes,
            IsPublic = q.Ispublic,
            CategoryName = q.Category.Name
        }).ToListAsync();
    }

    public async Task<List<CorrectAnswerDto>> GetCorrectAnswersForQuizAsync(int categoryid)
    {
        return await (from question in _context.Questions
                      join option in _context.Options
                      on question.QuestionId equals option.QuestionId
                      where question.CategoryId == categoryid && option.Iscorrect
                      select new CorrectAnswerDto
                      {
                          QuestionId = question.QuestionId,
                          CorrectOptionId = option.Id,
                          Marks = (int)question.Marks!
                      }).ToListAsync();
    }

    public async Task<int> GetTotalMarksByQuizIdAsync(SubmitQuizRequest request)
    {
        return await _context.Quizzes
            .Where(q => q.Id == request.QuizId)
            .Select(q => q.Totalmarks).FirstOrDefaultAsync();
    }

    public async Task<int> GetQuetionsMarkByIdAsync(int questionId)
    {
        int marks = await _context.Questions
            .Where(q => q.QuestionId == questionId)
            .Select(q => q.Marks).FirstOrDefaultAsync() ?? 0;

        return marks;
    }

    public async Task<Quiz> GetQuizByIdAsync(int quizId)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.Category)
            .FirstOrDefaultAsync(q => q.Id == quizId && q.Isdeleted == false);

        // if (quiz == null)
        // {
        //     throw new InvalidOperationException($"Quiz with ID {quizId} not found.");
        // }

        return quiz!;
    }

    public async Task<IEnumerable<Question>> GetQuestionsByIdsAsync(List<int> questionIds)
    {
        return await _context.Questions
            .Where(q => questionIds.Contains(q.QuestionId))
            .Include(q => q.Options)
            .ToListAsync();
    }

    public async Task AddQuestionToQuiz(Quiz quiz, Question question)
    {
        var quizQuestion = new Quizquestion
        {
            QuizId = quiz.Id,
            QuestionId = question.QuestionId,

        };

        await _context.Database.ExecuteSqlRawAsync(
          "INSERT INTO QuizQuestions (QuizId, QuestionId) VALUES ({0}, {1})",
          quizQuestion.QuizId, quizQuestion.QuestionId
      );
        await _context.SaveChangesAsync();
    }

    public async Task<List<Userquizattempt>> GetUserQuizAttemptsAsync(int userId)
    {
        return await _context.Userquizattempts
            .Where(attempt => attempt.UserId == userId)
            .Include(attempt => attempt.Quiz)
            .Include(attempt => attempt.Category)
            .Include(attempt => attempt.Useranswers)
                .ThenInclude(answer => answer.Question)
            .Include(attempt => attempt.Useranswers)
                .ThenInclude(answer => answer.Option)
            .ToListAsync();
    }

    public async Task<int> GetQuizQuestionsMarksSumAsync(int quizId)
    {
        return (int)await _context.Quizquestions
            .Where(qq => qq.QuizId == quizId)
            .Join(_context.Questions, qq => qq.QuestionId, q => q.QuestionId, (qq, q) => q.Marks)
            .SumAsync();
    }

    public async Task<bool> RemoveQuestionFromQuizAsync(int quizId, int questionId)
    {
        var quizQuestion = await _context.Quizquestions
            .FirstOrDefaultAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId);

        if (quizQuestion == null)
        {
            return false;
        }

        _context.Quizquestions.Remove(quizQuestion);

        var affected = await _context.SaveChangesAsync();

        return affected > 0;
    }

    public async Task<Question?> GetQuestionByIdAsync(int questionId)
    {
        return await _context.Questions
            .Include(q => q.Options)
            .Include(q => q.category)
            .FirstOrDefaultAsync(q => q.QuestionId == questionId);
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        _context.Questions.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveOptionsByQuestionIdAsync(int questionId)
    {
        var options = _context.Options.Where(o => o.QuestionId == questionId);
        _context.Options.RemoveRange(options);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsQuestionUsedInAnswersAsync(int questionId)
    {
        return await _context.Useranswers.AnyAsync(ua => ua.QuestionId == questionId);
    }

    public async Task<bool> IsQuestionInPublicQuizAsync(int questionId)
    {
        return await _context.Quizquestions
            .Where(q => q.QuestionId == questionId)
            .AnyAsync(q => q.Quiz.Ispublic == true);
    }

    public async Task<bool> IsQuestionInQuizAsync(int quizId, int questionId)
    {
        return await _context.Quizquestions.AnyAsync(q => q.QuizId == quizId && q.QuestionId == questionId);
    }

    public async Task<bool> HasUnsubmittedAttemptsAsync(int quizId)
    {
        return await _context.Userquizattempts.AnyAsync(uqa => uqa.QuizId == quizId && uqa.Issubmitted == false);
    }
}
