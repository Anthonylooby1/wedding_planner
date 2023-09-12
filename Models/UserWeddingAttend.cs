#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding_planner.Models;

public class UserWeddingAttend
{
    [Key]
    public int UserWeddingAttendId {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public int UserId {get;set;}
    public User? Attending {get;set;}

    public int WeddingId {get;set;}
    public Wedding? Weddings {get;set;}
}