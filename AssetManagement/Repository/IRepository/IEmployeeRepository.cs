using AssetManagement.Models;

namespace AssetManagement.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> Update(Employee entity);
    }
}
