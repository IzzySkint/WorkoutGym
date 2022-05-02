using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutGym.Controllers;

[Authorize(Roles = "Member")]
public class MemberController : Controller
{
    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpGet]
    public IActionResult BookSession()
    {
        return View();
    }
}