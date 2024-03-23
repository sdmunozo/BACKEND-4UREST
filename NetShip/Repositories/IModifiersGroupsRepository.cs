using NetShip.DTOs.Common;
using NetShip.Entities;

namespace NetShip.Repositories
{
    public interface IModifiersGroupsRepository
    {
        Task<List<ModifiersGroup>> GetAll(PaginationDTO paginationDTO);
        Task<ModifiersGroup?> GetById(Guid id);
        Task<Guid> Create(ModifiersGroup modifiersGroup);
        Task<bool> Exist(Guid id);
        Task Update(ModifiersGroup modifiersGroup);
        Task Delete(Guid id);
        Task<List<ModifiersGroup>> GetByName(string name);
        Task<Guid?> GetBranchIdByModifiersGroupId(Guid modifiersGroupId);
    }
}
