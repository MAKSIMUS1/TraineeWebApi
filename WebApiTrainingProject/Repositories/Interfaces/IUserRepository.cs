using WebApiTrainingProject.Data.Models;

namespace WebApiTrainingProject.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
    }
}
