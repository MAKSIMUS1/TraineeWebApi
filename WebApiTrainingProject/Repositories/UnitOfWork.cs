using WebApiTrainingProject.Data;
using WebApiTrainingProject.Repositories.Interfaces;

namespace WebApiTrainingProject.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProjectRepository Projects { get; }
        public IUserRepository Users { get; }
        public INodeGraphRepository NodeGraphs { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            INodeGraphRepository nodeGraphRepository)
        {
            _context = context;

            Projects = projectRepository;
            Users = userRepository;
            NodeGraphs = nodeGraphRepository;
        }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
