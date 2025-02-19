using Dapper;
using DapperAPI.IRepository;
using DapperAPI.Models;

namespace DapperAPI.Repository
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly DapperContext _context;
        public DesignationRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Designation>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Designation>("SELECT * FROM Designation");
        }
        public async Task<Designation> GetDesignationById(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM Designation WHERE DesignationId = @id";
            var result = await connection.QueryAsync<Designation>(sql, new {id});
            return result.FirstOrDefault() ?? throw new KeyNotFoundException($"Designation with ID {id} not found.");
        }
        public async Task<bool> DesignationExistsAsync(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
            SELECT COUNT(*) 
            FROM Designation 
            WHERE DesignationName = @name";
            int count = await connection.ExecuteScalarAsync<int>(sql, new { name });
            return count > 0;
        }
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
        public async Task AddItemAsync(Designation item)
        {
            using var connection = _context.CreateConnection();
            //Check if employee already exists
            bool exists = await DesignationExistsAsync(item.DesignationName);
            if (exists)
            {
                throw new InvalidOperationException("An Department with the same Name already exists.");
            }
            // $"" allows us to use string interpolation thats why we use it
            var sql = $"INSERT INTO Designation (DesignationName) VALUES (@name)";
            await connection.ExecuteAsync(sql, new { name = item.DesignationName });
        }

        public async Task DeleteAsync(Designation designation)
        {
            using var connection = _context.CreateConnection();
            var sql = $" DELETE FROM Designation WHERE DesignationID = @id";
            await connection.ExecuteAsync(sql, new { id = designation.DesignationId });
        }

        public async Task UpdateAsync(Designation designation)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Designation SET DesignationName = @name WHERE DesignationId = @id";

            // Passing both DepartmentName and DepartmentId as parameters to the query
            await connection.ExecuteAsync(sql, new
            {
                name = designation.DesignationName,
                id = designation.DesignationId
            });
        }
    }
}
