using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleAdsLogIns.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SimpleAdsLogIns.Web.Controllers
{
    public class Account : Controller
    {
        private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Ads;Integrated Security=true;";
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repo = new AdsRepository(_connectionString);
            repo.AddUser(user, password);
            return Redirect("Login");
        }
        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
            }
            [HttpPost]
            public IActionResult Login(string email, string password)
            {
                var repo = new AdsRepository(_connectionString);
                var user = repo.Login(email, password);
                if (user == null)
                {
                    TempData["message"] = "Invalid login!";
                    return RedirectToAction("Login");
                }

            var claims = new List<Claim>
                {
                    new Claim ("user", email)
                };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/Home/NewAd");

            }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
