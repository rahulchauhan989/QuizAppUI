$(document).ready(function () {
    fetchPublishedQuizzes();
});

let currentQuizId = null;
var currentCategoryId=0;

function fetchPublishedQuizzes() {
    debugger
    $.ajax({
        url: '/Quiz/GetPublishedQuizze', 
        type: 'GET',
        success: function (response) {
            if (response.isSuccess) {
                populateQuizList(response.data);
            } else {
                toastr.warning(response.message || "No published quizzes found.");
            }
        },
        error: function (error) {
            console.error("Error fetching published quizzes:", error);
            toastr.error("An error occurred while fetching published quizzes.");
        }
    });
}

function populateQuizList(quizzes) {
    debugger
    const quizList = $('#quizList');
    quizList.empty();

    quizzes.forEach(quiz => {
        const quizCard = `
            <div class="col-md-4 mb-4">
                <div class="card shadow">
                    <div class="card-body">
                        <h5 class="card-title">${quiz.title}</h5>
                        <p class="card-text">${quiz.description}</p>
                        <button class="btn btn-primary" onclick="startQuiz(${quiz.id},${quiz.categoryid})">Start Quiz</button>
                    </div>
                </div>
            </div>
        `;
        quizList.append(quizCard);
    });
}

function startQuiz(quizId,categoryId) {
    debugger
    currentQuizId = quizId;

    $.ajax({
        url: '/QuizSubmission/StartQuiz',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ QuizId: quizId, UserId: 2, categoryId: categoryId }), 
        success: function (response) {
            if (response.isSuccess) {
                currentCategoryId = response.data.quiz.categoryid;
                populateQuizQuestions(response.data.questions,response.data.quiz.categoryid);
                $('#quizTitle').text(response.data.quiz.title);
                $('#quizQuestionsModal').modal('show');
            } else {
                toastr.warning(response.message || "Failed to start quiz.");
            }
        },
        error: function (error) {
            console.error("Error starting quiz:", error);
            toastr.error("An error occurred while starting the quiz.");
        }
    });
}

function populateQuizQuestions(questions,categoryId) {
    debugger
    const questionsContainer = $('#questionsContainer');
    questionsContainer.empty();

    questions.forEach((question, index) => {
        const questionHtml = `
            <div class="mb-4">
                <h5>${index + 1}. ${question.text}</h5>
                ${question.options.map(option => `
                    <div class="form-check">
                        <input class="form-check-input" type="radio" name="question_${question.id}_${categoryId}" value="${option.id}" id="option_${option.id}">
                        <label class="form-check-label" for="option_${option.id}">
                            ${option.text}
                        </label>
                    </div>
                `).join('')}
            </div>
        `;
        questionsContainer.append(questionHtml);
    });
}

function submitQuiz() {
    debugger
    const answers = [];

    $('#questionsContainer').find('input[type="radio"]:checked').each(function () {
        const questionId = $(this).attr('name').split('_')[1];
        const categoryId =$(this).attr('name').split('_')[2]
        const optionId = $(this).val();
        answers.push({ QuestionId: parseInt(questionId), OptionId: parseInt(optionId) });
    });

    const payload = {
        QuizId: currentQuizId,
        UserId: 2, 
        categoryId : currentCategoryId,
        Answers: answers
    };

    $.ajax({
        url: '/QuizSubmission/SubmitQuiz',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (response) {
            if (response.isSuccess) {
                toastr.success(response.message || "Quiz submitted successfully.");
                $('#quizQuestionsModal').modal('hide');
            } else {
                toastr.warning(response.message || "Failed to submit quiz.");
            }
        },
        error: function (error) {
            console.error("Error submitting quiz:", error);
            toastr.error("An error occurred while submitting the quiz.");
        }
    });
}