using BookingSystem.Data.Models;
using BookingSystem.Repository.IRepostory;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.API.Controllers;

[Route("api/[controller]")]
public class CustomerController(ICustomerRepository _customerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _customerService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _customerService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]Customer customer)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if email already exists
        if (await _customerService.EmailExistsAsync(customer.Email))
        {
            return BadRequest("Email already exists.");
        }

        var newCustomerId = await _customerService.AddAsync(customer);
        return CreatedAtAction(nameof(GetById), new { id = newCustomerId }, new { CustomerId = newCustomerId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != customer.CustomerId)
            return BadRequest(new { message = "Invalid customer ID." });


        await _customerService.UpdateAsync(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.DeleteAsync(id);
        return NoContent();
    }
}
