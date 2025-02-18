using DapperAPI.Models;

namespace DapperAPI.IRepository
{
    public interface IDepartmentRepository
    {
        // we use IEnumerable<T> beacause this is suppose to return list of or all rows from the table
        Task<bool> DepartmentExistsAsync(string name); //to check if the Department exists or not

        Task<IEnumerable<Department>> GetAllAsync(); //to get all the department details
        Task<Department> GetDepartmentById(int id); //to get one department list i.e. using id
        Task AddItemAsync(Department item);
        Task DeleteAsync(Department department);
        Task UpdateAsync(Department department); //no return type as we dont need in this
    }
}
