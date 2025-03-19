using BookingSystem.Data;
using BookingSystem.Data.Models;
using BookingSystem.Repository.IRepostory;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Repository;


public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _fileUploadPath = "wwwroot/uploads"; // Set your actual file upload path

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings.FromSqlRaw("EXEC sp_GetAllBookings").ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        var result = await _context.Bookings
            .FromSqlRaw("EXEC sp_GetBookingById @BookingId", new SqlParameter("@BookingId", id))
            .ToListAsync();

        return result.FirstOrDefault();
    }

    public async Task<int> AddAsync(Booking booking)
    {
        string filePath = await SaveFileAsync(booking.FilePath);

        var bookingIdParam = new SqlParameter("@BookingId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };

        await _context.Database.ExecuteSqlRawAsync("EXEC sp_AddBooking @CustomerId, @EmailID, @Mobile, @StartDate, @EndDate, @FilePath, @BookingId OUTPUT",
            new SqlParameter("@CustomerId", booking.CustomerId),
            new SqlParameter("@EmailID", booking.EmailID),
            new SqlParameter("@Mobile", booking.Mobile),
            new SqlParameter("@StartDate", booking.StartDate),
            new SqlParameter("@EndDate", booking.EndDate),
            new SqlParameter("@FilePath", filePath),
            bookingIdParam);

        return (int)bookingIdParam.Value;
    }

    public async Task<bool> UpdateAsync(Booking booking)
    {
        string filePath = booking.FilePath;

        if (filePath != null)
        {
            filePath = await SaveFileAsync(booking.FilePath);
        }

        var result = await _context.Database.ExecuteSqlRawAsync("EXEC sp_UpdateBooking @BookingId, @CustomerId, @EmailID, @Mobile, @StartDate, @EndDate, @FilePath",
            new SqlParameter("@BookingId", booking.BookingId),
            new SqlParameter("@CustomerId", booking.CustomerId),
            new SqlParameter("@EmailID", booking.EmailID),
            new SqlParameter("@Mobile", booking.Mobile),
            new SqlParameter("@StartDate", booking.StartDate),
            new SqlParameter("@EndDate", booking.EndDate),
            new SqlParameter("@FilePath", filePath));

        return result > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteBooking @BookingId",
            new SqlParameter("@BookingId", id));

        return result > 0;
    }

    private async Task<string> SaveFileAsync(string sourceFilePath)
    {
        if (string.IsNullOrWhiteSpace(sourceFilePath) || !File.Exists(sourceFilePath))
            return string.Empty;

        string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(sourceFilePath)}";
        string destinationPath = Path.Combine(_fileUploadPath, fileName);

        Directory.CreateDirectory(_fileUploadPath); // Ensure directory exists

        using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
        using (var destinationStream = new FileStream(destinationPath, FileMode.Create))
        {
            await sourceStream.CopyToAsync(destinationStream);
        }

        return fileName; // Return only the file name for database storage
    }

}
