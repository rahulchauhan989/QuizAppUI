let currentTab = "overview";
document.addEventListener("DOMContentLoaded", () => {
  showTab("overview");
  setupEventListeners();
  fetchTotalCategories();
});

function setupEventListeners() {
  // Search 
  document
    .getElementById("categorySearch")
    ?.addEventListener("input", filterCategories);
  document
    .getElementById("questionSearch")
    ?.addEventListener("input", filterQuestions);
  document
    .getElementById("quizSearch")
    ?.addEventListener("input", filterQuizzes);
}

function showTab(tabName) {
  document.querySelectorAll(".tab-content").forEach((tab) => {
    tab.classList.remove("active");
  });

  document.getElementById(tabName + "-tab").classList.add("active");

  document.querySelectorAll(".sidebar .nav-link").forEach((link) => {
    link.classList.remove("active");
  });

  document
    .querySelector(`.sidebar .nav-link[href="#${tabName}-tab"]`)
    .classList.add("active");
  currentTab = tabName;
}

async function fetchTotalCategories() {
  debugger
  try {
    const response = await fetch('/Category/GetAllCategories');
    if (!response.ok) {
      throw new Error('Failed to fetch categories');
    }
    const data = await response.json();
    if (data.success) {
      const totalCategories = data.data.length;
      document.getElementById('totalCategories').textContent = totalCategories;
    } else {
      document.getElementById('totalCategories').textContent = '0';
    }
  } catch (error) {
    console.error('Error fetching categories:', error);
    document.getElementById('totalCategories').textContent = 'Error';
  }
}


$(document).ready(function () {
  var categoryIdd = 0;
  fetchTotalCategories();
  fetchTotalQuestions();
  fetchTotalQuizzes();
  fetchPublishedQuizzes();
  fetchQuestions();
  fetchQuizzes();

  $('#questionModal').on('show.bs.modal', function () {
    populateCategoryDropdown();
  });
});


function fetchTotalCategories() {
  $.ajax({
    url: '/Category/GetAllCategories',
    type: 'GET',
    success: function (response) {
      if (response.success) {
        $('#totalCategories').text(response.data.length);
        populateCategoriesTable(response.data);
      } else {
        $('#totalCategories').text('02');
        toastr.warning(response.message || "No categories found.");
      }
    },
    error: function (error) {
      console.error("Error fetching categories:", error);
      $('#totalCategories').text('Error');
      toastr.error("An error occurred while fetching categories.");
    }
  });
}

function fetchTotalQuestions() {
  debugger
  $.ajax({
    url: '/Question/GetTotalQuestions',
    type: 'GET',
    success: function (response) {
      if (response.success) {
        $('#totalQuestions').text(response.data);
      } else {
        $('#totalQuestions').text('0');
        toastr.warning(response.message || "No questions found.");
      }
    },
    error: function (error) {
      console.error("Error fetching questions:", error);
      $('#totalQuestions').text('Error');
      toastr.error("An error occurred while fetching questions.");
    }
  });
}


function fetchTotalQuizzes() {
  $.ajax({
    url: '/Quiz/GetTotalQuizzes',
    type: 'GET',
    success: function (response) {
      if (response.success) {
        $('#totalQuizzes').text(response.data);
      } else {
        $('#totalQuizzes').text('0');
        toastr.warning(response.message || "No quizzes found.");
      }
    },
    error: function (error) {
      console.error("Error fetching total quizzes:", error);
      $('#totalQuizzes').text('Error');
      toastr.error("An error occurred while fetching total quizzes.");
    }
  });
}

function fetchPublishedQuizzes() {
  $.ajax({
    url: '/Quiz/GetPublishedQuizzes',
    type: 'GET',
    success: function (response) {
      if (response.success) {
        $('#publishedQuizzes').text(response.data);
      } else {
        $('#publishedQuizzes').text('0');
        toastr.warning(response.message || "No published quizzes found.");
      }
    },
    error: function (error) {
      console.error("Error fetching published quizzes:", error);
      $('#publishedQuizzes').text('Error');
      toastr.error("An error occurred while fetching published quizzes.");
    }
  });
}

function populateCategoriesTable(categories) {
  const tableBody = $('#categoriesTableBody');
  tableBody.empty();

  categories.forEach(category => {

    const row = `
          <tr>
              <td class="font-weight-bold">${category.name}</td>
              <td>${category.description}</td>
              <td>
                  <button class="btn btn-sm me-1" data-bs-toggle="modal" data-bs-target="#EditcategoryModal" onclick="openCategoryModal('edit', ${category.id})">
                      <i class="fas fa-edit"></i>
                  </button>
                  <button class="btn btn-sm" data-bs-toggle="modal" data-bs-target="#deleteCategory" onclick="deleteCategory(${category.id})">
                      <i class="fas fa-trash"></i>
                  </button>
              </td>
          </tr>
      `;
    tableBody.append(row);
  });
}

function saveCategory() {
  const categoryName = $('#categoryName').val().trim();
  const categoryDescription = $('#categoryDescription').val().trim();

  if (!categoryName || !categoryDescription) {
    toastr.warning("Please fill in all fields.");
    return;
  }

  const categoryData = {
    name: categoryName,
    description: categoryDescription
  };

  $.ajax({
    url: '/Category/CreateCategory',
    type: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(categoryData),
    success: function (response) {
      if (response.isSuccess) {
        toastr.success(response.message || "Category added successfully.");
        $('#categoryModal').modal('hide');
        fetchTotalCategories();
      } else {
        toastr.error(response.message || "Failed to add category.");
      }
    },
    error: function (error) {
      console.error("Error adding category:", error);
      toastr.error("An error occurred while adding the category.");
    }
  });
}


function openCategoryModal(action, categoryId) {
  debugger
  if (action === 'edit') {
    $.ajax({
      url: `/Category/GetCategoryById?id=${categoryId}`,
      type: 'GET',
      success: function (response) {
        if (response.success) {
          $('#EditcategoryName').val(response.data.name);
          $('#EditcategoryDescription').val(response.data.description);

          $('#saveCategoryForEdit').off('click').on('click', function () {
            updateCategory(categoryId);
          });

          $('#EditcategoryModal').modal('show');
        } else {
          toastr.warning(response.message || "Failed to fetch category details.");
        }
      },
      error: function (error) {
        console.error("Error fetching category details:", error);
        toastr.error("An error occurred while fetching category details.");
      }
    });
  }
}

function updateCategory(categoryId) {
  debugger
  const categoryName = $('#EditcategoryName').val().trim();
  const categoryDescription = $('#EditcategoryDescription').val().trim();

  if (!categoryName || !categoryDescription) {
    toastr.warning("Please fill in all fields.");
    return;
  }

  const categoryData = {
    name: categoryName,
    description: categoryDescription
  };

  $.ajax({
    url: `/Category/UpdateCategory?id=${categoryId}`,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(categoryData),
    success: function (response) {
      if (response.success) {
        toastr.success(response.message || "Category updated successfully.");
        $('#EditcategoryModal').modal('hide');
        fetchTotalCategories();
      } else {
        toastr.error(response.message || "Failed to update category.");
      }
    },
    error: function (error) {
      console.error("Error updating category:", error);
      toastr.error("An error occurred while updating the category.");
    }
  });
}


let categoryIdToDelete = 0;

function deleteCategory(categoryId) {
  categoryIdToDelete = categoryId;
  $('#deleteCategory').modal('show');
}

$(document).on('click', '#confirmDeleteCategoryBtn', function () {
  if (categoryIdToDelete > 0) {
    $.ajax({
      url: `/Category/DeleteCategory?id=${categoryIdToDelete}`,
      type: 'DELETE',
      success: function (response) {
        if (response.success) {
          toastr.success(response.message || "Category deleted successfully.");
          fetchTotalCategories();
        } else {
          toastr.error(response.message || "Failed to delete category.");
        }
      },
      error: function (error) {
        console.error("Error deleting category:", error);
        toastr.error("An error occurred while deleting the category.");
      }
    });
  }
});


function fetchQuestions() {
  $.ajax({
    url: '/Question/GetAllQuestions',
    type: 'GET',
    success: function (response) {
      if (response.success) {
        populateQuestionsTable(response.data);
      } else {
        toastr.warning(response.message || "No questions found.");
      }
    },
    error: function (error) {
      console.error("Error fetching questions:", error);
      toastr.error("An error occurred while fetching questions.");
    }
  });
}

function populateQuestionsTable(questions) {
  const tableBody = $('#questionsTableBody');
  tableBody.empty();

  questions.forEach(question => {
    const row = `
          <tr>
              <td class="font-weight-bold">${question.text}</td>
              <td>${question.categoryName}</td>
              <td><span class="badge bg-info">${question.difficulty}</span></td>
              <td>${question.marks}</td>
              <td>
                  <button class="btn btn-sm me-1" onclick="openEditQuestionModal( ${question.id})">
                      <i class="fas fa-edit"></i>
                  </button>
                  <button class="btn btn-sm" onclick="deleteQuestion(${question.id})">
                      <i class="fas fa-trash"></i>
                  </button>
              </td>
          </tr>
      `;
    tableBody.append(row);
  });
}


function populateCategoryDropdown() {
  $.ajax({
    url: '/Category/GetAllCategories',
    type: 'GET',
    success: function (response) {
      if (response.success) {
        const categoryDropdown = $('#questionCategory');
        categoryDropdown.empty();
        categoryDropdown.append('<option value="">Select Category</option>');

        response.data.forEach(category => {
          categoryDropdown.append(`<option value="${category.id}">${category.name}</option>`);
        });
      } else {
        toastr.warning(response.message || "Failed to fetch categories.");
      }
    },
    error: function (error) {
      console.error("Error fetching categories:", error);
      toastr.error("An error occurred while fetching categories.");
    }
  });
}

function saveQuestion() {
  const questionText = $('#questionText').val().trim();
  const questionCategory = $('#questionCategory').val();
  const questionDifficulty = $('#questionDifficulty').val();
  const questionMarks = $('#questionMarks').val();
  const options = [];

  $('.option-group .input-group').each(function (index) {
    const optionText = $(this).find('input[type="text"]').val().trim();
    const isCorrect = $(this).find('input[type="radio"]').is(':checked');
    options.push({ text: optionText, isCorrect: isCorrect });
  });

  if (!questionText || !questionCategory || !questionDifficulty || !questionMarks) {
    toastr.warning("Please fill in all fields and provide exactly 4 options.");
    return;
  }

  const questionData = {
    text: questionText,
    categoryid: parseInt(questionCategory),
    difficulty: questionDifficulty,
    marks: parseInt(questionMarks),
    options: options
  };

  $.ajax({
    url: '/Question/CreateQuestion',
    type: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(questionData),
    success: function (response) {
      if (response.isSuccess) {
        toastr.success(response.message || "Question added successfully.");
        $('#questionModal').modal('hide');
        fetchQuestions();
      } else {
        toastr.error(response.message || "Failed to add question.");
      }
    },
    error: function (error) {
      console.error("Error adding question:", error);
      toastr.error("An error occurred while adding the question.");
    }
  });
}

function openEditQuestionModal(questionId) {
  $.ajax({
    url: `/Question/GetQuestionForEdit?id=${questionId}`,
    type: 'GET',
    success: function (response) {
      if (response.isSuccess) {
        const question = response.data;

        $('#editQuestionText').val(question.text);
        $('#editQuestionDifficulty').val(question.difficulty);
        $('#editQuestionMarks').val(question.marks);

        $.ajax({
          url: '/Category/GetAllCategories',
          type: 'GET',
          success: function (categoryResponse) {
            if (categoryResponse.success) {
              const categoryDropdown = $('#editQuestionCategory');
              categoryDropdown.empty();
              categoryDropdown.append('<option value="">Select Category</option>');

              categoryResponse.data.forEach(category => {
                const isSelected = category.id === question.categoryid ? 'selected' : '';
                categoryDropdown.append(`<option value="${category.id}" ${isSelected}>${category.name}</option>`);
              });
            } else {
              toastr.warning(categoryResponse.message || "Failed to fetch categories.");
            }
          },
          error: function (error) {
            console.error("Error fetching categories:", error);
            toastr.error("An error occurred while fetching categories.");
          }
        });

        const optionsGroup = $('#editOptionsGroup');
        optionsGroup.empty();
        question.options.forEach((option, index) => {
          const optionHtml = `
                      <div class="input-group mb-2">
                          <div class="input-group-text">
                              <input class="form-check-input" type="radio" name="editCorrectAnswer" value="${index}" ${option.isCorrect ? 'checked' : ''}>
                          </div>
                          <input type="text" class="form-control" value="${option.text}" required>
                      </div>
                  `;
          optionsGroup.append(optionHtml);
        });

        $('#editQuestionModal').modal('show');

        $('#editQuestionModal').data('questionId', questionId);
      } else {
        toastr.warning(response.message || "Failed to fetch question details.");
      }
    },
    error: function (error) {
      console.error("Error fetching question details:", error);
      toastr.error("An error occurred while fetching question details.");
    }
  });
}


function updateQuestion() {
  debugger
  const questionId = $('#editQuestionModal').data('questionId');
  const questionText = $('#editQuestionText').val().trim();
  const questionCategory = $('#editQuestionCategory').val();
  const questionDifficulty = $('#editQuestionDifficulty').val();
  const questionMarks = $('#editQuestionMarks').val();
  const options = [];

  $('#editOptionsGroup .input-group').each(function (index) {
    const optionText = $(this).find('input[type="text"]').val().trim();
    const isCorrect = $(this).find('input[type="radio"]').is(':checked');
    options.push({ text: optionText, isCorrect: isCorrect });
  });

  if (!questionText || !questionCategory || !questionDifficulty || !questionMarks) {
    toastr.warning("Please fill in all fields.");
    return;
  }

  const questionData = {
    id: questionId,
    text: questionText,
    categoryid: parseInt(questionCategory),
    difficulty: questionDifficulty,
    marks: parseInt(questionMarks),
    options: options
  };

  $.ajax({
    url: `/Question/EditQuestion?id=${questionId}`,
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(questionData),
    success: function (response) {
      if (response.isSuccess) {
        toastr.success(response.message || "Question updated successfully.");
        $('#editQuestionModal').modal('hide');
        fetchQuestions();
      } else {
        toastr.error(response.message || "Failed to update question.");
      }
    },
    error: function (error) {
      console.error("Error updating question:", error);
      toastr.error("An error occurred while updating the question.");
    }
  });
}


function deleteQuestion(questionId) {
  if (confirm("Are you sure you want to delete this question?")) {
    $.ajax({
      url: `/Question/SoftDeleteQuestion?id=${questionId}`,
      type: 'DELETE',
      success: function (response) {
        if (response.isSuccess) {
          toastr.success(response.message || "Question deleted successfully.");
          fetchQuestions();
        } else {
          toastr.error(response.message || "Failed to delete question.");
        }
      },
      error: function (error) {
        console.error("Error deleting question:", error);
        toastr.error("An error occurred while deleting the question.");
      }
    });
  }
}


function fetchQuizzes() {
  $.ajax({
    url: '/Quiz/GetAllQuizzes',
    type: 'GET',
    success: function (response) {
      if (response.isSuccess) {
        populateQuizzesTable(response.data);
      } else {
        toastr.warning(response.message || "Failed to fetch quizzes.");
      }
    },
    error: function (error) {
      console.error("Error fetching quizzes:", error);
      toastr.error("An error occurred while fetching quizzes.");
    }
  });
}

function populateQuizzesTable(quizzes) {
  const tableBody = $('#quizzesTable tbody');
  tableBody.empty();

  quizzes.forEach(quiz => {
    const isPublished = quiz.isPublic ? 'checked' : '';
    const row = `
          <tr>
              <td>
                  <div class="font-weight-bold">${quiz.title}</div>
              </td>
              <td>${quiz.categoryName}</td>
              <td>${quiz.totalQuestions}</td>
              <td>${quiz.durationMinutes} min</td>
              <td>
                  <div class="form-check form-switch">
                      <input class="form-check-input" type="checkbox" ${isPublished} onclick="togglePublish(${quiz.id}, this.checked)">
                  </div>
              </td>
              <td>
                  <button class="btn btn-sm me-1" onclick="openQuizModal('edit', ${quiz.id})">
                      <i class="fas fa-edit"></i>
                  </button>
                  <button class="btn btn-sm" onclick="deleteQuiz(${quiz.id})">
                      <i class="fas fa-trash"></i>
                  </button>
              </td>
          </tr>
      `;
    tableBody.append(row);
  });
}


function togglePublish(quizId, isPublished) {
  const url = isPublished ? `/Quiz/${quizId}/publish` : `/Quiz/${quizId}/unpublish`;

  $.ajax({
    url: url,
    type: 'PUT',
    success: function (response) {
      if (response.success) {
        toastr.success(response.message || "Quiz status updated successfully.");
        fetchQuizzes();
      } else {
        toastr.warning(response.message || "Failed to update quiz status.");
      }
    },
    error: function (error) {
      console.error("Error updating quiz status:", error);
      toastr.error("An error occurred while updating quiz status.");
    }
  });
}

function populateQuizCategoryDropdown() {
  $.ajax({
    url: '/Category/GetAllCategories',
    type: 'GET',
    success: function (response) {
      if (response.success) {
        const categoryDropdown = $('#quizCategory');
        categoryDropdown.empty();
        categoryDropdown.append('<option value="">Select Category</option>');
        response.data.forEach(category => {
          categoryDropdown.append(`<option value="${category.id}">${category.name}</option>`);
        });
      } else {
        toastr.warning(response.message || "Failed to fetch categories.");
      }
    },
    error: function (error) {
      console.error("Error fetching categories:", error);
      toastr.error("An error occurred while fetching categories.");
    }
  });
}

function saveQuiz() {
  const quizData = {
      title: $('#quizTitle').val().trim(),
      description: $('#quizDescription').val().trim(),
      categoryid: $('#quizCategory').val(),
      durationminutes: parseInt($('#quizTimeLimit').val()),
      totalmarks: parseInt($('#quizTotalMarks').val()), 
      ispublic: $('#quizPublished').is(':checked')
  };

  if (!quizData.title || !quizData.description || !quizData.categoryid || !quizData.durationminutes || !quizData.totalmarks) {
      toastr.warning("Please fill in all required fields.");
      return;
  }

  $.ajax({
      url: '/Quiz/CreateQuizOnly',
      type: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(quizData),
      success: function (response) {
          if (response.isSuccess) {
              toastr.success(response.message || "Quiz created successfully.");
              $('#quizModal').modal('hide');
              fetchQuizzes();
          } else {
              toastr.error(response.message || "Failed to create quiz.");
          }
      },
      error: function (error) {
          console.error("Error creating quiz:", error);
          toastr.error("An error occurred while creating the quiz.");
      }
  });
}

function openQuizModal(action, quizId) {
  if (action === 'edit') {
    $.ajax({
      url: `/Quiz/GetQuizForEdit?id=${quizId}`,
      type: 'GET',
      success: function (response) {
        if (response.isSuccess) {
          const quiz = response.data;

          $('#quizTitle').val(quiz.title);
          $('#quizDescription').val(quiz.description);
          $('#quizTimeLimit').val(quiz.durationminutes);
          $('#quizTotalMarks').val(quiz.totalmarks); 
          $('#quizPublished').prop('checked', quiz.ispublic);

          $.ajax({
            url: '/Category/GetAllCategories',
            type: 'GET',
            success: function (categoryResponse) {
              if (categoryResponse.success) {
                const categoryDropdown = $('#quizCategory');
                categoryDropdown.empty();
                categoryDropdown.append('<option value="">Select Category</option>');

                categoryResponse.data.forEach(category => {
                  const isSelected = category.id === quiz.categoryid ? 'selected' : '';
                  categoryDropdown.append(`<option value="${category.id}" ${isSelected}>${category.name}</option>`);
                });
              } else {
                toastr.warning(categoryResponse.message || "Failed to fetch categories.");
              }
            },
            error: function (error) {
              console.error("Error fetching categories:", error);
              toastr.error("An error occurred while fetching categories.");
            }
          });

          $('#quizModalTitle').text("Edit Quiz");
          console.log("Opening modal with quiz data:", quiz);
          $('#quizModal').modal('show');

          $('#saveQuizButton').off('click').on('click', function () {
            updateQuiz(quizId);
          });
        } else {
          toastr.warning(response.message || "Failed to fetch quiz details.");
        }
      },
      error: function (error) {
        console.error("Error fetching quiz details:", error);
        toastr.error("An error occurred while fetching quiz details.");
      }
    });
  }
}

function updateQuiz(quizId) {
  const quizData = {
    id: quizId,
    title: $('#quizTitle').val().trim(),
    description: $('#quizDescription').val().trim(),
    categoryid: $('#quizCategory').val(),
    durationminutes: parseInt($('#quizTimeLimit').val()),
    totalmarks: parseInt($('#quizTotalMarks').val()), 
    ispublic: $('#quizPublished').is(':checked')
  };

  if (!quizData.title || !quizData.description || !quizData.categoryid || !quizData.durationminutes || !quizData.totalmarks) {
    toastr.warning("Please fill in all required fields.");
    return;
  }

  $.ajax({
    url: '/Quiz/EditQuiz',
    type: 'PUT',
    contentType: 'application/json',
    data: JSON.stringify(quizData),
    success: function (response) {
      if (response.success) {
        toastr.success(response.message || "Quiz updated successfully.");
        $('#quizModal').modal('hide');
        fetchQuizzes();
      } else {
        toastr.error(response.message || "Failed to update quiz.");
      }
    },
    error: function (error) {
      console.error("Error updating quiz:", error);
      toastr.error("An error occurred while updating the quiz.");
    }
  });
}





function deleteQuiz(quizId) {
  if (confirm("Are you sure you want to delete this quiz?")) {
    $.ajax({
      url: `/Quiz/DeleteQuiz?id=${quizId}`,
      type: 'DELETE',
      success: function (response) {
        if (response.success) {
          toastr.success(response.message || "Quiz deleted successfully.");
          fetchQuizzes();
        } else {
          toastr.error(response.message || "Failed to delete quiz.");
        }
      },
      error: function (error) {
        console.error("Error deleting quiz:", error);
        toastr.error("An error occurred while deleting the quiz.");
      }
    });
  }
}