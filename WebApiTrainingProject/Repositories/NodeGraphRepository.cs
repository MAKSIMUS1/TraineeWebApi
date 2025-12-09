using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.Data;
using WebApiTrainingProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApiTrainingProject.Repositories
{
    public class NodeGraphRepository : BaseRepository<NodeGraph, ApplicationDbContext>, INodeGraphRepository
    {
        public NodeGraphRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<NodeGraph> GetByIdAsyncIncludeProject(Guid id)
        {
            return await _dbSet
                .Include(g => g.Project)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task<List<NodeGraph>> GetAllByUserIdAsync(Guid userId)
        {
            return await _dbSet.Include(g => g.Project)
                .Where(g => g.Project.UserId == userId)
                .ToListAsync();
        }
    }
}
