using BookingSystem.Data.Models;
using BookingSystem.Repository.IRepostory;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingService;

    public BookingController(IBookingRepository bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _bookingService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        if (booking == null) return NotFound();

        return Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Booking booking)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newBookingId = await _bookingService.AddAsync(booking);
        return CreatedAtAction(nameof(GetById), new { id = newBookingId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Booking booking)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != booking.BookingId) return BadRequest("Invalid booking ID");

        var updated = await _bookingService.UpdateAsync(booking);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bookingService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
