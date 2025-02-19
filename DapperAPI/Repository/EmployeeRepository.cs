using Dapper;
using DapperAPI.IRepository;
using DapperAPI.Models;
using System.Net.NetworkInformation;

namespace DapperAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;
        private readonly IDepartmentRepository _depart;
        private readonly IDesignationRepository _desig;

        public EmployeeRepository(DapperContext context, IDepartmentRepository depart, IDesignationRepository desig)
        {
            _context = context;
            _depart = depart;
            _desig = desig;
        }
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM Employee";
            return await connection.QueryAsync<Employee>(sql);
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM Employee WHERE EmployeeId = @id";
            var result = await connection.QueryAsync<Employee>(sql, new {id});
            return result.FirstOrDefault() ?? throw new KeyNotFoundException($"Employee with ID {id} not found.");
        }

        public async Task AddItemAsync(Employee obj)
        {
            using var connection = _context.CreateConnection();
            //Check if employee with the id already exists
            bool hasDepartId = await _depart.DepartmentIdExistsAsync(obj.DepartmentId);
            bool hasDesigId = await _desig.DesignationIdExistsAsync(obj.DesignationId);
            //if anyone is false or both is false then this statement runs
            if (!hasDepartId || !hasDesigId)
            {
                throw new InvalidOperationException("An Department or Designation Id doesnot exists.");
            }
            var sql = $"INSERT INTO Employee (FirstName,LastName,Email,PhoneNumber,DepartmentId,DesignationId) VALUES (@fname,@lname,@email,@phnum,@deptId,@desigId)";
            await connection.ExecuteAsync(sql, new
            {
                fname = obj.FirstName,
                lname = obj.LastName,
                email = obj.Email,
                phnum = obj.PhoneNumber,
                deptId = obj.DepartmentId,
                desigId = obj.DesignationId
            });
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"DELETE FROM Employee WHERE EmployeeId = @id";
            await connection.ExecuteAsync(sql,new { id });
        }

        public async Task UpdateAsync(Employee obj)
        {
            using var connection = _context.CreateConnection();
            var sql = $"UPDATE Employee SET FirstName = @fname, LastName=@lname, Email=@email, PhoneNumber=@phnum, DepartmentId=@deptId, DesignationId=@desigId WHERE EmployeeId = @id";
            await connection.ExecuteAsync(sql, new
            {
                id = obj.EmployeeId,
                fname = obj.FirstName,
                lname = obj.LastName,
                email = obj.Email,
                phnum = obj.PhoneNumber,
                deptId = obj.DepartmentId,
                desigId = obj.DesignationId
            });
        }
    }
}
