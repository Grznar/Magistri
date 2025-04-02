using Magistri.Application.Common.Interfaces;
using Magistri.Application.Common.Utlity;
using Magistri.Domain.Entities;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Magistri.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager; 
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            
            LoginVM loginVM = new()
            {
               
            };
            return View(loginVM);
        }
        public async Task<IActionResult> Register()
        {
            if (!await _roleManager.RoleExistsAsync(SD.Role_Teacher))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Teacher));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Student));
            }

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }).ToList(), 
            };
            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    UserName = registerVM.Email,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    Name = registerVM.Name,
                    

                };


                var result_ = _userManager.CreateAsync(user, registerVM.Password).GetAwaiter().GetResult();

                if (result_.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Teacher).GetAwaiter().GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole(SD.Role_Teacher)).Wait();
                        _roleManager.CreateAsync(new IdentityRole(SD.Role_Student)).Wait();
                    }
                        if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Student);
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");



                }

                foreach (var error in result_.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                });
            }
            return View(registerVM);


        }
        public async Task<IActionResult> Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password,true, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Teacher))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        
                            return RedirectToAction("Index", "Home");
                        
                    }
                }
                else
                {

                    ModelState.AddModelError("", "Invalid Login Attempt");
                }
            }
            return View(loginVM);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
