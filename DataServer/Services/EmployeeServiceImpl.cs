using DataServer.Entities;
using DataServer.Grpc;
using DataServer.Repositories;
using Grpc.Core;


namespace DataServer.Services;

public class EmployeeServiceImpl : EmployeeService.EmployeeServiceBase
{
    private readonly IEmployeeRepository repository;
    
    public EmployeeServiceImpl(IEmployeeRepository repository)
    {
        this.repository = repository;
    }
    
    //send the employee's password (hash) and role to Java
    public override async Task<EmployeeReply> GetByUsername(
        GetByUsernameRequest request,
        ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                "Username is required"));
        }

        Employee? employee =
            await repository.GetByUsernameAsync(request.Username.Trim());

        if (employee == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                "Employee not found"));
        }

        return new EmployeeReply
        {
            Id = employee.Id,
            Username = employee.Username,
            PasswordHash = employee.PasswordHash,
            Role = ToGrpcRole(employee.Role),
        };
    }

    private static RoleType ToGrpcRole(Role role) => role switch
    {
        Role.WarehouseWorker => RoleType.WarehouseWorker,
        Role.DispatchManager => RoleType.DispatchManager,
        _ => throw new ArgumentOutOfRangeException(nameof(role))
    };
}