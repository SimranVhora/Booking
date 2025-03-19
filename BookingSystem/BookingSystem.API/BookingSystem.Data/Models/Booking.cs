using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Data.Models;

public class Booking
{
    [Key]
    [Display(Name = "Booking Id")]
    public int BookingId { get; set; }

    [Required,ForeignKey("Customer")]
    public int CustomerId { get; set; }

    [Required,EmailAddress]
    [Display(Name ="Email ID")]
    public String EmailID { get; set; }=string.Empty;

    [Required]
    [RegularExpression(@"^[0-9]{10}$",ErrorMessage ="Mobile number must be 10 digits.")]
    [Display(Name = "Phone Number")]
    public string Mobile { get; set; } = string.Empty;

    [Required,DataType(DataType.Date)]
    [StartDateBeforeEndDate]
    [Display(Name ="Start Date")]
    public DateTime StartDate { get; set; }
    
    [Required,DataType(DataType.Date)]
    [Display(Name ="End Date")]
    public DateTime EndDate { get; set; }

    [Required]
    public string FilePath { get; set; } = string.Empty;

    public Customer? Customer { get; set; }
}

