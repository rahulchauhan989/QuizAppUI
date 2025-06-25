using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Domain.Dto;
using QuizApp.Services.Interface;

namespace QuizApp.Web.Controllers;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var validationResult = await _loginService.ValidateLoginAsync(request);
            if (!validationResult.IsValid)
            {
                ModelState.AddModelError(nameof(request.password), "Invalid email or password"); ;
                return View(request);
            }

            string token = await _loginService.GenerateToken(request);
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToAction("Dashboard", "Admin");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Login method: {ex.Message}");
            ModelState.AddModelError(string.Empty, "Internal server error");
            return View(request);
        }
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            if (request == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid registration data");
                return View(request);
            }

            string result = await _loginService.RegisterUserAsync(request);

            if (result == "User already exists")
            {
                ModelState.AddModelError(string.Empty, result);
                return View(request);
            }

            return RedirectToAction("Login", "Login");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Register method: {ex.Message}");
            ModelState.AddModelError(string.Empty, "Internal server error");
            return View(request);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            string? token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                token = Request.Cookies["jwtToken"];
            }

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token not found.");

            var userProfile = await _loginService.GetCurrentUserProfileAsync(token);

            if (userProfile == null)
                return NotFound("User not found");

            return View(userProfile);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in GetCurrentUser: " + ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public IActionResult PageNotFond()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}