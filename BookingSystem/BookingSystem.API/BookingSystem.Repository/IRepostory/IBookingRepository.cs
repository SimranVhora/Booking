using BookingSystem.Data.Models;
using Microsoft.AspNetCore.Http;


namespace BookingSystem.Repository.IRepostory;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAllAsync();
    Task<Booking?> GetByIdAsync(int id);
    Task<int> AddAsync(Booking booking);
    Task<bool> UpdateAsync(Booking booking);
    Task<bool> DeleteAsync(int id);
}
