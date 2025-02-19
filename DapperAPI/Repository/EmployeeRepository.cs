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
        //to check wheather the department with the id exists or not
        public async Task<bool> DepartmentIdExistsAsync(int id)
        {
            using var connection = _context.CreateConnection();
            // Returns true if an employee with the same details existsusing var connection = _context.CreateConnection();
            var sql = @"
            SELECT COUNT(*) 
            FROM Department 
            WHERE DepartmentId = @id";

            int count = await connection.ExecuteScalarAsync<int>(sql, new { id });

            return count > 0;
        }

        //to check wheather the designation with the id exists or not
        public async Task<bool> DesignationIdExistsAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            SELECT COUNT(*) 
            FROM Designation 
            WHERE DesignationId = @id";
            int count = await connection.ExecuteScalarAsync<int>(sql, new { id });
            return count > 0;
        }
        public async Task AddItemAsync(Employee obj)
        {
            using var connection = _context.CreateConnection();
            //Check if employee with the id already exists
            bool hasDepartId = await DepartmentIdExistsAsync(obj.DepartmentId);
            bool hasDesigId = await DesignationIdExistsAsync(obj.DesignationId);
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
