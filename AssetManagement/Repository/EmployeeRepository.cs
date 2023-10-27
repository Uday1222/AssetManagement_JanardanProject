using AssetManagement.Data;
using AssetManagement.Models;
using AssetManagement.Repository.IRepository;

namespace AssetManagement.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Employee> Update(Employee entity)
        {
            _db.Employees.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
