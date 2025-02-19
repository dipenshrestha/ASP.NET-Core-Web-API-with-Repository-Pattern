using Dapper;
using DapperAPI.IRepository;
using DapperAPI.Models;

namespace DapperAPI.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DapperContext _context;
        public DepartmentRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            // connection.Close(); // You MUST close it manually if u dont use using
            return await connection.QueryAsync<Department>("SELECT * FROM Department");
        }

        //It returns a Task<Department>, meaning it will return a Department object asynchronously
        public async Task<Department> GetDepartmentById(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM dbo.Department WHERE DepartmentId = @id";
            var result = await connection.QueryAsync<Department>(sql, new {id});
            //use throw as this result can have a Null reference 
            //return result.FirstOrDefault() ?? throw new KeyNotFoundException($"Department with ID {id} not found.");
            //but dont use it as it is. It will throw detailed error into swagger which can be hacked later and its not user friendly

            //The ?? operator in C# is called the null-coalescing operator. It is used to provide a default value when the left-hand side expression is null
            //we can also use it like this
            /*
            var department = result.FirstOrDefault();
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found.");
            }
            return department;
            */

            return result.FirstOrDefault() ?? throw new KeyNotFoundException($"Department with ID {id} not found.");
        }
        public async Task<bool> DepartmentExistsAsync(string name)
        {
            using var connection = _context.CreateConnection();
            // Returns true if an Depertment with the same details existsusing var connection = _context.CreateConnection();
            var sql = @"
            SELECT COUNT(*) 
            FROM Department 
            WHERE DepartmentName = @name";

            int count = await connection.ExecuteScalarAsync<int>(sql, new { name });

            return count > 0;
        }
        public async Task AddItemAsync(Department item)
        {
            using var connection = _context.CreateConnection();
            //Check if employee already exists
            bool exists = await DepartmentExistsAsync(item.DepartmentName);
            if (exists)
            {
                throw new InvalidOperationException("An Department with the same Name already exists.");
            }

            // $"" allows us to use string interpolation thats why we use it
            var sql = $"INSERT INTO Department (DepartmentName) VALUES (@name)";
            await connection.ExecuteAsync(sql, new {name = item.DepartmentName});
        }
        public async Task DeleteAsync(Department department)
        {
            using var connection = _context.CreateConnection();
            var sql = $" DELETE FROM Department WHERE DepartmentID = @id";
            await connection.ExecuteAsync(sql, new { id = department.DepartmentId });
        }

        public async Task UpdateAsync(Department department)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Department SET DepartmentName = @name WHERE DepartmentId = @id";

            // Passing both DepartmentName and DepartmentId as parameters to the query
            await connection.ExecuteAsync(sql, new
            {
                name = department.DepartmentName,
                id = department.DepartmentId
            });
        }
    }
}
