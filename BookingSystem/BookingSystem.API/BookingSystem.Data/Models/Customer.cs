using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Data.Models;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name should be between 2 to 50 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [UniqueEmail]
    public string Email { get; set; } = string.Empty;

    public List<Booking>? Bookings { get; set; }
}
