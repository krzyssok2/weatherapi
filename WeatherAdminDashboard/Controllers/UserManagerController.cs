using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherAdminDashboard.Models;
using WeatherForecastAPI;
using WeatherForecastAPI.Entities;

namespace WeatherAdminDashboard.Controllers
{
   
    public class UserManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public WeatherContext _context;
        public UserManagerController(WeatherContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var users = GetAllUsers();
            return View(new UsersViewModel {  UserEmails = users});
        }
        //public IActionResult Create(string email, string password)
        //{
        //    return View();
        //}
        public async Task<IActionResult> Create(string email, string password)
        {
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = email,
                Email = email,
            }, password);

            if (!result.Succeeded)
            {
                Redirect("/UserManager");
            }
            else
            {
                _context.UserSettings.Add(new UserSettings
                {
                    User = email,
                    Units = 0,
                });
                _context.SaveChanges();
            }
            return Redirect("/UserManager");
        }

        public async Task<IActionResult> Delete(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            var result = await _userManager.DeleteAsync(user);
            _context.SaveChanges();
            return Redirect("/UserManager");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        List<string> GetAllUsers()
        {
            return _context.Users.Select(i => i.UserName).ToList();
        }
    }
}