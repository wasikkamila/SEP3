using DataServer.Entities;

namespace DataServer.Repositories;

public interface IVanRepository
{
    Task<List<Van>> GetAllAsync();
    Task<List<VanParcelCount>> GetParcelCountAsync();
}
