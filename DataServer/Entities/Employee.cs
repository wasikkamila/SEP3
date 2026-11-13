namespace DataServer.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public Role Role { get; set; }
}