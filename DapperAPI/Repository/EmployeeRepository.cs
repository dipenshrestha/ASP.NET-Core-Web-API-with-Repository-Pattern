using Dapper;
using DapperAPI.IRepository;
using DapperAPI.Models;
using System.Net.NetworkInformation;

namespace DapperAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
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
