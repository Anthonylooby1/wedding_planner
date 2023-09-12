#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding_planner.Models;

public class Wedding
{
    [Key]
    public int WeddingId {get;set;}

    [Required]
    public string NameOne {get;set;}

    [Required]
    public string NameTwo {get;set;}

    [Required]
    public DateTime WeddingDate {get;set;}

    [Required]
    public string Address {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    //foreign key
    public int UserId {get;set;}
    //Nav Prop
    public User? OneUser {get;set;}

    public List<UserWeddingAttend> UserAttending {get;set;} = new();
}