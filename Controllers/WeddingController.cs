using System.Diagnostics;
using LoginReg.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wedding_planner.Models;

namespace wedding_planner.Controllers;

public class WeddingController : Controller
{
        private readonly ILogger<WeddingController> _logger;

    private MyContext _context; 

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;


        _context = context;
    }

    //CREATE A WEDDING VIEW**************
    [SessionCheck]
    [HttpGet("wedding/new")]
    public ViewResult NewWedding()
    {
        return View("CreateWedding");
    }

    [SessionCheck]
    [HttpPost("create/wedding")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if(!ModelState.IsValid)
        {
            return View("CreateWedding");
        }
        newWedding.UserId = (int)HttpContext.Session.GetInt32("UUID");
        _context.Add(newWedding);
        _context.SaveChanges();
        return RedirectToAction("Home", "User");
    }


    [SessionCheck]
    [HttpPost("delete")]
    public IActionResult Delete(int WeddingId)
    {
        Wedding? deleted = _context.Weddings.SingleOrDefault(w => w.WeddingId == WeddingId);
        if(deleted != null)
        {
            _context.Remove(deleted);
            _context.SaveChanges();
        }
        return RedirectToAction("Home", "User");
    }


    [SessionCheck]
    [HttpGet("wedding/{WeddingId}")]
    public IActionResult ViewWedding(int WeddingId)
    {
        Wedding? OneWedding = _context.Weddings.Include(w => w.OneUser)
        .Include(w => w.UserAttending)
        .ThenInclude(w => w.Attending)
        .FirstOrDefault(w => w.WeddingId == WeddingId);
        if(OneWedding == null)
        {
            return RedirectToAction("Home");
        }
        return View(OneWedding);
    }


    [SessionCheck]
    [HttpPost("wedding/{weddingId}/attending")]
    public IActionResult ToggleAttend(int weddingId)
    {
        int UUID = (int)HttpContext.Session.GetInt32("UUID");
        UserWeddingAttend attending = _context.UserWeddingAttending.FirstOrDefault(a => a.WeddingId == weddingId && a.UserId == UUID);
        
        if(attending == null)
        {
            UserWeddingAttend newAttendee = new()
            {
                UserId = UUID,
                WeddingId = weddingId
            };
            _context.Add(newAttendee);
            
        } 
        else
        {
            _context.Remove(attending);
        }
    _context.SaveChanges();
    return RedirectToAction("Home", "User");
    }
}

