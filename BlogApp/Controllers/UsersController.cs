using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Model;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index","Posts");
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x=>x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.Create(new User{
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "avatar.jpg"
                    });
                    return RedirectToAction("Login");
                }
                else 
                {
                    ModelState.AddModelError("","Username yada email kullanimda.");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(k=>k.Email == model.Email && k.Password == model.Password) ;

                if (isUser != null)
                {
                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));     
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));     
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));     
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));     

                    if (isUser.Email == "vusalhesenov361@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));     
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims,CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new  AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties
                    );

                    return RedirectToAction("Index","Posts");
                }
                else 
                {
                    ModelState.AddModelError("", "Kullanici adi veya sifre yanlis");
                }

            }
            
            return View(model);
        }

        


    }
}