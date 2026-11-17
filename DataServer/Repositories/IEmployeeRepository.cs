using DataServer.Entities;

namespace DataServer.Repositories;

public interface IEmployeeRepository
{
    Task<Employee> GetByUsernameAsync(string username);
}