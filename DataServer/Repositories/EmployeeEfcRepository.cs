using DataServer.Data;
using DataServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataServer.Repositories;

public class EmployeeEfcRepository
{
    private readonly AppDbContext db;
    
    public EmployeeEfcRepository(AppDbContext db)
    {
        this.db = db;
    }
    
    //find the employee who is trying to log in
    public async Task<Employee> GetByUsernameAsync(string username)
    {
        return await db.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Username == username);
    }
}