using Kindergarten_Management_System.Data;
using Kindergarten_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;

        public AccountController
            (
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IPasswordHasher<AppUser> passwordHasher
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
        }

        // GET /account/login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login
            {
                ReturnUrl = returnUrl
            };

            return View(login);
        }

        // Post /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                    if (appUser != null)
                    {
                        await signInManager.SignOutAsync();
                        Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, true);
                        if (appUser.Status == false)
                        {
                            ModelState.AddModelError("", "Your account is locked, try to contact an admin.");
                            throw new ArgumentNullException();
                        }

                        if (result.Succeeded)
                            return Redirect(login.ReturnUrl ?? "/");

                        if (result.IsLockedOut)
                            ModelState.AddModelError("", "Your account is locked out try again later.");
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Login failed, wrong credentials.");
                return View();
            }
        }
            // GET /account/logout
            public async Task<IActionResult> Logout()
            {
                await signInManager.SignOutAsync();
                return Redirect("/");
            }

            public IActionResult AccessDenied()
            {
                return View();
            }
        }
    }

