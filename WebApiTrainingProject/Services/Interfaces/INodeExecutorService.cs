namespace WebApiTrainingProject.Services.Interfaces
{
    public interface INodeExecutorService
    {
        Task<object> ExecuteGraphAsync(string jsonData);
    }
}
