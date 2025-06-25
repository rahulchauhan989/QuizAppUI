using Microsoft.AspNetCore.Mvc;

namespace QuizApp.Web.Controllers;

public class UserController : Controller
{
    public IActionResult Quiz()
    {
        return View();
    }
}
