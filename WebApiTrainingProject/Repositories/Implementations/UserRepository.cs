using Microsoft.EntityFrameworkCore;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.Data;
using WebApiTrainingProject.Repositories.Interfaces;

namespace WebApiTrainingProject.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User, ApplicationDbContext>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
