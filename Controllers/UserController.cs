using System.Diagnostics;
using LoginReg.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wedding_planner.Models;

namespace wedding_planner.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;

    private MyContext _context; 

    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;


        _context = context;
    }

    //LOGIN REG VIEW**************
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    //HOME PAGE VIEW***************
    [SessionCheck]
    [HttpGet("home")]
    public IActionResult Home()
    {
        List<Wedding> Weddings = _context.Weddings.Include(w => w.UserAttending).ToList();
        return View("Home", Weddings);
    }

    //REGISTERPOST ROUTE**********************

    [HttpPost("users/register")]
    public IActionResult Register(User newUser)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> hashing = new();
        newUser.Password = hashing.HashPassword(newUser, newUser.Password);
        _context.Add(newUser);
        _context.SaveChanges();
        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        return RedirectToAction("Home");
    }


    //USER LOGIN ROUTE************************

    [HttpPost("users/login")]
    public IActionResult Login(LogUser LogAttempt)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }
        User? dbUser = _context.Users.FirstOrDefault(u => u.Email == LogAttempt.LogEmail);
        if(dbUser == null)
        {
            ModelState.AddModelError("LogPassword", "Invalid Login Attempt");
            return View("Index");
        }
        PasswordHasher<LogUser> hashing = new();
        PasswordVerificationResult pwCompareResult = hashing.VerifyHashedPassword(LogAttempt, dbUser.Password, LogAttempt.LogPassword);
        if(pwCompareResult == 0)
        {
            ModelState.AddModelError("LogPassword", "Invalid Login Attempt");
            return View("Index");
        }
        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        return RedirectToAction("Home");
    }


    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UUID");
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}