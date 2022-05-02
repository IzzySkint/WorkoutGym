using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkoutGym.Data;
using WorkoutGym.Models;

namespace WorkoutGym.Controllers;

public class AccountController : Controller
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AccountController(IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        this._mapper = mapper;
        this._userManager = userManager;
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(UserRegistrationModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _mapper.Map<ApplicationUser>(model);

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "Member");
        
        return RedirectToAction(nameof(AccountController.Login), "Account");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(UserLoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);

        if ((user != null) && (await _userManager.CheckPasswordAsync(user, model.Password)))
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var isMember = await _userManager.IsInRoleAsync(user, "Member");
            
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            if (isAdmin)
            {
                identity.AddClaim((new Claim(ClaimTypes.Role, "Admin")));
            }
            else if (isMember)
            {
                identity.AddClaim((new Claim(ClaimTypes.Role, "Member")));
            }
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));

            if (isAdmin)
            {
                return RedirectToAction(nameof(AdminController.Dashboard), "Admin");
            }
            else if (isMember)
            {
                return RedirectToAction(nameof(MemberController.Dashboard), "Member");
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        else
        {
            ModelState.AddModelError("","Invalid Email or Password");
            return View(model);
        }
    }
}