using BookingSystem.Data;
using BookingSystem.Data.Models;
using BookingSystem.Repository.IRepostory;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Repository;

public class CustomerRepository(ApplicationDbContext _context): ICustomerRepository
{
    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .FromSqlRaw("EXEC sp_GetAllCustomers")
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        var customers = await _context.Customers
            .FromSqlRaw("EXEC sp_GetCustomerById @p0", id)
            .ToListAsync();
        return customers.FirstOrDefault();
    }

    public async Task<int> AddAsync(Customer customer)
    {
        var customerIdParam = new SqlParameter("@CustomerId", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_AddCustomer @Name, @Email, @CustomerId OUTPUT",
            new SqlParameter("@Name", customer.Name),
            new SqlParameter("@Email", customer.Email),
            customerIdParam 
        );

        return (int)customerIdParam.Value;
    }

    public async Task UpdateAsync(Customer customer)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_UpdateCustomer @p0, @p1, @p2",
            customer.CustomerId, customer.Name, customer.Email);
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteCustomer @p0", id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var emailExistsParam = new SqlParameter("@EmailID", email);

        var result = await _context.Database.ExecuteSqlRawAsync("EXEC sp_CheckEmailExists @EmailID", emailExistsParam);

        return result > 0; // Returns true if email exists, false otherwise.
    }


}
