using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace contact_management.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserInterface _userRepo;


        public UserController(ILogger<UserController> logger, IUserInterface userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        #region Register

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            int response = await _userRepo.AddUser(user);

            if (response == 0)
            {
                TempData["Message"] = "User Already Exist!";
                return RedirectToAction("Register", "User");
                // return Json(new { success = false, message = "Already Exist!" });
            }
            else if (response > 0)
            {
                TempData["Message"] = "Register Successfully";
                return RedirectToAction("Index", "Home");
                // return Json(new { success = true, message = "Register Successfully" });
            }
            else
            {
                TempData["Message"] = "An error occurred!";
                TempData["Message"] = "Error";
                return View(user);
            }
        }
        #endregion register end 

        #region Login
        public ActionResult Login()
        {
            return View();
        }
            [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {

                var user = await _userRepo.GetUser(login);
            if(user != null && user.c_id != 0 )
            {
                HttpContext.Session.SetInt32("UserId", user.c_id);
                HttpContext.Session.SetString("Username", user.c_name);
                TempData["Message"] = "Login Successfully";
                return RedirectToAction("Login", "User");  
            }
            TempData["Message"] = "Invalid Email or Password";
            return View(login);
        }
        #endregion 
    }
}