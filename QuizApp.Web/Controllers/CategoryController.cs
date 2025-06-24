using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Domain.Dto;
using QuizApp.Services.Interface;

namespace QuizApp.Web.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    // [HttpGet]
    // [Authorize(Roles = "Admin, User")]
    // public async Task<ActionResult<ResponseDto>> GetAllCategories()
    // {
    //     try
    //     {
    //         var categories = await _categoryService.GetAllCategoriesAsync();
    //         return categories != null && categories.Any()
    //             ? Ok(new ResponseDto(true, "Categories fetched successfully.", categories))
    //             : new ResponseDto(false, "No categories found.", null, 404);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error occurred while fetching categories.");
    //         return new ResponseDto(false, "An internal server error occurred.", null, 500);
    //     }
    // }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(new { success = true, data = categories });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching categories.");
            return StatusCode(500, new { success = false, message = "An internal server error occurred." });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        if (id <= 0)
            return BadRequest(new { success = false, message = "Invalid Category ID." });

        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return category != null
                ? Ok(new { success = true, data = category })
                : NotFound(new { success = false, message = "Category not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching category.");
            return StatusCode(500, new { success = false, message = "An internal server error occurred." });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ResponseDto>> CreateCategory([FromBody] CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var validationResult = await _categoryService.validateCategoryAsync(dto);
            if (!validationResult.IsValid)
                return new ResponseDto(false, validationResult.ErrorMessage, null, 400);

            var createdCategory = await _categoryService.CreateCategoryAsync(dto);
            return createdCategory != null
                ? new ResponseDto(true, "Category created successfully.", createdCategory)
                : new ResponseDto(false, "Failed to create category.", null, 400);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating category.");
            return new ResponseDto(false, "An internal server error occurred.", null, 500);
        }
    }


    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Invalid data provided." });

        try
        {
            var validationResult = await _categoryService.validateCategoryUpdateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(new { success = false, message = validationResult.ErrorMessage });

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, dto);
            return updatedCategory != null
                ? Ok(new { success = true, message = "Category updated successfully.", data = updatedCategory })
                : NotFound(new { success = false, message = "Category not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating category.");
            return StatusCode(500, new { success = false, message = "An internal server error occurred." });
        }
    }


    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            return deleted
                ? Ok(new { success = true, message = "Category deleted successfully." })
                : NotFound(new { success = false, message = "Category not found or already deleted." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting category.");
            return StatusCode(500, new { success = false, message = "An internal server error occurred." });
        }
    }
}
