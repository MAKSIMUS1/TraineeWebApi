namespace WebApiTrainingProject.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        IUserRepository Users { get; }
        INodeGraphRepository NodeGraphs { get; }

        Task<int> SaveChangesAsync();
    }
}
