using Microsoft.EntityFrameworkCore;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.Data;
using WebApiTrainingProject.Repositories.Interfaces;

namespace WebApiTrainingProject.Repositories.Implementations
{
    public class ProjectRepository : BaseRepository<Project, ApplicationDbContext>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<List<Project>> GetAllByUserAsync(Guid userId)
        {
            return await _dbSet.Where(p => p.UserId == userId).ToListAsync();
        }
    }
}
